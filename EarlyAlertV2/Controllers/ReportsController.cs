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

        public ReportsController(ICanvasClient canvasClient, IReportBLL reportBll, IStudentBLL studentBll,
            ICourseBLL courseBll, IAssignmentBLL assignmentBll, IStudentAssignmentSubmissionBLL studentAssignmentSubmissionBll,
            IAssignmentGroupBLL assignmentGroupBll)
        {
            this.canvasClient = canvasClient;
            this.reportBll = reportBll;
            this.studentBll = studentBll;
            this.courseBll = courseBll;
            this.assignmentBll = assignmentBll;
            this.studentAssignmentSubmissionBll = studentAssignmentSubmissionBll;
            this.assignmentGroupBll = assignmentGroupBll;
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

        public async Task<IActionResult> RiskIndex(int reportId)
        {

            var model = new RiskIndexReportViewModel()
            {
                ReportId = reportId,
                Users = new List<UserResult>(),
                EnrollmentResults = new List<EnrollmentResult>()
            };

            if (reportId > 0)
            {
                var report = reportBll.Get(reportId);

                // Get students for report
                var students = await UpdateStudents(report.ReportData);

                if (false)
                {
                    // Determine classes students are currently taking
                    var studentCourses = await UpdateCourses(students);

                    // Get all the assignments for courses
                    await UpdateAssignmentsForCourses(studentCourses);

                    // Get student submissions for assignments
                    var studentSubmissions = await UpdateStudentSubmissions(students, studentCourses);
                }

                foreach (var student in students)
                {
                    var currentStudent = studentBll.Get(student.Id);
                    var activeCourses = currentStudent.StudentCourses.Select(x => x.Course);
                    

                    foreach(var course in activeCourses)
                    {
                        var studentCourseSubmissions = studentAssignmentSubmissionBll.GetAll().Where(x => x.StudentId == currentStudent.Id);
                        var assignmentGroups = assignmentGroupBll.GetAllByCourseId(course.Id);


                        List<Tuple<double, double, double>> groupScore = new List<Tuple<double, double, double>>();

                        // If an assigmentgroup exists where the groupweight is set, calculate score using the weight
                        if(assignmentGroups.Any(x => x.GroupWeight > 0))
                        {
                            foreach (var assignmentGroup in assignmentGroups.Where(x => x.GroupWeight > 0))
                            {
                                double maxGroupPossible = 0;
                                double studentGroupScore = 0;

                                var dueAssignments = assignmentGroup.Assignments.Where(x => x.DueAt != null && x.DueAt <= DateTime.Now);

                                if (dueAssignments.Any())
                                {
                                    foreach (var assignment in dueAssignments)
                                    {
                                        var studentSubmission = studentCourseSubmissions.FirstOrDefault(x => x.AssignmentId == assignment.Id);

                                        if (studentSubmission != null && studentSubmission.Score != null && studentSubmission.WorkflowState != "pending_review")
                                        {
                                            studentGroupScore += studentSubmission.Score ?? 0;
                                            maxGroupPossible += assignment.PointsPossible ?? 0;
                                        }
                                    }

                                    groupScore.Add(new Tuple<double, double, double>(studentGroupScore, maxGroupPossible, assignmentGroup.GroupWeight));
                                }
                            }
                        }
                        else // Otherwise, calculate the score using all of the assignments in the course
                        {
                            foreach(var assignmentGroup in assignmentGroups)
                            {
                                double maxGroupPossible = 0;
                                double studentGroupScore = 0;
                            }
                        }




                        double studentCourseScore = 0;
                        double maxCourseScore = 0;
                        foreach(var score in groupScore)
                        {
                            if (score.Item2 != 0)
                            {
                                studentCourseScore += (score.Item1 / score.Item2) * score.Item3;
                                maxCourseScore += score.Item3;
                            }
                        }

                        studentCourseScore = (studentCourseScore / maxCourseScore) * 100;
                    }

                    // Need to calculate max possible score... summation?


                    // For each student
                        // Calculate score for each course.
                            // For each assignment group in course
                                // Get list of due assignments
                                // Calculate total score of due assignments
                                // Calculate total score of turned in assignments
                                // Divide the two
                                // Multiply score by group weight


                    //foreach(var course in activeCourses)
                    //{
                    //    var courseDueAssignments = course.Assignments.Where(x => x.DueAt != null && x.DueAt >= DateTime.Now);

                    //    var courseLateAssignments = currentStudent.StudentAssignmentSubmissions?.Where(x => x.Assignment.CourseId == course.Id && x.Late).Count();



                    //    var courseScore = currentStudent.StudentAssignmentSubmissions?.Where(x => x.Assignment.CourseId == course.Id && x.Score != null).Sum(x => x.Score) 
                    //        / courseDueAssignments.Where(x => x.PointsPossible != null).Sum(x => x.PointsPossible) * 100;
                    //}

                }


                // OK .... Now... I finally have all the data.  Start calculating scores for students.

                foreach(var user in model.Users)
                {
                    var userActiveCourses = await canvasClient.UsersClient.Courses.GetAll(user.Id, true, true);

                    foreach(var course in userActiveCourses)
                    {
                        var userSubmissions = await canvasClient.CoursesClient.GetAllUserSubmissions(new List<int>() { user.Id }, course.Id);
                        model.EnrollmentResults.AddRange(course.Enrollments);
                    }
                }
            }

            return View(model);
        }

        private async Task<List<Student>> UpdateStudents(string reportData)
        {
            List<Student> students = new List<Student>();

            var studentSidIds = GetStudentSidIds(reportData);

            foreach(var sid in studentSidIds)
            {
                var student = studentBll.GetBySisId(sid);

                // If the student doesn't exist, pull it from the api and add it to the DB.
                if(student == null)
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

        private async Task<List<Course>> UpdateCourses(List<Student> students)
        {
            List<Course> courses = new List<Course>();

            foreach (var student in students)
            {
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
            }

            return courses;
        }

        private async Task UpdateAssignmentsForCourses(List<Course> studentCourses)
        {

            foreach (var studentCourse in studentCourses)
            {
                var courseGroups = await canvasClient.CoursesClient.GetAllAssignmentGroups(studentCourse.CanvasId);

                // Add assignmentGroups
                foreach(var group in courseGroups)
                {
                    var assignmentGroup = assignmentGroupBll.GetByCanvasId(group.Id);

                    if(assignmentGroup == null)
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

                    // Add assignments
                    foreach(var groupAssignment in group.Assignments)
                    {
                        var assignment = assignmentBll.GetByCanvasId(groupAssignment.Id);

                        if(assignment == null)
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
                    }
                }
            }
        }

        private async Task<List<StudentAssignmentSubmission>> UpdateStudentSubmissions(List<Student> students, List<Course> studentCourses)
        {
            var studentAssignmentSubmissions = new List<StudentAssignmentSubmission>();
            foreach (var student in students)
            {
                var currentStudent = studentBll.Get(student.Id);

                foreach(var studentCourse in student.StudentCourses)
                {
                    var course = courseBll.Get(studentCourse.CourseId);

                    IReadOnlyList<UserSubmissionsResult> studentSubmissionResults = new List<UserSubmissionsResult>();
                    try
                    {
                        studentSubmissionResults = await canvasClient.CoursesClient.GetAllUserSubmissions(new List<int>() { student.CanvasId }, studentCourse.Course.CanvasId);
                    }
                    catch (Exception ex)
                    {
                        // Error Code 400 for Yaning Song... a teacher .... why are they on this list?
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

                            studentAssignmentSubmissions.Add(studentSubmission);
                        }
                    }
                }
            }

            return studentAssignmentSubmissions;
        }
    }
}