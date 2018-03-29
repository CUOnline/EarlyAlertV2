using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas.Models.Response
{
    public class UserSubmissionsResult
    {
        public int UserId { get; set; }
        public int SectionId { get; set; }
        
        public IReadOnlyList<SubmissionsResult> Submissions { get; set; }
    }
}
