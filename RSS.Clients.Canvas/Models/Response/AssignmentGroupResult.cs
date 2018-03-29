using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas.Models.Response
{
    public class AssignmentGroupResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double GroupWeight { get; set; }
        public IReadOnlyCollection<AssignmentResult> Assignments { get; set; }
    }
}
