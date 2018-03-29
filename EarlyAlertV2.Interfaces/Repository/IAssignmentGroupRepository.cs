using  EarlyAlertV2.Models;
using System.Linq;

namespace EarlyAlertV2.Interfaces.Repository
{
    public interface IAssignmentGroupRepository : IRepository<AssignmentGroup>
    {
        AssignmentGroup GetByCanvasId(int canvasId);

        IQueryable<AssignmentGroup> GetAllByCourseId(int courseId);
    }
}