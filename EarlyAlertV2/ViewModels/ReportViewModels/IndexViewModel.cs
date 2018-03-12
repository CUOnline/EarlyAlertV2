using EarlyAlertV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels.ReportViewModels
{
    public class IndexViewModel
    {
        public List<Report> Reports { get; set; }
        public AddEditReportViewModel AddEditReportViewModel { get; set; }
    }
}
