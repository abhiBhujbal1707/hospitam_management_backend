using HospitalManagement.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagement.Services
{
    public class AppointmentService
    {
        private readonly IMongoCollection<Appointment> _appointments;

        public AppointmentService(IMongoDatabase database)
        {
            _appointments = database.GetCollection<Appointment>("appointments");
        }

        public async Task InsertAppointmentAsync(Appointment appointment)
        {
            await _appointments.InsertOneAsync(appointment);
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointments.Find(_ => true).ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(string id)
        {
            return await _appointments.Find(a => a.Id == id).FirstOrDefaultAsync();
        }
    }
}