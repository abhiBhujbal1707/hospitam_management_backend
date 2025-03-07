// using MongoDB.Driver;
// using Microsoft.Extensions.Configuration;
// using HospitalManagement.Models;
// using MongoDB.Bson;
// namespace HospitalManagement.Services
// {
//     public class PatientService
//     {
//         private readonly IMongoCollection<Patient> _patientCollection;

//         public PatientService(IConfiguration config)
//         {
//             var client = new MongoClient(config.GetConnectionString("MongoDB"));
//             var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
//             _patientCollection = database.GetCollection<Patient>(config["MongoDB:PatientCollection"]);
//         }

//         public async Task<List<Patient>> GetAllPatientsAsync() =>
//             await _patientCollection.Find(_ => true).ToListAsync();

//         public async Task<Patient?> GetPatientByIdAsync(string id) =>
//             await _patientCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

//         public async Task InsertPatientAsync(Patient patient) =>
//             await _patientCollection.InsertOneAsync(patient);
//         public async Task<bool> DeletePatientByIdAsync(string id)
//         {
//             // ✅ Convert string id to ObjectId
//             if (!ObjectId.TryParse(id, out ObjectId objectId))
//             {
//                 return false; // ❌ If ID is not valid, return false
//             }

//             var filter = Builders<Patient>.Filter.Eq("_id", objectId); // ✅ Use ObjectId in query
//             var result = await _patientCollection.DeleteOneAsync(filter);

//             return result.DeletedCount > 0;
//         }





//     }
// }
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using HospitalManagement.Models;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagement.Services
{
    public class PatientService
    {
        private readonly IMongoCollection<Patient> _patientCollection;

        public PatientService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            var database = client.GetDatabase(config["MongoDB:DatabaseName"]);
            _patientCollection = database.GetCollection<Patient>(config["MongoDB:PatientCollection"]);
        }

        // ✅ Get all patients
        public async Task<List<Patient>> GetAllPatientsAsync() =>
            await _patientCollection.Find(_ => true).ToListAsync();

        // ✅ Get a single patient by ID
        public async Task<Patient?> GetPatientByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
                return null;

            return await _patientCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // ✅ Insert a new patient
        public async Task InsertPatientAsync(Patient patient) =>
            await _patientCollection.InsertOneAsync(patient);

        // ✅ Update an existing patient
        public async Task<bool> UpdatePatientByIdAsync(string id, Patient updatedPatient)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
                return false;

            var filter = Builders<Patient>.Filter.Eq("_id", objectId);
            var update = Builders<Patient>.Update
                .Set(p => p.FirstName, updatedPatient.FirstName)
                .Set(p => p.LastName, updatedPatient.LastName)
                .Set(p => p.Gender, updatedPatient.Gender)
                .Set(p => p.Phone, updatedPatient.Phone)
                .Set(p => p.Email, updatedPatient.Email)
                .Set(p => p.Address, updatedPatient.Address)
                .Set(p => p.Dob, updatedPatient.Dob)
                .Set(p => p.BloodGroup, updatedPatient.BloodGroup)
                .Set(p => p.EmergencyContact, updatedPatient.EmergencyContact)
                .Set(p => p.MedicalHistory, updatedPatient.MedicalHistory);

            var result = await _patientCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        // ✅ Delete a patient by ID
        public async Task<bool> DeletePatientByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
                return false;

            var filter = Builders<Patient>.Filter.Eq("_id", objectId);
            var result = await _patientCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
        public async Task<Patient> GetPatientByPhoneAsync(string phone)
        {
            return await _patientCollection.Find(p => p.Phone == phone).FirstOrDefaultAsync();
        }

    }
}
