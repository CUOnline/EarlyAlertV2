using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Repository
{
    public class StudentRepository : RepositoryBase, IStudentRepository
    {
        public StudentRepository(EarlyAlertV2Context context) : base(context) { }

        public Student Add(Student model)
        {
            Context.Students.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<Student> GetAll()
        {
            return Context.Students;
        }

        public Student Get(int modelId)
        {
            return Context.Students.Find(modelId);
        }

        public Student Update(Student model)
        {
            Context.Students.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}