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

        public async Task<List<Appointment>> GetAllAppointmentsAsync() =>
            await _appointments.Find(_ => true).ToListAsync();

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

        public async Task<bool> InsertAppointmentAsync(Appointment appointment)
        {
            try
            {
                await _appointments.InsertOneAsync(appointment);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inserting appointment: {ex.Message}");
                return false;
            }
        }
        public async Task<List<Appointment>> GetTodaysAppointmentsAsync()
        {
            var today = DateTime.UtcNow.Date; // Get today's date (UTC)
    
            return await _appointments.Find(a => a.CreatedAt >= today && a.CreatedAt < today.AddDays(1)).ToListAsync();
        }

    }
}
