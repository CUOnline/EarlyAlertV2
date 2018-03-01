using System.Text;
using System.Threading.Tasks;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;
using System.Linq;

namespace EarlyAlertV2.Repository
{
    public class GradeRepository : RepositoryBase, IGradeRepository
    {
        public GradeRepository(EarlyAlertV2Context context) : base(context) { }

        public Grade Add(Grade model)
        {
            Context.Grades.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<Grade> GetAll()
        {
            return Context.Grades;
        }

        public Grade Get(int modelId)
        {
            return Context.Grades.Find(modelId);
        }

        public Grade Update(Grade model)
        {
            Context.Grades.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}