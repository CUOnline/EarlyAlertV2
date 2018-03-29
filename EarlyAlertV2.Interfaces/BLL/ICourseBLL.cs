using EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface ICourseBLL : IBLL<Course>
    {
        Course GetByCanvasId(int canvasId);
    }
}