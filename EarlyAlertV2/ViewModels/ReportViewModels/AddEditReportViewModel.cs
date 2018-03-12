using EarlyAlertV2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels.ReportViewModels
{
    public class AddEditReportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ReportType ReportType { get; set; }
    }
}
