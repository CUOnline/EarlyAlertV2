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
        public Task<List<CourseResult>> GetAll(string userId)
    }
}

// Current Grade of student
// - Get Courses student is signed up for
//      - Get all assignments for course.
//          - Determine which assignments should be turned in already (for Late/Missing Assignments)
//      - Get assignments submitted by user
//          - Get graded assignments
//          - Get ungraded assignments


// Missing Assignments
// Late Assignments


// Activity
// Communication
// Participation
// Page Views
// Possibly - How many courses is the student currently taking?