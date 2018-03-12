using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Models
{
    public class Report : ModelBase
    {
        public string Name { get; set; }
        public ReportType ReportType { get; set; }
        public string Status { get; set; }
        public string ReportData { get; set; }
    }
}