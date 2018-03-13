using System;
using System.Collections.Generic;
using System.Text;

namespace RSS.Clients.Canvas.Models.Response
{
    public class CourseResult
    {
        public int Id { get; set; }
        public string SisCourseId { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public string Uuid { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string CourseCode { get; set; }
        public int RootAccountId { get; set; }
        public int EnrollmentTermId { get; set; }
        public IReadOnlyList<EnrollmentResult> Enrollments { get; set; }
        public string WorkflowState { get; set; }

    }
}
