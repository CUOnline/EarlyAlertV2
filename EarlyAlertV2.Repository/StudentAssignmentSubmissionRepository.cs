using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;

namespace EarlyAlertV2.Repository
{
    public class StudentAssignmentSubmissionRepository : RepositoryBase, IStudentAssignmentSubmissionRepository
    {
        public StudentAssignmentSubmissionRepository(EarlyAlertV2Context context) : base(context) { }

        public StudentAssignmentSubmission Add(StudentAssignmentSubmission model)
        {
            Context.StudentAssignmentSubmissions.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<StudentAssignmentSubmission> GetAll()
        {
            return Context.StudentAssignmentSubmissions;
        }

        public StudentAssignmentSubmission Get(int modelId)
        {
            return Context.StudentAssignmentSubmissions.Find(modelId);
        }

        public StudentAssignmentSubmission GetByCanvasId(int canvasId)
        {
            return Context.StudentAssignmentSubmissions.FirstOrDefault(x => x.CanvasId == canvasId);
        }

        public StudentAssignmentSubmission Update(StudentAssignmentSubmission model)
        {
            Context.StudentAssignmentSubmissions.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}