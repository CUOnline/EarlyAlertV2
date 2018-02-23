using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EarlyAlertV2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string FullNameReversed => $"{LastName}, {FirstName}";

        //public virtual ICollection<ApplicationUserProgram> ApplicationUserPrograms { get; set; }
    }
}
