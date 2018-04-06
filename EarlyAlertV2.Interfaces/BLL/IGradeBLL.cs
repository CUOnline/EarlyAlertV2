using EarlyAlertV2.Models;

namespace EarlyAlertV2.Interfaces.BLL
{
    public interface IGradeBLL : IBLL<Grade>
    {
        Grade GetByCourseAndStudent(int courseId, int studentId);
    }
}