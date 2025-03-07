// // using Microsoft.AspNetCore.Builder;
// // using Microsoft.AspNetCore.Http;
// // using Microsoft.Extensions.DependencyInjection;
// // using Microsoft.Extensions.Hosting;
// // using HospitalManagement.Models;
// // using HospitalManagement.Services;
// // using System;
// // using System.IO;
// // using System.Threading.Tasks;
// // using BCrypt.Net;
// // using HospitalManagement.Routes;
// // using Microsoft.AspNetCore.Authentication.JwtBearer; // For JwtBearerDefaults
// // using Microsoft.IdentityModel.Tokens; // For TokenValidationParameters and SymmetricSecurityKey
// // using System.Text; // For Encoding
// // using Microsoft.Extensions.FileProviders;

// // var builder = WebApplication.CreateBuilder(args);

// // //jwt service
// // builder.Services.AddSingleton<JwtService>(sp => 
// //     new JwtService(
// //         builder.Configuration["Jwt:Key"],
// //         builder.Configuration["Jwt:Issuer"],
// //         builder.Configuration["Jwt:Audience"]
// //     )
// // );

// // // Enable CORS
// // builder.Services.AddCors(options =>
// // {
// //     options.AddPolicy("AllowAll",
// //         policy => policy.AllowAnyOrigin()
// //                         .AllowAnyMethod()
// //                         .AllowAnyHeader());
// // });

// // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// //     .AddJwtBearer(options =>
// //     {
// //         options.TokenValidationParameters = new TokenValidationParameters
// //         {
// //             ValidateIssuerSigningKey = true,
// //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
// //             ValidateIssuer = true,
// //             ValidIssuer = builder.Configuration["Jwt:Issuer"],
// //             ValidateAudience = true,
// //             ValidAudience = builder.Configuration["Jwt:Audience"],
// //             ValidateLifetime = true,
// //             ClockSkew = TimeSpan.Zero
// //         };
// //     });
// //     builder.Services.AddAuthorization(options =>
// // {
// //     options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
// //     options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
// // });

// // // Register MongoDB Services
// // builder.Services.AddSingleton<MongoDbService>();   // For Staff
// // builder.Services.AddSingleton<PatientService>();  // For Patients

// // var app = builder.Build();

// // // Use CORS Middleware
// // app.UseCors("AllowAll");

// // // Ensure 'uploads' folder exists for profile images
// // var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
// // if (!Directory.Exists(uploadFolder))
// // {
// //     Directory.CreateDirectory(uploadFolder);
// //     Console.WriteLine("📂 Created 'uploads' folder for profile images.");
// // }
// // app.UseStaticFiles(new StaticFileOptions
// // {
// //     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
// //     RequestPath = "/uploads"
// // });


// // // ✅ Staff Registration API (Handles File Upload)
// // app.MapPost("/api/staff/register", async (HttpContext context, MongoDbService dbService) =>
// // {
// //     var form = await context.Request.ReadFormAsync();

// //     var staff = new Staff
// //     {
// //         FirstName = form["firstName"],
// //         LastName = form["lastName"],
// //         Gender = form["gender"],
// //         Phone = form["phone"],
// //         Email = form["email"],
// //         Address = form["address"],
// //         PasswordHash = BCrypt.Net.BCrypt.HashPassword(form["passwordHash"]), // Hash Password
// //         Dob = DateTime.Parse(form["dob"]),
// //         JoiningDate = DateTime.Parse(form["joiningDate"]),
// //         Shift = form["shift"],
// //         Salary = decimal.Parse(form["salary"]),
// //         Status = form["status"],
// //         EmergencyContact = form["emergencyContact"],
// //         Role = form["role"],
// //         Specialization = form["specialization"],
// //         LicenseNumber = form["licenseNumber"],
// //         Experience = string.IsNullOrEmpty(form["experience"]) ? null : int.Parse(form["experience"])
// //     };

// //     // Handle File Upload
// //     var file = form.Files["profileImage"];
// //     if (file != null && file.Length > 0)
// //     {
// //         var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
// //         var filePath = Path.Combine(uploadFolder, fileName);

// //         using (var stream = new FileStream(filePath, FileMode.Create))
// //         {
// //             await file.CopyToAsync(stream);
// //         }

// //         staff.ProfileImage = $"/uploads/{fileName}"; // Store file path
// //     }

// //     // Save to MongoDB
// //     await dbService.InsertStaffAsync(staff);

// //     return Results.Ok(new { message = "✅ Staff registered successfully!", profileImage = staff.ProfileImage });
// // });

// // // ✅ Patient Registration API
// // // app.MapPost("/api/patient/register", async (HttpContext context, PatientService patientService) =>
// // // {
// // //     var patient = await context.Request.ReadFromJsonAsync<Patient>();

// // //     if (patient == null)
// // //         return Results.BadRequest("Invalid patient data!");

// // //     // Hash the password before saving
// // //     patient.PasswordHash = BCrypt.Net.BCrypt.HashPassword(patient.PasswordHash);

// // //     // Save patient to MongoDB
// // //     await patientService.InsertPatientAsync(patient);

// // //     return Results.Ok(new { message = "✅ Patient registered successfully!" });
// // // });

