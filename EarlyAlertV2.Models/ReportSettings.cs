using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class ReportSettings : ModelBase
    {
        [Display(Name = "Average Grade")]
        public double GradeWeight { get; set; }

        [Display(Name = "Missing Assignments")]
        public double MissedAssignmentsWeight { get; set; }

        [Display(Name = "Late Assignments")]
        public double LateAssignmentsWeight { get; set; }

        [Display(Name = "Activity")]
        public double ActivityWeight { get; set; }

        [Display(Name = "Communication")]
        public double CommunicationWeight { get; set; }

        [Display(Name = "Participation")]
        public double ParticipationWeight { get; set; }

        [Display(Name = "Page Views")]
        public double PageViewsWeight { get; set; }

        [Display(Name = "Number of Active Courses")]
        public double NumberOfActiveCoursesWeight { get; set; }
    }
}