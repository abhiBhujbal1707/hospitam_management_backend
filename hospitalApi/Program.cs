
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

builder.Services.AddSingleton<BillService>(sp =>
    new BillService(
        sp.GetRequiredService<IMongoClient>(),
        configuration
    )
);
builder.Services.AddSingleton<LeaveRequestService>(sp =>
    new LeaveRequestService(
        sp.GetRequiredService<IMongoClient>(),
        configuration
    )
);

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
app.UseBillRoutes();
app.MapLeaveRequestRoutes();
app.MapGet("/", () => "🏥 Hospital API is Running...");

app.Run();
