using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace EarlyAlertV2.Repository
{
    public class AssignmentGroupRepository : RepositoryBase, IAssignmentGroupRepository
    {
        public AssignmentGroupRepository(EarlyAlertV2Context context) : base(context) { }

        public AssignmentGroup Add(AssignmentGroup model)
        {
            Context.AssignmentGroups.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<AssignmentGroup> GetAll()
        {
            return Context.AssignmentGroups;
        }

        public IQueryable<AssignmentGroup> GetAllByCourseId(int courseId)
        {
            return Context.AssignmentGroups
                .Where(x => x.CourseId == courseId)
                .Include(x => x.Assignments);
        }

        public AssignmentGroup Get(int modelId)
        {
            return Context.AssignmentGroups.Find(modelId);
        }

        public AssignmentGroup GetByCanvasId(int canvasId)
        {
            return Context.AssignmentGroups.FirstOrDefault(x => x.CanvasId == canvasId);
        }

        public AssignmentGroup Update(AssignmentGroup model)
        {
            Context.AssignmentGroups.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}