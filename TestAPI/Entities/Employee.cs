using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestAPI.Entities;

public partial class Employee
{
    [BsonElement("EmployeeId")]
    [Key]
    public int EmployeeId { get; set; }

    [BsonElement("EmployeeName")]
    public string? EmployeeName { get; set; }

    [BsonElement("Department")]
    public string? Department { get; set; }

    [BsonElement("DateOfJoining")]
    public DateTime? DateOfJoining { get; set; }

    [BsonElement("PhotoFileName")]
    public string? PhotoFileName { get; set; }
}
