using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EarlyAlertV2.Models;
using EarlyAlertV2.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace EarlyAlertV2.Repository
{
    public class CourseRepository : RepositoryBase, ICourseRepository
    {
        public CourseRepository(EarlyAlertV2Context context) : base(context) { }

        public Course Add(Course model)
        {
            Context.Courses.Add(model);
            Context.SaveChanges();
            return model;
        }

        public IQueryable<Course> GetAll()
        {
            return Context.Courses;
        }

        public Course Get(int modelId)
        {
            return Context.Courses.Find(modelId);
        }

        public Course GetByCanvasId(int canvasId)
        {
            return Context.Courses
                .Include(x => x.StudentCourses)
                .FirstOrDefault(x => x.CanvasId == canvasId);
        }

        public Course Update(Course model)
        {
            Context.Courses.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}