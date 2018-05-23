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
    public class AssignmentsClient : ApiClient, IAssignmentsClient
    {
        public AssignmentsClient(IApiConnection apiConnection)
            : base(apiConnection)
        {

        }

        public Task<AssignmentResult> Get(int assignmentId)
        {
            return ApiConnection.Get<AssignmentResult>(ApiUrls.Assignment(assignmentId));
        }

        /// <summary>
        /// Get all assignments for course
        /// </summary>
        public Task<IReadOnlyList<AssignmentResult>> GetAll(int courseId)
        {
            return ApiConnection.GetAll<AssignmentResult>(ApiUrls.CourseAssignments(courseId));
        }
    }
}
