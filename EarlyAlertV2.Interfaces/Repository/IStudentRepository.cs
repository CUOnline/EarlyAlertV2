using  EarlyAlertV2.Models;
using System.Collections.Generic;
using System.Linq;

namespace EarlyAlertV2.Interfaces.Repository
{
    public interface IStudentRepository : IRepository<Student>
    {
        IQueryable<Student> GetAll(IEnumerable<int> studentIds);
        Student GetByCanvasId(int canvasId);
        Student GetBySisId(string sisId);
    }
}