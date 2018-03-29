using System;

namespace RSS.Clients.Canvas.Models.Response
{
    public class SubmissionsResult
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string Grade { get; set; }
        public double? Score { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public bool Late { get; set; }
        public bool Missing { get; set; }
        public bool? Excused { get; set; }
        public string PreviewUrl { get; set; }
        public string WorkflowState { get; set; }
    }
}