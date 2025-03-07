using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using HospitalManagement.Models;

namespace HospitalManagement.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Staff> _staffCollection;

        public MongoDbService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _staffCollection = database.GetCollection<Staff>(config["MongoDB:StaffCollection"]);
        }

        public async Task InsertStaffAsync(Staff staff) =>
            await _staffCollection.InsertOneAsync(staff);
        public async Task<Staff> GetStaffByPhoneAsync(string phone)
        {
            return await _staffCollection.Find(s => s.Phone == phone).FirstOrDefaultAsync();
        }
        public async Task<List<Staff>> GetStaffByRoleAsync(string role)
        {
            var filter = Builders<Staff>.Filter.Eq(s => s.Role, role);
            return await _staffCollection.Find(filter).ToListAsync();
        }

    }
}
