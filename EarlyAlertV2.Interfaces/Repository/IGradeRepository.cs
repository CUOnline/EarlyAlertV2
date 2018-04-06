using  EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.Repository
{
    public interface IGradeRepository : IRepository<Grade>
    {
        Grade GetByCourseAndStudent(int courseId, int studentId);
    }
}