using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HospitalManagement.Models
{
    public class Staff
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Shift { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Status { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? LicenseNumber { get; set; }
        public int? Experience { get; set; }
        public string ProfileImage { get; set; } = string.Empty; // Stores file path
    }
}
