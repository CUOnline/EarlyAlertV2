using System;
using System.Collections.Generic;
using System.Text;

namespace EarlyAlertV2.Models
{
    public abstract class ModelBase
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
