using System.ComponentModel.DataAnnotations;

namespace InternshipBack.Domain.Entities;

public class User : Entity
{
    [MaxLength(20)]
    public required string FirstName { get; set; }
    [MaxLength(25)]
    public required string LastName { get; set; }
    [MaxLength(8)]
    public required string Identifier  { get; set; }
    
    public List<Item> Items { get; set; } = new List<Item>();
}