using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities;

public partial class Department
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } // Maps MongoDB _id

    [BsonElement("DepartmentId")]
    [Key]
    public int DepartmentId { get; set; }

    [BsonElement("DepartmentName")]
    public string? DepartmentName { get; set; }
}
