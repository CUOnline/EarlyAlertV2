using  EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.Repository
{
    public interface IStudentRepository : IRepository<Student>
    {
        Student GetByCanvasId(int canvasId);
        Student GetBySisId(string sisId);
    }
}