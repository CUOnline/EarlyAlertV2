using EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface IStudentAssignmentSubmissionBLL : IBLL<StudentAssignmentSubmission>
    {
        StudentAssignmentSubmission GetByCanvasId(int canvasId);
    }
}