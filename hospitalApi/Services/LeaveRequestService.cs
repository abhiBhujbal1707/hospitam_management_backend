using HospitalManagement.Models;
using MongoDB.Driver;

namespace HospitalManagement.Services
{
    public class LeaveRequestService
    {
        private readonly IMongoCollection<LeaveRequest> _leaveRequests;

        public LeaveRequestService(IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);
            _leaveRequests = database.GetCollection<LeaveRequest>("LeaveRequests");
        }

        // Insert a new leave request
        public async Task InsertLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            await _leaveRequests.InsertOneAsync(leaveRequest);
        }

        // Get all leave requests (optional)
        public async Task<List<LeaveRequest>> GetAllLeaveRequestsAsync()
        {
            return await _leaveRequests.Find(_ => true).ToListAsync();
        }

        // Get all leave requests with status "Pending"
        public async Task<List<LeaveRequest>> GetPendingLeaveRequestsAsync()
        {
            var filter = Builders<LeaveRequest>.Filter.Eq(r => r.Status, "Pending");
            return await _leaveRequests.Find(filter).ToListAsync();
        }

        // Update the status of a leave request
        public async Task<bool> UpdateLeaveRequestStatusAsync(string id, string newStatus)
        {
            var filter = Builders<LeaveRequest>.Filter.Eq(r => r.Id, id);
            var update = Builders<LeaveRequest>.Update.Set(r => r.Status, newStatus);

            var result = await _leaveRequests.UpdateOneAsync(filter, update);

            // Return true if the document was updated
            return result.ModifiedCount > 0;
        }
    }
}