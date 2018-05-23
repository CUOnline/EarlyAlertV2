using EarlyAlertV2.Models;
using System.Collections.Generic;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface IStudentBLL : IBLL<Student>
    {
        IEnumerable<Student> GetAll(IEnumerable<int> studentIds);
        Student GetByCanvasId(int canvasId);
        Student GetBySisId(string sisId);
    }
}