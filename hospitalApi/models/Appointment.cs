using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace HospitalManagement.Models
{
    public class Appointment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("firstName")]
    public string? FirstName { get; set; }

    [BsonElement("lastName")]
    public string? LastName { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("phone")]
    public string? Phone { get; set; }

    [BsonElement("date")]
    public string? Date { get; set; }

    [BsonElement("time")]
    public string? Time { get; set; }

    [BsonElement("address")]
    public string? Address { get; set; }

    [BsonElement("symptoms")]
    public string? Symptoms { get; set; }

    [BsonElement("doctor")]
    public string? Doctor { get; set; }

    [BsonElement("bill")]
    [BsonDefaultValue(null)]
    public decimal? Bill { get; set; } // Bill is nullable, to be updated later

    [BsonElement("createdAt")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Automatically set

    


}

}
