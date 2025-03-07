// using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;

// namespace HospitalManagement.Models
// {
//     public class Patient
//     {
//         [BsonId]
//         [BsonRepresentation(BsonType.ObjectId)]
//         public string? Id { get; set; }

//         public string FirstName { get; set; } = string.Empty;
//         public string LastName { get; set; } = string.Empty;
//         public string Gender { get; set; } = string.Empty;
//         public string Phone { get; set; } = string.Empty;
//         public string Email { get; set; } = string.Empty;
//         public string Address { get; set; } = string.Empty;
//         public string PasswordHash { get; set; } = string.Empty;
//         public DateTime Dob { get; set; }
//         public string BloodGroup { get; set; } = string.Empty;
//         public string EmergencyContact { get; set; } = string.Empty;
//         public string MedicalHistory { get; set; } = string.Empty;
//     }
// }
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HospitalManagement.Models
{
    public class Patient
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
        public string BloodGroup { get; set; } = string.Empty;
        public string EmergencyContact { get; set; } = string.Empty;
        public string MedicalHistory { get; set; } = string.Empty;
    }
}
