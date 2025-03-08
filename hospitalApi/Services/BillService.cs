// // Services/BillService.cs
// using HospitalManagement.Models;
// using MongoDB.Driver;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace HospitalManagement.Services
// {
//     public class BillService
//     {
//         private readonly IMongoCollection<Bill> _bills;

//         public BillService(IMongoClient client, IConfiguration configuration)
//         {
//             var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
//             _bills = database.GetCollection<Bill>("Bills");
//         }

//         public async Task CreateBillAsync(Bill bill)
//         {
//             await _bills.InsertOneAsync(bill);
//         }

//         public async Task UpdateBillAsync(string id, Bill bill)
//         {
//             await _bills.ReplaceOneAsync(b => b.Id == id, bill);
//         }

//         public async Task<Bill> GetBillByIdAsync(string id)
//         {
//             return await _bills.Find(b => b.Id == id).FirstOrDefaultAsync();
//         }

//         public async Task AddBillItemsAsync(string id, List<BillItem> items)
//         {
//             var filter = Builders<Bill>.Filter.Eq(b => b.Id, id);
//             var update = Builders<Bill>.Update.PushEach(b => b.AdditionalItems, items);
//             await _bills.UpdateOneAsync(filter, update);
//         }

//         public async Task UpdatePaymentStatusAsync(string id, string paymentStatus, string paymentMethod)
//         {
//             var filter = Builders<Bill>.Filter.Eq(b => b.Id, id);
//             var update = Builders<Bill>.Update
//                 .Set(b => b.PaymentStatus, paymentStatus)
//                 .Set(b => b.PaymentMethod, paymentMethod);
//             await _bills.UpdateOneAsync(filter, update);
//         }
//     }
// }

// Services/BillService.cs
using HospitalManagement.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagement.Services
{
    public class BillService
    {
        private readonly IMongoCollection<Bill> _bills;

        public BillService(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
            _bills = database.GetCollection<Bill>("Bills");
        }

        // Helper method to calculate TotalBill
        private decimal CalculateTotalBill(Bill bill)
        {
            decimal total = bill.ConsultancyFee;
            if (bill.AdditionalItems != null)
            {
                total += bill.AdditionalItems.Sum(item => item.ItemPrice);
            }
            return total;
        }

        public async Task CreateBillAsync(Bill bill)
        {
            // Calculate TotalBill when creating a new bill
            bill.TotalBill = CalculateTotalBill(bill);
            await _bills.InsertOneAsync(bill);
        }

        public async Task UpdateBillAsync(string id, Bill bill)
        {
            // Calculate TotalBill when updating a bill
            bill.TotalBill = CalculateTotalBill(bill);
            await _bills.ReplaceOneAsync(b => b.Id == id, bill);
        }

        public async Task<Bill> GetBillByIdAsync(string id)
        {
            return await _bills.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddBillItemsAsync(string id, List<BillItem> items)
        {
            var filter = Builders<Bill>.Filter.Eq(b => b.Id, id);
            var update = Builders<Bill>.Update.PushEach(b => b.AdditionalItems, items);

            // Fetch the bill to calculate the new TotalBill
            var bill = await _bills.Find(filter).FirstOrDefaultAsync();
            if (bill != null)
            {
                bill.AdditionalItems ??= new List<BillItem>();
                bill.AdditionalItems.AddRange(items);
                bill.TotalBill = CalculateTotalBill(bill);

                // Update the bill with the new TotalBill
                await _bills.ReplaceOneAsync(filter, bill);
            }
        }

        public async Task UpdatePaymentStatusAsync(string id, string paymentStatus, string paymentMethod)
        {
            var filter = Builders<Bill>.Filter.Eq(b => b.Id, id);
            var update = Builders<Bill>.Update
                .Set(b => b.PaymentStatus, paymentStatus)
                .Set(b => b.PaymentMethod, paymentMethod);
            await _bills.UpdateOneAsync(filter, update);
        }
    }
}