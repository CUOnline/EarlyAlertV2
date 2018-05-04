using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels.AccountViewModels
{
    public class AddEditUserViewModel
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsReportManagerRole { get; set; }
        public bool IsAdminRole { get; set; }
    }
}
