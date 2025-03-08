
using HospitalManagement.Models;
using HospitalManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace HospitalManagement.Routes
{
    public static class AppointmentRoutes
    {
        public static void UseAppointmentRoutes(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/appointments", async (Appointment appointment, AppointmentService service) =>
            {
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.Status ??= "Pending"; // Ensure status is always set
                var success = await service.InsertAppointmentAsync(appointment);

                if (!success)
                    return Results.BadRequest(new { message = "❌ Failed to create appointment." });

                return Results.Ok(new { message = "✅ Appointment booked successfully!" });
            });

            // Appointment route to get all appointments of all time
            app.MapGet("/api/appointments", async (AppointmentService service) =>
            {
                var appointments = await service.GetAllAppointmentsAsync();
                return Results.Ok(appointments);
            });

            // Get all today's appointments
            app.MapGet("/api/appointments/today", async (AppointmentService service) =>
            {
                var appointments = await service.GetTodaysAppointmentsAsync();
                return Results.Ok(appointments);
            });

            // Update appointment status
            app.MapPut("/api/appointments/{id}/status", async (string id, HttpRequest request, AppointmentService service) =>
            {
                Console.WriteLine($"Received request to update status for appointment ID: {id}");

                var requestBody = await request.ReadFromJsonAsync<UpdateStatusRequest>();
                if (requestBody == null || string.IsNullOrEmpty(requestBody.NewStatus))
                {
                    Console.WriteLine("❌ New status is missing or invalid.");
                    return Results.BadRequest(new { message = "❌ New status is required." });
                }

                Console.WriteLine($"New status: {requestBody.NewStatus}");

                var newStatus = requestBody.NewStatus;

                var validStatuses = new List<string> { "Pending", "Completed", "Canceled" };
                if (!validStatuses.Contains(newStatus))
                {
                    Console.WriteLine($"❌ Invalid status value: {newStatus}");
                    return Results.BadRequest(new { message = "❌ Invalid status value" });
                }

                var success = await service.UpdateAppointmentStatusAsync(id, newStatus);
                if (!success)
                {
                    Console.WriteLine($"❌ Failed to update status for appointment ID: {id}");
                    return Results.NotFound(new { message = "❌ Appointment not found or update failed" });
                }

                Console.WriteLine($"✅ Successfully updated status for appointment ID: {id}");
                return Results.Ok(new { message = "✅ Appointment status updated successfully!" });
            });

            // Delete appointment by email
            app.MapDelete("/api/appointments/email/{email}", async (string email, AppointmentService service) =>
            {
                Console.WriteLine($"🛠️ Received DELETE request for email: {email}");

                var success = await service.DeleteAppointmentByEmailAsync(email);
                if (!success)
                    return Results.NotFound(new { message = "❌ Appointment not found or deletion failed." });

                return Results.Ok(new { message = "✅ Appointment deleted successfully!" });
            });

            // Get today's appointments for a specific doctor
            app.MapGet("/api/appointments/today/{doctorName}", async (string doctorName, AppointmentService service) =>
            {
                var appointments = await service.GetTodaysAppointmentsByDoctorAsync(doctorName);
                return Results.Ok(appointments);
            });

            // Get appointments by patient email
app.MapGet("/api/appointments/patient/{email}", async (string email, AppointmentService service) =>
{
    var appointments = await service.GetAppointmentsByPatientEmailAsync(email);
    if (appointments == null || !appointments.Any())
    {
        return Results.NotFound(new { message = "❌ No appointments found for the given email." });
    }

    return Results.Ok(appointments);
});
        }

        // Define the UpdateStatusRequest class within the same file
        public class UpdateStatusRequest
        {
            public string NewStatus { get; set; }
        }
    }
}