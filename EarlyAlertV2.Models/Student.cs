using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class Student : ModelBase
    {
        public int CanvasId { get; set; }
        public string SISUserId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<StudentAssignmentSubmission> StudentAssignmentSubmissions { get; set; }
    }
}