using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;

namespace EarlyAlertV2.Repository
{
    public class ClassRepository : RepositoryBase, IClassRepository
    {
        public ClassRepository(EarlyAlertV2Context context) : base(context) { }

        public Class Add(Class model)
        {
            Context.Classes.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<Class> GetAll()
        {
            return Context.Classes;
        }

        public Class Get(int modelId)
        {
            return Context.Classes.Find(modelId);
        }

        public Class Update(Class model)
        {
            Context.Classes.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}