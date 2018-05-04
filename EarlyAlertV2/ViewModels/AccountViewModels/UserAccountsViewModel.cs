using EarlyAlertV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarlyAlertV2.ViewModels.AccountViewModels
{
    public class UserAccountsViewModel
    {
        public string SearchFirstName { get; set; }
        public string SearchLastName { get; set; }

        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
