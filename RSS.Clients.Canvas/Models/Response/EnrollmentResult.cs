namespace RSS.Clients.Canvas.Models.Response
{
    public class EnrollmentResult
    {
        public string Type { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string EnrollmentState { get; set; }
        public double? ComputedCurrentScore { get; set; }
        public double? ComputedFinalScore { get; set; }
        public double? ComputedCurrentGrade { get; set; }
        public double? ComputedFinalGrade { get; set; }
        public double? UnpostedCurrentScore { get; set; }
        public double? UnpostedFinalScore { get; set; }
        public double? UnpostedCurrentGrade { get; set; }
        public double? UnpostedFinalGrade { get; set; }
    }
}