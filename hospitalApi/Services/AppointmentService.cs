
using HospitalManagement.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HospitalManagement.Services
{
    public class AppointmentService
    {
        private readonly IMongoCollection<Appointment> _appointments;

        public AppointmentService(IMongoClient mongoClient, IConfiguration configuration)
        {
            var databaseName = configuration["MongoDB:DatabaseName"];
            var database = mongoClient.GetDatabase(databaseName);
            _appointments = database.GetCollection<Appointment>("appointments");
        }

        // ✅ Get All Appointments
        public async Task<List<Appointment>> GetAllAppointmentsAsync() =>
            await _appointments.Find(_ => true).ToListAsync();

        // ✅ Get Appointment by ID
        public async Task<Appointment?> GetAppointmentByIdAsync(string id)
        {
            try
            {
                var objectId = ObjectId.Parse(id);
                return await _appointments.Find(a => a.Id == objectId.ToString()).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Invalid ObjectId: {ex.Message}");
                return null;
            }
        }

        // ✅ Insert a New Appointment
        public async Task<bool> InsertAppointmentAsync(Appointment appointment)
        {
            try
            {
                appointment.Status ??= "Pending"; // Default status is "Pending" if not provided
                appointment.CreatedAt = DateTime.UtcNow; // Ensure CreatedAt is set

                await _appointments.InsertOneAsync(appointment);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inserting appointment: {ex.Message}");
                return false;
            }
        }

        // ✅ Get Today's Appointments
        // public async Task<List<Appointment>> GetTodaysAppointmentsAsync()
        // {
        //     var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

        //     return await _appointments
        //         .Find(a => a.Date == today)
        //         .Project(a => new Appointment
        //         {
        //             Id = a.Id,
        //             FirstName = a.FirstName,
        //             LastName = a.LastName,
        //             Email = a.Email,
        //             Phone = a.Phone,
        //             Date = a.Date,
        //             Time = a.Time,
        //             Doctor = a.Doctor,
        //             Symptoms = a.Symptoms,
        //             Status = a.Status ?? "Pending" // ✅ Handle missing status gracefully
        //         })
        //         .ToListAsync();
        // }
        public async Task<List<Appointment>> GetTodaysAppointmentsAsync()
{
    var today = DateTime.UtcNow.Date; // Get today's date in UTC
    var todayString = today.ToString("yyyy-MM-dd"); // Convert to string format matching the database

    return await _appointments
        .Find(a => a.Date == todayString) // Compare as strings
        .Project(a => new Appointment
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Email = a.Email,
            Phone = a.Phone,
            Date = a.Date,
            Time = a.Time,
            Doctor = a.Doctor,
            Symptoms = a.Symptoms,
            Status = a.Status ?? "Pending"
        })
        .ToListAsync();
}


        // ✅ Update Appointment Status
        public async Task<bool> UpdateAppointmentStatusAsync(string id, string newStatus)
{
    var validStatuses = new List<string> { "Pending", "Completed", "Canceled" };
    if (!validStatuses.Contains(newStatus))
    {
        Console.WriteLine("❌ Invalid status value.");
        return false;
    }

    var update = Builders<Appointment>.Update.Set(a => a.Status, newStatus);
    var result = await _appointments.UpdateOneAsync(a => a.Id == id, update);
    return result.ModifiedCount > 0;
}

        // ✅ Delete Appointment
        public async Task<bool> DeleteAppointmentByEmailAsync(string email)
        {
            try
            {
                email = email.Trim(); // Remove leading/trailing spaces
                Console.WriteLine($"🔍 Searching for appointment with email: '{email}'");

                var filter = Builders<Appointment>.Filter.Eq("email", email);
                var matchedDocument = await _appointments.Find(filter).FirstOrDefaultAsync();

                if (matchedDocument == null)
                {
                    Console.WriteLine($"❌ No matching appointment found for email: {email}");
                    return false;
                }

                var result = await _appointments.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                {
                    Console.WriteLine($"❌ Appointment with email {email} was not deleted.");
                    return false;
                }

                Console.WriteLine($"✅ Appointment with email {email} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error deleting appointment: {ex.Message}");
                return false;
            }
        }
        
        public async Task<List<Appointment>> GetTodaysAppointmentsByDoctorAsync(string doctorName)
{
    var today = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");

    return await _appointments
        .Find(a => a.Date == today && a.Doctor == doctorName)
        .Project(a => new Appointment
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Email = a.Email,
            Phone = a.Phone,
            Date = a.Date,
            Time = a.Time,
            Doctor = a.Doctor,
            Symptoms = a.Symptoms,
            Status = a.Status ?? "Pending"
        })
        .ToListAsync();
}

// ✅ Get Appointments by Patient Email
public async Task<List<Appointment>> GetAppointmentsByPatientEmailAsync(string email)
{
    try
    {
        email = email.Trim(); // Remove leading/trailing spaces
        Console.WriteLine($"🔍 Searching for appointments with email: '{email}'");

        var filter = Builders<Appointment>.Filter.Eq("email", email);
        var appointments = await _appointments.Find(filter).ToListAsync();

        if (appointments == null || !appointments.Any())
        {
            Console.WriteLine($"❌ No appointments found for email: {email}");
            return new List<Appointment>();
        }

        Console.WriteLine($"✅ Found {appointments.Count} appointments for email: {email}");
        return appointments;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error fetching appointments: {ex.Message}");
        return new List<Appointment>();
    }
}
        






    }
}
