using System;
using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class Assignment : ModelBase
    {
        public int CanvasId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? PointsPossible { get; set; }
        public DateTime? DueAt { get; set; }
        public DateTime? UnlockAt { get; set; }
        public DateTime? LockAt { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int AssignmentGroupId { get; set; }
        public virtual AssignmentGroup AssignmentGroup { get; set; }
    }
}