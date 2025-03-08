// namespace HospitalManagement.Models
// {
//     public class LeaveRequest
//     {
//         public string Id { get; set; } = Guid.NewGuid().ToString();
//         public string StaffId { get; set; } // ID of the staff member
//         public string FirstName { get; set; } // First name of the staff member
//         public string LastName { get; set; } // Last name of the staff member
//         public string Role { get; set; } // Role of the staff member
//         public string LeaveType { get; set; } // Type of leave (e.g., Sick Leave, Vacation)
//         public DateTime StartDate { get; set; } // Start date of the leave
//         public DateTime EndDate { get; set; } // End date of the leave
//         public string Reason { get; set; } // Reason for the leave
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp of the request
//     }
// }

namespace HospitalManagement.Models
{
    public class LeaveRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StaffId { get; set; } // ID of the staff member
        public string FirstName { get; set; } // First name of the staff member
        public string LastName { get; set; } // Last name of the staff member
        public string Role { get; set; } // Role of the staff member
        public string LeaveType { get; set; } // Type of leave (e.g., Sick Leave, Vacation)
        public DateTime StartDate { get; set; } // Start date of the leave
        public DateTime EndDate { get; set; } // End date of the leave
        public string Reason { get; set; } // Reason for the leave
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp of the request
        public string Status { get; set; } = "Pending"; // Status of the leave request, default is "Pending"
    }
}