using EarlyAlertV2.Models;
using System.Linq;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface IAssignmentGroupBLL : IBLL<AssignmentGroup>
    {
        AssignmentGroup GetByCanvasId(int canvasId);

        IQueryable<AssignmentGroup> GetAllByCourseId(int courseId);
    }
}