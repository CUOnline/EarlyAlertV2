using RSS.Clients.Canvas.Helpers;
using RSS.Clients.Canvas.Interfaces.Client;
using RSS.Clients.Canvas.Interfaces.Http;
using RSS.Clients.Canvas.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RSS.Clients.Canvas.Clients
{
    public class CoursesClient : ApiClient, ICoursesClient
    {
        public CoursesClient(IApiConnection apiConnection)
            : base(apiConnection)
        {
            Assignments = new AssignmentsClient(apiConnection);
        }

        public IAssignmentsClient Assignments { get; private set; }

        public Task<CourseResult> Get(string courseId)
        {
            Ensure.ArgumentNotNull(courseId, "courseId");

            return ApiConnection.Get<CourseResult>(ApiUrls.Course(courseId));
        }

        /// <summary>
        /// Get a list of courses where the given user is currently enrolled
        /// </summary>
        public Task<IReadOnlyList<CourseResult>> GetAll(int userId, bool includeTotalScores, bool activeCourses)
        {
            var parameters = new Dictionary<string, string>();
            if (includeTotalScores)
            {
                parameters.Add("include[]", "total_scores");
            }

            if (activeCourses)
            {
                parameters.Add("enrollment_state", "active");
            }

            return ApiConnection.GetAll<CourseResult>(ApiUrls.UserCourses(userId), parameters);
        }

        public Task<IReadOnlyList<UserSubmissionsResult>> GetAllUserSubmissions(List<int> userId, int courseId)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("grouped", "1");
            parameters.Add("student_ids[]", string.Join(",", userId));
            
            return ApiConnection.GetAll<UserSubmissionsResult>(ApiUrls.UserSubmissions(courseId), parameters);
        }

        public Task<IReadOnlyList<AssignmentGroupResult>> GetAllAssignmentGroups(int courseId)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("include[]", "assignments");
            
            return ApiConnection.GetAll<AssignmentGroupResult>(ApiUrls.AssignmentGroups(courseId), parameters);
        }
    }
}