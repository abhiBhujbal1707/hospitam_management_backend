// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Routing;
// using HospitalManagement.Models;
// using HospitalManagement.Services;

// public static class LeaveRequestEndpoints
// {
//     public static void MapLeaveRequestRoutes(this WebApplication app)
//     {
//         // Existing route to submit a leave request
//         app.MapPost("/api/leaverequest", async (HttpContext context, LeaveRequestService leaveRequestService, JwtService jwtService) =>
//         {
//             var leaveRequestData = await context.Request.ReadFromJsonAsync<LeaveRequest>();

//             if (leaveRequestData == null)
//             {
//                 Console.WriteLine("Invalid leave request data.");
//                 return Results.BadRequest("Invalid leave request data.");
//             }

//             // Log the incoming request
//             Console.WriteLine("Received leave request:");
//             Console.WriteLine($"StaffId: {leaveRequestData.StaffId}");
//             Console.WriteLine($"FirstName: {leaveRequestData.FirstName}");
//             Console.WriteLine($"LastName: {leaveRequestData.LastName}");
//             Console.WriteLine($"Role: {leaveRequestData.Role}");
//             Console.WriteLine($"LeaveType: {leaveRequestData.LeaveType}");
//             Console.WriteLine($"StartDate: {leaveRequestData.StartDate}");
//             Console.WriteLine($"EndDate: {leaveRequestData.EndDate}");
//             Console.WriteLine($"Reason: {leaveRequestData.Reason}");
//             Console.WriteLine($"Status: {leaveRequestData.Status}"); // Log the status

//             // Save the leave request to MongoDB
//             try
//             {
//                 await leaveRequestService.InsertLeaveRequestAsync(leaveRequestData);
//                 return Results.Ok(new { message = "Leave request submitted successfully!" });
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error saving leave request: {ex.Message}");
//                 return Results.Problem("Failed to submit leave request.");
//             }
//         });

//         // New route to fetch all pending leave requests
//         app.MapGet("/api/leaverequest/pending", async (LeaveRequestService leaveRequestService) =>
//         {
//             try
//             {
//                 // Fetch all leave requests with status "Pending"
//                 var pendingRequests = await leaveRequestService.GetPendingLeaveRequestsAsync();
//                 return Results.Ok(pendingRequests);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"Error fetching pending leave requests: {ex.Message}");
//                 return Results.Problem("Failed to fetch pending leave requests.");
//             }
//         });
//     }
// }

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using HospitalManagement.Models;
using HospitalManagement.Services;

public static class LeaveRequestEndpoints
{
    public static void MapLeaveRequestRoutes(this WebApplication app)
    {
        // Existing route to submit a leave request
        app.MapPost("/api/leaverequest", async (HttpContext context, LeaveRequestService leaveRequestService, JwtService jwtService) =>
        {
            var leaveRequestData = await context.Request.ReadFromJsonAsync<LeaveRequest>();

            if (leaveRequestData == null)
            {
                Console.WriteLine("Invalid leave request data.");
                return Results.BadRequest("Invalid leave request data.");
            }

            // Log the incoming request
            Console.WriteLine("Received leave request:");
            Console.WriteLine($"StaffId: {leaveRequestData.StaffId}");
            Console.WriteLine($"FirstName: {leaveRequestData.FirstName}");
            Console.WriteLine($"LastName: {leaveRequestData.LastName}");
            Console.WriteLine($"Role: {leaveRequestData.Role}");
            Console.WriteLine($"LeaveType: {leaveRequestData.LeaveType}");
            Console.WriteLine($"StartDate: {leaveRequestData.StartDate}");
            Console.WriteLine($"EndDate: {leaveRequestData.EndDate}");
            Console.WriteLine($"Reason: {leaveRequestData.Reason}");
            Console.WriteLine($"Status: {leaveRequestData.Status}"); // Log the status

            // Save the leave request to MongoDB
            try
            {
                await leaveRequestService.InsertLeaveRequestAsync(leaveRequestData);
                return Results.Ok(new { message = "Leave request submitted successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving leave request: {ex.Message}");
                return Results.Problem("Failed to submit leave request.");
            }
        });

        // Route to fetch all pending leave requests
        app.MapGet("/api/leaverequest/pending", async (LeaveRequestService leaveRequestService) =>
        {
            try
            {
                // Fetch all leave requests with status "Pending"
                var pendingRequests = await leaveRequestService.GetPendingLeaveRequestsAsync();
                return Results.Ok(pendingRequests);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching pending leave requests: {ex.Message}");
                return Results.Problem("Failed to fetch pending leave requests.");
            }
        });

        // New route to update the status of a leave request
        app.MapPut("/api/leaverequest/{id}/status", async (HttpContext context, LeaveRequestService leaveRequestService, string id) =>
        {
            // Read the new status from the request body
            var statusUpdate = await context.Request.ReadFromJsonAsync<StatusUpdateRequest>();

            if (statusUpdate == null || string.IsNullOrEmpty(statusUpdate.Status))
            {
                Console.WriteLine("Invalid status update data.");
                return Results.BadRequest("Invalid status update data.");
            }

            // Validate the new status
            if (statusUpdate.Status != "Approved" && statusUpdate.Status != "Rejected")
            {
                Console.WriteLine("Invalid status value. Status must be 'Approved' or 'Rejected'.");
                return Results.BadRequest("Invalid status value. Status must be 'Approved' or 'Rejected'.");
            }

            try
            {
                // Update the leave request status
                var updated = await leaveRequestService.UpdateLeaveRequestStatusAsync(id, statusUpdate.Status);

                if (updated)
                {
                    return Results.Ok(new { message = $"Leave request status updated to {statusUpdate.Status} successfully!" });
                }
                else
                {
                    Console.WriteLine("Leave request not found.");
                    return Results.NotFound("Leave request not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating leave request status: {ex.Message}");
                return Results.Problem("Failed to update leave request status.");
            }
        });
    }
}

// Model for the status update request
public class StatusUpdateRequest
{
    public string Status { get; set; } // New status ("Approved" or "Rejected")
}