// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Builder;
// using HospitalManagement.Models;
// using HospitalManagement.Services;
// using System;
// using System.Threading.Tasks;
// using BCrypt.Net;

// namespace HospitalManagement.Routes
// {
//     public static class PatientRoutes
//     {
//         public static void UsePatientRoutes(this WebApplication app)
//         {
//             app.MapPost("/api/patient/register", async (HttpContext context, PatientService patientService) =>
//             {
//                 var patient = await context.Request.ReadFromJsonAsync<Patient>();

//                 if (patient == null)
//                     return Results.BadRequest("Invalid patient data!");

//                 // Hash the password before saving
//                 patient.PasswordHash = BCrypt.Net.BCrypt.HashPassword(patient.PasswordHash);

//                 // Save patient to MongoDB
//                 await patientService.InsertPatientAsync(patient);

//                 return Results.Ok(new { message = "✅ Patient registered successfully!" });
//             });

//             app.MapGet("/api/patients", async (PatientService patientService) =>
//             {
//                 var patients = await patientService.GetAllPatientsAsync();
//                 return Results.Ok(patients);
//             });

//             app.MapGet("/api/patient/{id}", async (string id, PatientService patientService) =>
//             {
//                 var patient = await patientService.GetPatientByIdAsync(id);
//                 return patient is not null ? Results.Ok(patient) : Results.NotFound();
//             });
//             app.MapDelete("/api/patient/{id}", async (string id, PatientService patientService) =>
//             {
//                 var result = await patientService.DeletePatientByIdAsync(id);
//                 return result ? Results.Ok(new { message = "✅ Patient deleted successfully!" })
//                              : Results.NotFound(new { message = "❌ Patient not found!" });
// });


//         }
//     }
// }
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using HospitalManagement.Models;
using HospitalManagement.Services;
using System;
using System.Threading.Tasks;
using BCrypt.Net;

namespace HospitalManagement.Routes
{
    public static class PatientRoutes
    {
        public static void UsePatientRoutes(this WebApplication app)
        {
            // ✅ Register a Patient
            app.MapPost("/api/patient/register", async (HttpContext context, PatientService patientService) =>
            {
                try
                {
                    var patient = await context.Request.ReadFromJsonAsync<Patient>();
                    if (patient == null)
                        return Results.BadRequest(new { message = "❌ Invalid patient data!" });

                    if (string.IsNullOrEmpty(patient.PasswordHash))
                        return Results.BadRequest(new { message = "❌ Password is required!" });

                    // Hash the password before saving
                    patient.PasswordHash = BCrypt.Net.BCrypt.HashPassword(patient.PasswordHash);

                    // Save patient to MongoDB
                    await patientService.InsertPatientAsync(patient);

                    return Results.Ok(new { message = "✅ Patient registered successfully!" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"❌ Error: {ex.Message}");
                }
            });

            // ✅ Get All Patients
            app.MapGet("/api/patients", async (PatientService patientService) =>
            {
                var patients = await patientService.GetAllPatientsAsync();
                return Results.Ok(patients);
            });

            // ✅ Get a Patient by ID
            app.MapGet("/api/patient/{id}", async (string id, PatientService patientService) =>
            {
                var patient = await patientService.GetPatientByIdAsync(id);
                return patient is not null ? Results.Ok(patient) : Results.NotFound();
            });

            // ✅ Update Patient by ID
            app.MapPut("/api/patient/{id}", async (string id, HttpContext context, PatientService patientService) =>
            {
                try
                {
                    var updatedPatient = await context.Request.ReadFromJsonAsync<Patient>();
                    if (updatedPatient == null)
                        return Results.BadRequest(new { message = "❌ Invalid patient data!" });

                    var result = await patientService.UpdatePatientByIdAsync(id, updatedPatient);
                    return result
                        ? Results.Ok(new { message = "✅ Patient updated successfully!" })
                        : Results.NotFound(new { message = "❌ Patient not found!" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"❌ Error: {ex.Message}");
                }
            });

            // ✅ Delete a Patient by ID
            app.MapDelete("/api/patient/{id}", async (string id, PatientService patientService) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                        return Results.BadRequest(new { message = "❌ Patient ID is required!" });

                    var result = await patientService.DeletePatientByIdAsync(id);
                    return result
                        ? Results.Ok(new { message = "✅ Patient deleted successfully!" })
                        : Results.NotFound(new { message = "❌ Patient not found!" });
                }
                catch (Exception ex)
                {
                    return Results.Problem($"❌ Error: {ex.Message}");
                }
            });
        }
    }
}
