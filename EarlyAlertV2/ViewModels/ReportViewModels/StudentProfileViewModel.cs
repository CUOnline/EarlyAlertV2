using EarlyAlertV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels.ReportViewModels
{
    public class StudentProfileViewModel
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }
    }
}
