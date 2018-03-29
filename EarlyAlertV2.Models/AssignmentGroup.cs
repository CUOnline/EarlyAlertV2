using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class AssignmentGroup : ModelBase
    {
        public int CanvasId { get; set; }
        public string Name { get; set; }
        public double GroupWeight { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
    }
}