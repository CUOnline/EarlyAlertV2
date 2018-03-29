using EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface IStudentBLL : IBLL<Student>
    {
        Student GetByCanvasId(int canvasId);
        Student GetBySisId(string sisId);
    }
}