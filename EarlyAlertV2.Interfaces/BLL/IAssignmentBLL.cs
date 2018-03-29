using EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface IAssignmentBLL : IBLL<Assignment>
    {
        Assignment GetByCanvasId(int canvasId);
    }
}