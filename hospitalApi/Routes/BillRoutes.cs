// // Routes/BillRoutes.cs
// using HospitalManagement.Models;
// using HospitalManagement.Services;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Http;
// using System.Threading.Tasks;

// namespace HospitalManagement.Routes
// {
//     public static class BillRoutes
//     {
//         public static void UseBillRoutes(this WebApplication app)
//         {
//             app.MapPost("/api/bills", async (HttpContext context, BillService billService) =>
//             {
//                 var bill = await context.Request.ReadFromJsonAsync<Bill>();
//                 if (bill == null)
//                 {
//                     return Results.BadRequest("Invalid bill data.");
//                 }

//                 await billService.CreateBillAsync(bill);
//                 return Results.Created($"/api/bills/{bill.Id}", bill);
//             });

//             app.MapPut("/api/bills/{id}/items", async (string id, HttpContext context, BillService billService) =>
//             {
//                 var items = await context.Request.ReadFromJsonAsync<List<BillItem>>();
//                 if (items == null)
//                 {
//                     return Results.BadRequest("Invalid items data.");
//                 }

//                 await billService.AddBillItemsAsync(id, items);
//                 return Results.Ok();
//             });

//             app.MapPut("/api/bills/{id}/payment", async (string id, HttpContext context, BillService billService) =>
//             {
//                 var paymentData = await context.Request.ReadFromJsonAsync<PaymentUpdateRequest>();
//                 if (paymentData == null)
//                 {
//                     return Results.BadRequest("Invalid payment data.");
//                 }

//                 await billService.UpdatePaymentStatusAsync(id, paymentData.PaymentStatus, paymentData.PaymentMethod);
//                 return Results.Ok();
//             });

//             app.MapGet("/api/bills/{id}", async (string id, BillService billService) =>
//             {
//                 var bill = await billService.GetBillByIdAsync(id);
//                 return bill == null ? Results.NotFound() : Results.Ok(bill);
//             });
//         }
//     }

//     public class PaymentUpdateRequest
//     {
//         public string PaymentStatus { get; set; }
//         public string PaymentMethod { get; set; }
//     }
// }

// Routes/BillRoutes.cs
using HospitalManagement.Models;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HospitalManagement.Routes
{
    public static class BillRoutes
    {
        public static void UseBillRoutes(this WebApplication app)
        {
            app.MapPost("/api/bills", async (HttpContext context, BillService billService) =>
            {
                var bill = await context.Request.ReadFromJsonAsync<Bill>();
                if (bill == null)
                {
                    return Results.BadRequest("Invalid bill data.");
                }

                await billService.CreateBillAsync(bill);
                return Results.Created($"/api/bills/{bill.Id}", bill);
            });

            app.MapPut("/api/bills/{id}/items", async (string id, HttpContext context, BillService billService) =>
            {
                var items = await context.Request.ReadFromJsonAsync<List<BillItem>>();
                if (items == null)
                {
                    return Results.BadRequest("Invalid items data.");
                }

                await billService.AddBillItemsAsync(id, items);
                return Results.Ok();
            });

            app.MapPut("/api/bills/{id}/payment", async (string id, HttpContext context, BillService billService) =>
            {
                var paymentData = await context.Request.ReadFromJsonAsync<PaymentUpdateRequest>();
                if (paymentData == null)
                {
                    return Results.BadRequest("Invalid payment data.");
                }

                await billService.UpdatePaymentStatusAsync(id, paymentData.PaymentStatus, paymentData.PaymentMethod);
                return Results.Ok();
            });

            app.MapGet("/api/bills/{id}", async (string id, BillService billService) =>
            {
                var bill = await billService.GetBillByIdAsync(id);
                return bill == null ? Results.NotFound() : Results.Ok(bill);
            });

            // New route to update ConsultancyFee and recalculate TotalBill
            app.MapPut("/api/bills/{id}/consultancyfee", async (string id, HttpContext context, BillService billService) =>
            {
                var feeUpdate = await context.Request.ReadFromJsonAsync<ConsultancyFeeUpdateRequest>();
                if (feeUpdate == null)
                {
                    return Results.BadRequest("Invalid consultancy fee data.");
                }

                var bill = await billService.GetBillByIdAsync(id);
                if (bill == null)
                {
                    return Results.NotFound();
                }

                bill.ConsultancyFee = feeUpdate.ConsultancyFee;
                await billService.UpdateBillAsync(id, bill); // This will recalculate TotalBill
                return Results.Ok();
            });
        }
    }

    public class PaymentUpdateRequest
    {
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class ConsultancyFeeUpdateRequest
    {
        public decimal ConsultancyFee { get; set; }
    }
}