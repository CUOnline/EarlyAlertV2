using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class Course : ModelBase
    {
        public int CanvasId { get; set; }
        public string Name { get; set; }
        public int EnrollmentTermId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string CourseCode { get; set; }
        
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Grade> StudentGrades { get; set; }
    }
}