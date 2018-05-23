using EarlyAlertV2.Interfaces.Repository;
using EarlyAlertV2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public IQueryable<Student> GetAll(IEnumerable<int> studentIds)
        {
            return Context.Students
                .Include(x => x.StudentCourses)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.Assignments)
                .Include(x => x.StudentAssignmentSubmissions)
                .Include(x => x.CourseGrades)
                    .ThenInclude(x => x.Course)
                .Where(x => studentIds.Contains(x.Id));
        }

        public Student Get(int modelId)
        {
            return Context.Students
                .Include(x => x.StudentCourses)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.Assignments)
                .Include(x => x.StudentAssignmentSubmissions)
                .Include(x => x.CourseGrades)
                    .ThenInclude(x => x.Course)
                .FirstOrDefault(x => x.Id == modelId);
        }

        public Student GetBySisId(string sisId)
        {
            return Context.Students
                .Include(x => x.StudentCourses)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.Assignments)
                .Include(x => x.StudentAssignmentSubmissions)
                .Include(x => x.CourseGrades)
                    .ThenInclude(x => x.Course)
                .FirstOrDefault(x => x.SISUserId == sisId);
        }

        public Student GetByCanvasId(int canvasId)
        {
            return Context.Students.FirstOrDefault(x => x.CanvasId == canvasId);
        }

        public Student Update(Student model)
        {
            Context.Students.Update(model);
            Context.SaveChanges();
            return model;
        }
    }
}