// // Models/Bill.cs
// using System;
// using System.Collections.Generic;

// namespace HospitalManagement.Models
// {
//     public class Bill
//     {
//         public string Id { get; set; } = Guid.NewGuid().ToString();
//         public string AppointmentId { get; set; }
//         public string PatientName { get; set; }
//         public string DoctorName { get; set; }
//         public decimal ConsultancyFee { get; set; }
//         public List<BillItem>? AdditionalItems { get; set; } = new List<BillItem>();
//         public DateTime Date { get; set; } = DateTime.UtcNow;
//         public decimal? TotalAmount => ConsultancyFee + AdditionalItems.Sum(item => item.ItemPrice);
//         public string? PaymentStatus { get; set; } = "Unpaid";
//         public string? PaymentMethod { get; set; }
//     }

//     public class BillItem
//     {
//         public string ItemName { get; set; }
//         public decimal ItemPrice { get; set; }
//     }
// }

// Models/Bill.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.Models
{
    public class Bill
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public decimal ConsultancyFee { get; set; }
        public List<BillItem>? AdditionalItems { get; set; } = new List<BillItem>();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? PaymentStatus { get; set; } = "Unpaid";
        public string? PaymentMethod { get; set; }
        public decimal? TotalBill { get; set; } = null; // TotalBill will now include ConsultancyFee + AdditionalItems
    }

    public class BillItem
    {
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
    }
}