using System.ComponentModel.DataAnnotations;
using InternshipBack.Domain.Types;

namespace InternshipBack.Domain.Entities;

public class Item : Entity
{
    public required ItemsEnum ItemType { get; set; }
    [MaxLength(8)]
    public required string Identifier { get; set; }
    [MaxLength(150)]
    public string? Comment { get; set; }
    public DateTime PurchaseDate { get; set; }
    
    public Guid AssignedUserId { get; set; }
    public User AssignedUser { get; set; } = null!;
    
    public bool IsDeleted { get; set; }
}