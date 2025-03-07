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
            app.MapPost("/api/appointment/book", async (HttpContext context, AppointmentService appointmentService) =>
            {
                var appointment = await context.Request.ReadFromJsonAsync<Appointment>();

                if (appointment == null)
                    return Results.BadRequest("Invalid appointment data!");

                appointment.Bill = null; // Ensure bill is empty while booking
                appointment.CreatedAt = DateTime.UtcNow;

                await appointmentService.InsertAppointmentAsync(appointment);

                return Results.Ok(new { message = "✅ Appointment booked successfully!" });
            });

            app.MapGet("/api/appointments", async (AppointmentService appointmentService) =>
            {
                var appointments = await appointmentService.GetAllAppointmentsAsync();
                return Results.Ok(appointments);
            });

            app.MapGet("/api/appointment/{id}", async (string id, AppointmentService appointmentService) =>
            {
                var appointment = await appointmentService.GetAppointmentByIdAsync(id);
                return appointment is not null ? Results.Ok(appointment) : Results.NotFound();
            });
        }
    }
}