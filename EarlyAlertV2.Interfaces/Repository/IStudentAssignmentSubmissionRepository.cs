using  EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.Repository
{
    public interface IStudentAssignmentSubmissionRepository : IRepository<StudentAssignmentSubmission>
    {
        StudentAssignmentSubmission GetByCanvasId(int canvasId);
    }
}