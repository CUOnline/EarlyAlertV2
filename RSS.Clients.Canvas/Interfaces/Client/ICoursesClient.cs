using RSS.Clients.Canvas.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RSS.Clients.Canvas.Interfaces.Client
{
    public interface ICoursesClient
    {
        Task<CourseResult> Get(string courseId);

        IAssignmentsClient Assignments { get; }

        Task<IReadOnlyList<CourseResult>> GetAll(int userId, bool includeTotalScores, bool activeCourses);

        Task<IReadOnlyList<UserSubmissionsResult>> GetAllUserSubmissions(List<int> userId, int courseId);

        Task<IReadOnlyList<AssignmentGroupResult>> GetAllAssignmentGroups(int courseId);
    }
}
