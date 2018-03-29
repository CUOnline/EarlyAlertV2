using  EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.Repository
{
    public interface ICourseRepository : IRepository<Course>
    {
        Course GetByCanvasId(int canvasId);
    }
}