using RSS.Clients.Canvas.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels.ReportViewModels
{
    public class RiskIndexReportViewModel
    {
        public int ReportId { get; set; }
        public List<UserResult> Users { get; set; }
    }
}
