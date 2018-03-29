using System.Text;
using System.Threading.Tasks;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;
using System.Linq;

namespace EarlyAlertV2.Repository
{
    public class AssignmentRepository : RepositoryBase, IAssignmentRepository
    {
        public AssignmentRepository(EarlyAlertV2Context context) : base(context) { }

        public Assignment Add(Assignment model)
        {
            Context.Assignments.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<Assignment> GetAll()
        {
            return Context.Assignments;
        }

        public Assignment Get(int modelId)
        {
            return Context.Assignments.Find(modelId);
        }

        public Assignment GetByCanvasId(int canvasId)
        {
            return Context.Assignments.FirstOrDefault(x => x.CanvasId == canvasId);
        }

        public Assignment Update(Assignment model)
        {
            Context.Assignments.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}