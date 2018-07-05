using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Models;
using EarlyAlertV2.ViewModels.ReportViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSS.Clients.Canvas;
using RSS.Clients.Canvas.Exceptions;
using RSS.Clients.Canvas.Models.Response;

namespace EarlyAlertV2.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ICanvasClient canvasClient;
        private readonly IReportBLL reportBll;
        private readonly IStudentBLL studentBll;
        private readonly ICourseBLL courseBll;
        private readonly IAssignmentBLL assignmentBll;
        private readonly IStudentAssignmentSubmissionBLL studentAssignmentSubmissionBll;
        private readonly IAssignmentGroupBLL assignmentGroupBll;
        private readonly IGradeBLL gradeBll;
        private readonly IReportSettingsBLL reportSettingsBll;

        public ReportsController(ICanvasClient canvasClient, IReportBLL reportBll, IStudentBLL studentBll,
            ICourseBLL courseBll, IAssignmentBLL assignmentBll, IStudentAssignmentSubmissionBLL studentAssignmentSubmissionBll,
            IAssignmentGroupBLL assignmentGroupBll, IGradeBLL gradeBll, IReportSettingsBLL reportSettingsBll)
        {
            this.canvasClient = canvasClient;
            this.reportBll = reportBll;
            this.studentBll = studentBll;
            this.courseBll = courseBll;
            this.assignmentBll = assignmentBll;
            this.studentAssignmentSubmissionBll = studentAssignmentSubmissionBll;
            this.assignmentGroupBll = assignmentGroupBll;
            this.gradeBll = gradeBll;
            this.reportSettingsBll = reportSettingsBll;
        }

        public IActionResult Index()
        {
            var reports = reportBll.GetAll();

            var model = new IndexViewModel();
            model.Reports = reports.ToList();
            model.AddEditReportViewModel = new AddEditReportViewModel();
            model.AddEditReportViewModel.ReportType = ReportType.RiskIndex;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditReport(IndexViewModel model, IFormFile reportData)
        {
            if (ModelState.IsValid && reportData != null && reportData.Length > 0)
            {
                string fileContents;
                using (var reader = new StreamReader(reportData.OpenReadStream()))
                {
                    fileContents = await reader.ReadToEndAsync();
                }

                reportBll.Add(new Report()
                {
                    Name = model.AddEditReportViewModel.Name,
                    ReportType = model.AddEditReportViewModel.ReportType,
                    Status = "Pending Creation",
                    ReportData = fileContents
                });
            }

            return RedirectToAction("Index");
        }

        public IActionResult StudentProfile(int id)
        {
            var student = studentBll.Get(id);

            var model = new StudentProfileViewModel()
            {
                StudentId = id,
                Student = student
            };

            return View(model);
        }

        public IActionResult RiskIndex(int reportId, bool? refreshData)
        {

            var model = new RiskIndexReportViewModel()
            {
                ReportId = reportId,
                Users = new List<Student>(),
                UserRiskIndicies = new Dictionary<int, double>()
            };

            return View(model);
        }

        public async Task<JsonResult> GetStudents(int reportId)
        {
            var report = reportBll.Get(reportId);
            var users = await UpdateStudents(report.ReportData);

            return Json(users.Select(x => new
            {
                x.Id,
                x.Name,
                RiskIndex = 0
            }));
        }

        public JsonResult GetStudentRiskIndex(int userId)
        {
            var user = studentBll.Get(userId);
            return Json(CalculateUserRiskIndex(user));
        }

        public async Task<JsonResult> UpdateStudentCourses(int studentId)
        {
            List<Course> courses = new List<Course>();
            var student = studentBll.Get(studentId);

            // clean up student's active courses
            student.StudentCourses = new List<StudentCourse>();
            studentBll.Update(student);

            // Get active courses for student
            var studentActiveCourses = await canvasClient.UsersClient.Courses.GetAll(student.CanvasId, includeTotalScores: false, activeCourses: true);

            foreach (var activeCourse in studentActiveCourses)
            {
                var course = courseBll.GetByCanvasId(activeCourse.Id);

                // If the course doesn't exist, add it to the DB.
                if (course == null)
                {
                    course = courseBll.Add(new Course()
                    {
                        CanvasId = activeCourse.Id,
                        Name = activeCourse.Name,
                        EnrollmentTermId = activeCourse.EnrollmentTermId,
                        CourseCode = activeCourse.CourseCode,
                        StartAt = activeCourse.StartAt,
                        EndAt = activeCourse.EndAt,
                        StudentCourses = new List<StudentCourse>()
                    });
                }

                // Add student to course
                if (!course.StudentCourses.Any(x => x.StudentId == student.Id))
                {
                    course.StudentCourses.Add(new StudentCourse()
                    {
                        CourseId = course.Id,
                        StudentId = student.Id
                    });

                    courseBll.Update(course);
                }

                courses.Add(course);
            }

            return Json(courses.Select(x => new { x.Id }));
        }

        public async Task<JsonResult> UpdateAssignmentsForCourse(int courseId)
        {
            var studentCourse = courseBll.Get(courseId);
            var courseGroups = await canvasClient.CoursesClient.GetAllAssignmentGroups(studentCourse.CanvasId);

            // Add assignmentGroups
            foreach (var group in courseGroups)
            {
                var assignmentGroup = assignmentGroupBll.GetByCanvasId(group.Id);

                if (assignmentGroup == null)
                {
                    assignmentGroup = assignmentGroupBll.Add(new AssignmentGroup()
                    {
                        CanvasId = group.Id,
                        CourseId = studentCourse.Id,
                        Name = group.Name,
                        GroupWeight = group.GroupWeight,
                        Assignments = new List<Assignment>()
                    });
                }
                else
                {
                    assignmentGroup.Assignments = new List<Assignment>();
                    assignmentGroup = assignmentGroupBll.Update(assignmentGroup);
                }

                // Add assignments
                foreach (var groupAssignment in group.Assignments)
                {
                    var assignment = assignmentBll.GetByCanvasId(groupAssignment.Id);

                    if (assignment == null)
                    {
                        var course = courseBll.GetByCanvasId(groupAssignment.CourseId);

                        assignment = assignmentBll.Add(new Assignment()
                        {
                            CanvasId = groupAssignment.Id,
                            CourseId = course.Id,
                            AssignmentGroupId = assignmentGroup.Id,
                            Name = groupAssignment.Name,
                            Description = groupAssignment.Description,
                            DueAt = groupAssignment.DueAt,
                            LockAt = groupAssignment.LockAt,
                            UnlockAt = groupAssignment.UnlocksAt,
                            PointsPossible = groupAssignment.PointsPossible
                        });
                    }
                    else
                    {
                        assignment.DueAt = groupAssignment.DueAt;
                        assignment.LockAt = groupAssignment.LockAt;
                        assignment.UnlockAt = groupAssignment.UnlocksAt;
                        assignment.PointsPossible = groupAssignment.PointsPossible;
                        assignment = assignmentBll.Update(assignment);
                    }
                }
            }

            return Json("Ok");
        }

        public async Task<JsonResult> UpdateStudentSubmissionsForCourse(int studentId)
        {
            var student = studentBll.Get(studentId);

            var studentAssignmentSubmissions = new List<StudentAssignmentSubmission>();

            var currentStudent = studentBll.Get(student.Id);
            currentStudent.StudentAssignmentSubmissions = new List<StudentAssignmentSubmission>();
            studentBll.Update(currentStudent);

            foreach (var studentCourse in student.StudentCourses)
            {
                var course = courseBll.Get(studentCourse.CourseId);

                IReadOnlyList<UserSubmissionsResult> studentSubmissionResults = new List<UserSubmissionsResult>();
                try
                {
                    studentSubmissionResults = await canvasClient.CoursesClient.GetAllUserSubmissions(new List<int>() { student.CanvasId }, studentCourse.Course.CanvasId);
                }
                catch (AuthorizationException)
                {
                    // Supress unauthroized exception.  Ran into this when running Yaning Song, who is a teacher for a sandbox course.
                }

                foreach (var submissionResult in studentSubmissionResults)
                {
                    foreach (var submission in submissionResult.Submissions)
                    {
                        var studentSubmission = studentAssignmentSubmissionBll.GetByCanvasId(submission.Id);

                        if (studentSubmission == null)
                        {
                            var assignment = assignmentBll.GetByCanvasId(submission.AssignmentId);

                            studentSubmission = studentAssignmentSubmissionBll.Add(new StudentAssignmentSubmission()
                            {
                                StudentId = student.Id,
                                AssignmentId = assignment.Id,
                                CanvasId = submission.Id,
                                CanvasAssignmentId = submission.AssignmentId,
                                CanvasUserId = student.CanvasId,
                                Score = submission.Score,
                                Late = submission.Late,
                                Missing = submission.Missing,
                                Excused = submission.Excused,
                                WorkflowState = submission.WorkflowState
                            });
                        }
                        else
                        {
                            studentSubmission.Score = submission.Score;
                            studentSubmission.Late = submission.Late;
                            studentSubmission.Missing = submission.Missing;
                            studentSubmission.Excused = submission.Excused;
                            studentSubmission.WorkflowState = submission.WorkflowState;

                            studentSubmission = studentAssignmentSubmissionBll.Update(studentSubmission);
                        }

                        studentAssignmentSubmissions.Add(studentSubmission);
                    }
                }
            }

            return Json("Ok");
        }

        public JsonResult UpdateStudentGrades(int studentId)
        {
            var currentStudent = studentBll.Get(studentId);

            // Clean out old grades
            currentStudent.CourseGrades = new List<Grade>();
            currentStudent = studentBll.Update(currentStudent);

            var activeCourses = currentStudent.StudentCourses.Select(x => x.Course);

            foreach (var course in activeCourses)
            {
                var studentCourseSubmissions = studentAssignmentSubmissionBll.GetAll().Where(x => x.StudentId == currentStudent.Id);
                var assignmentGroups = assignmentGroupBll.GetAllByCourseId(course.Id);

                // If calculation is using course weights
                double average = 0;
                if (assignmentGroups.Select(x => x.GroupWeight).Any(weight => weight > 0))
                {
                    foreach (var assignmentGroup in assignmentGroups)
                    {
                        average += GetStudentAverage(assignmentGroup.Assignments.ToList(), studentCourseSubmissions.ToList(), assignmentGroup.GroupWeight);
                    }
                }
                else // calculate using all due assignments in course.  All assignments count toward 100% of the course.
                {
                    average = GetStudentAverage(assignmentGroups.SelectMany(x => x.Assignments).ToList(), studentCourseSubmissions.ToList(), 100);
                }

                var grade = gradeBll.GetByCourseAndStudent(course.Id, studentId);
                if (grade == null)
                {
                    gradeBll.Add(new Grade()
                    {
                        CourseId = course.Id,
                        StudentId = studentId,
                        Value = average
                    });
                }
                else
                {
                    grade.Value = average;
                    gradeBll.Update(grade);
                }
            }

            return Json("Ok");
        }

        public async Task<JsonResult> UpdateStudentActivity(int studentId)
        {
            var user = studentBll.Get(studentId);
            var latestPageView = await canvasClient.UsersClient.GetLatestPageView(user.SISUserId);
            user.LatestActivity = latestPageView.FirstOrDefault()?.CreatedAt;
            studentBll.Update(user);

            return Json("Ok");
        }

        private double CalculateUserRiskIndex(Student user)
        {
            var reportSettings = reportSettingsBll.GetAll().FirstOrDefault();

            var averageGradeIndex = GetOverallAverageGrade(user) * (reportSettings.GradeWeight / 100);
            var lateAssignmentsIndex = GetAverageLateAssignments(user) * (reportSettings.LateAssignmentsWeight / 100);
            var missingAssignmentsIndex = GetAverageMissingAssignments(user) * (reportSettings.MissedAssignmentsWeight / 100);
            var activityIndex = GetActivityPercentage(user, reportSettings.ActivityTimeMin, reportSettings.ActivityTimeMax) * (reportSettings.ActivityWeight / 100);

            return 100 - (averageGradeIndex
                        + lateAssignmentsIndex
                        + missingAssignmentsIndex
                        + activityIndex);
        }

        private double GetOverallAverageGrade(Student user)
        {
            if (!user.CourseGrades.Any())
            {
                return 100;
            }

            double totalCourseGrade = 0;

            foreach (var courseGrade in user.CourseGrades)
            {
                totalCourseGrade += courseGrade.Value;
            }

            return (totalCourseGrade / user.CourseGrades.Count);
        }

        private double GetAverageLateAssignments(Student user)
        {
            double totalSubmissions = user.StudentAssignmentSubmissions.Count();

            if (totalSubmissions == 0)
            {
                return 100;
            }

            return (user.StudentAssignmentSubmissions.Where(x => !x.Late).Count() / totalSubmissions) * 100;
        }

        private double GetAverageMissingAssignments(Student user)
        {
            double totalSubmissions = user.StudentAssignmentSubmissions.Count();

            if (totalSubmissions == 0)
            {
                return 100;
            }

            return (user.StudentAssignmentSubmissions.Where(x => !x.Missing).Count() / totalSubmissions) * 100;
        }

        private double GetActivityPercentage(Student user, int activityTimeMin, int activityTimeMax)
        {
            if (user.LatestActivity.HasValue)
            {
                var daysSinceActivity = user.LastUpdated.Value.Day - user.LatestActivity.Value.Day;

                if (daysSinceActivity <= activityTimeMin)
                {
                    return 100;
                }
                else if (daysSinceActivity >= activityTimeMax)
                {
                    return 0;
                }
                else
                {
                    return ((daysSinceActivity - activityTimeMax) / (activityTimeMin - activityTimeMax)) * 100;
                }
            }

            // Student has no activity
            return 0;
        }

        private double GetStudentAverage(List<Assignment> assignments, List<StudentAssignmentSubmission> studentSubmissions, double weight)
        {
            double totalStudentGrade = 0;
            double maxPossibleGrade = 0;
            foreach (var assignment in assignments)
            {
                var studentAssignmentSubmission = studentSubmissions.FirstOrDefault(x => x.AssignmentId == assignment.Id);
                if (studentAssignmentSubmission != null
                    && studentAssignmentSubmission.Score != null
                    && studentAssignmentSubmission.WorkflowState == "graded"
                    && studentAssignmentSubmission.Assignment.PointsPossible != null)
                {
                    totalStudentGrade += studentAssignmentSubmission.Score.Value;
                    maxPossibleGrade += studentAssignmentSubmission.Assignment.PointsPossible.Value;
                }
            }

            // Handle potential edge case where no assignments are due for a course group. Example: Final Exam
            if (maxPossibleGrade == 0)
            {
                return weight;
            }
            else
            {
                return (totalStudentGrade / maxPossibleGrade) * weight;
            }
        }

        private async Task<List<Student>> UpdateStudents(string reportData)
        {
            List<Student> students = new List<Student>();

            var studentSidIds = GetStudentSidIds(reportData);

            foreach (var sid in studentSidIds)
            {
                var student = studentBll.GetBySisId(sid);

                // If the student doesn't exist, pull it from the api and add it to the DB.
                if (student == null)
                {
                    // NEED TO HANDLE ERRORS WHERE STUDENT DOES NOT EXIST
                    var newStudent = await canvasClient.UsersClient.Get(sid);

                    student = studentBll.Add(new Student()
                    {
                        CanvasId = newStudent.Id,
                        SISUserId = sid,
                        Name = newStudent.Name
                    });
                }

                students.Add(student);
            }

            return students;
        }

        private IEnumerable<string> GetStudentSidIds(string reportData)
        {
            var studentData = reportData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var data in studentData.Skip(1)) // First index contains headers
            {
                var student = data.Split(",", StringSplitOptions.RemoveEmptyEntries);
                yield return student[1];
            }
        }
    }
}