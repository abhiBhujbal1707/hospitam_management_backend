namespace HospitalManagement.Models
{
    public class Appointment
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Time { get; set; } = null!;
        public string? Symptoms { get; set; }
        public decimal? Bill { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}