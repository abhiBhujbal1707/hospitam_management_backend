// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Http;
// using HospitalManagement.Models;
// using HospitalManagement.Services;
// using System.Threading.Tasks;
// using BCrypt.Net;

// namespace HospitalManagement.Routes
// {
//     public static class AuthRoutes
//     {
//         public static void UseAuthRoutes(this WebApplication app)
//         {
//             app.MapPost("/api/auth/login", async (HttpContext context, MongoDbService dbService, PatientService patientService, JwtService jwtService) =>
//             {
//                 var loginRequest = await context.Request.ReadFromJsonAsync<LoginRequest>();
//                 if (loginRequest == null)
//                     return Results.BadRequest("Invalid login data!");

//                 // Check in Staff collection
//                 var staff = await dbService.GetStaffByPhoneAsync(loginRequest.Phone);
//                 if (staff != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, staff.PasswordHash))
//                 {
//                     var token = jwtService.GenerateToken(staff.Id, "Staff");
//                     return Results.Ok(new { token, role = "Staff" });
//                 }

//                 // Check in Patient collection
//                 var patient = await patientService.GetPatientByPhoneAsync(loginRequest.Phone);
//                 if (patient != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, patient.PasswordHash))
//                 {
//                     var token = jwtService.GenerateToken(patient.Id, "Patient");
//                     return Results.Ok(new { token, role = "Patient" });
//                 }

//                 return Results.Unauthorized();
//             });
//         }
//     }
// }
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using HospitalManagement.Models;
using HospitalManagement.Services;
using System.Threading.Tasks;
using BCrypt.Net;

namespace HospitalManagement.Routes
{
    public static class AuthRoutes
    {
        public static void UseAuthRoutes(this WebApplication app)
        {
            app.MapPost("/api/auth/login", async (HttpContext context, MongoDbService dbService, PatientService patientService, JwtService jwtService) =>
            {
                var loginRequest = await context.Request.ReadFromJsonAsync<LoginRequest>();
                if (loginRequest == null)
                    return Results.BadRequest("Invalid login data!");

                // Check in Staff collection
                var staff = await dbService.GetStaffByPhoneAsync(loginRequest.Phone);
                if (staff != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, staff.PasswordHash))
                {
                    var token = jwtService.GenerateToken(staff.Id, "Staff");
                    return Results.Ok(new 
                    { 
                        token, 
                        role = "Staff", 
                        user = staff // Return all staff data
                    });
                }

                // Check in Patient collection
                var patient = await patientService.GetPatientByPhoneAsync(loginRequest.Phone);
                if (patient != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, patient.PasswordHash))
                {
                    var token = jwtService.GenerateToken(patient.Id, "Patient");
                    return Results.Ok(new 
                    { 
                        token, 
                        role = "Patient", 
                        user = patient // Return all patient data
                    });
                }

                return Results.Unauthorized();
            });
        }
    }
}