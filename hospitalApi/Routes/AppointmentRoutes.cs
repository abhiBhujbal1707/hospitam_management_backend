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
                var success = await service.InsertAppointmentAsync(appointment);

                if (!success)
                   return Results.BadRequest(new { message = "❌ Failed to create appointment." });

                return Results.Ok(new { message = "✅ Appointment booked successfully!" });
            });
            //appointment route to gett all appointments of all time
            app.MapGet("/api/appointments", async (AppointmentService service) =>
            {
                var appointments = await service.GetAllAppointmentsAsync();
                return Results.Ok(appointments);
            });
            // get all todays appointments
            app.MapGet("/api/appointments/today", async (AppointmentService service) =>
            {
                var appointments = await service.GetTodaysAppointmentsAsync();
                return Results.Ok(appointments);
            });


        }
    }
}