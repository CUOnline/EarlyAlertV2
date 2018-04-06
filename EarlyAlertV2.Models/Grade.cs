using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class Grade : ModelBase
    {
        public double Value { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}