// // // // ✅ Get all patients
// // // app.MapGet("/api/patients", async (PatientService patientService) =>
// // // {
// // //     var patients = await patientService.GetAllPatientsAsync();
// // //     return Results.Ok(patients);
// // // });

// // // // ✅ Get a single patient by ID
// // // app.MapGet("/api/patient/{id}", async (string id, PatientService patientService) =>
// // // {
// // //     var patient = await patientService.GetPatientByIdAsync(id);
// // //     return patient is not null ? Results.Ok(patient) : Results.NotFound();
// // // });
// // app.UseAuthentication();
// // app.UseAuthorization();
// // app.UsePatientRoutes();//patient routes
// // app.UseAuthRoutes(); 
// // app.MapGet("/", () => "🏥 Hospital API is Running...");
// // app.Run();
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using HospitalManagement.Models;
// using HospitalManagement.Services;
// using System;
// using System.IO;
// using System.Threading.Tasks;
// using BCrypt.Net;
// using HospitalManagement.Routes;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using Microsoft.Extensions.FileProviders;
// using MongoDB.Driver;

// var builder = WebApplication.CreateBuilder(args);

// // JWT service
// builder.Services.AddSingleton<JwtService>(sp => 
//     new JwtService(
//         builder.Configuration["Jwt:Key"],
//         builder.Configuration["Jwt:Issuer"],
//         builder.Configuration["Jwt:Audience"]
//     )
// );

// // Enable CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         policy => policy.AllowAnyOrigin()
//                         .AllowAnyMethod()
//                         .AllowAnyHeader());
// });

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
//             ValidateIssuer = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidateAudience = true,
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             ValidateLifetime = true,
//             ClockSkew = TimeSpan.Zero
//         };
//     });

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
//     options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
// });
// builder.Services.AddSingleton<IMongoClient>(sp =>
//     new MongoClient(builder.Configuration.GetConnectionString("MongoDB"))
// );
// // Register MongoDB Services
// builder.Services.AddSingleton<MongoDbService>();   // For Staff
// builder.Services.AddSingleton<PatientService>();  // For Patients
// builder.Services.AddSingleton<AppointmentService>();

// var app = builder.Build();

// // Use CORS Middleware
// app.UseCors("AllowAll");

// // Ensure 'uploads' folder exists for profile images
// var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
// if (!Directory.Exists(uploadFolder))
// {
//     Directory.CreateDirectory(uploadFolder);
//     Console.WriteLine("📂 Created 'uploads' folder for profile images.");
// }
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
//     RequestPath = "/uploads"
// });

// // ✅ Get all doctors
// app.MapGet("/api/doctors", async (MongoDbService dbService) =>
// {
//     var doctors = await dbService.GetStaffByRoleAsync("Doctor");
//     return Results.Ok(doctors);
// });

// // ✅ Appointment Routes
// app.MapPost("/api/appointments", async (Appointment appointment, AppointmentService service) =>
// {
//     appointment.CreatedAt = DateTime.UtcNow;
//     await service.InsertAppointmentAsync(appointment);
//     return Results.Ok(new { message = "✅ Appointment booked successfully!" });
// });

// app.MapGet("/api/appointments", async (AppointmentService service) =>
// {
//     var appointments = await service.GetAllAppointmentsAsync();
//     return Results.Ok(appointments);
// });

// app.UseAuthentication();
// app.UseAuthorization();
// app.UsePatientRoutes(); // Patient routes
// app.UseAuthRoutes();
// app.MapGet("/", () => "🏥 Hospital API is Running...");
// app.Run();
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HospitalManagement.Models;
using HospitalManagement.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using BCrypt.Net;
using HospitalManagement.Routes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration; // Added for IConfiguration support

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// JWT service
builder.Services.AddSingleton<JwtService>(sp => 
    new JwtService(
        configuration["Jwt:Key"],
        configuration["Jwt:Issuer"],
        configuration["Jwt:Audience"]
    )
);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Register MongoDB Client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = configuration.GetConnectionString("MongoDB");
    Console.WriteLine($"📌 Connecting to MongoDB at: {connectionString}");
    return new MongoClient(connectionString);
});

// Register MongoDB Services
builder.Services.AddSingleton<MongoDbService>();   // For Staff
builder.Services.AddSingleton<PatientService>();  // For Patients
builder.Services.AddSingleton<AppointmentService>(sp =>
    new AppointmentService(
        sp.GetRequiredService<IMongoClient>(),
        configuration  // Inject configuration here
    ));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
    options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
});

var app = builder.Build();

// Use Middleware in correct order
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Ensure 'uploads' folder exists for profile images
var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if (!Directory.Exists(uploadFolder))
{
    Directory.CreateDirectory(uploadFolder);
    Console.WriteLine("📂 Created 'uploads' folder for profile images.");
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});

// ✅ Get all doctors
app.MapGet("/api/doctors", async (MongoDbService dbService) =>
{
    var doctors = await dbService.GetStaffByRoleAsync("Doctor");
    return Results.Ok(doctors);
});






// Apply custom route handlers
app.UsePatientRoutes(); 
app.UseAuthRoutes();
app.UseAppointmentRoutes();
app.MapGet("/", () => "🏥 Hospital API is Running...");

app.Run();
