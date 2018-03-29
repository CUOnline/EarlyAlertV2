using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas.Models.Response
{
    public class AssignmentResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DueAt { get; set; }
        public DateTime? UnlocksAt { get; set; }
        public DateTime? LockAt { get; set; }
        public double? PointsPossible { get; set; }
        public int CourseId { get; set; }
        public string HtmlUrl { get; set; }
    }
}
