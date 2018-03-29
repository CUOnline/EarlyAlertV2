namespace RSS.Clients.Canvas.Models.Response
{
    public class EnrollmentResult
    {
        public string Type { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string EnrollmentState { get; set; }
        public string ComputedCurrentScore { get; set; }
        public string ComputedFinalScore { get; set; }
        public string ComputedCurrentGrade { get; set; }
        public string ComputedFinalGrade { get; set; }
        public string UnpostedCurrentScore { get; set; }
        public string UnpostedFinalScore { get; set; }
        public string UnpostedCurrentGrade { get; set; }
        public string UnpostedFinalGrade { get; set; }
    }
}