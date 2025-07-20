using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TestAPI.Entities;

public partial class Department
{

    [BsonElement("DepartmentId")]
    [Key]
    public int DepartmentId { get; set; }

    [BsonElement("DepartmentName")]
    public string? DepartmentName { get; set; }
}
