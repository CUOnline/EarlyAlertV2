using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class StudentAssignmentSubmission : ModelBase
    {
        public int CanvasId { get; set; }
        public int CanvasUserId { get; set; }
        public int CanvasAssignmentId { get; set; }
        public double? Score { get; set; }
        public bool Late { get; set; }
        public bool Missing { get; set; }
        public bool? Excused { get; set; }
        public string WorkflowState { get; set; }

        public int StudentId { get; set; }
        public int AssignmentId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}