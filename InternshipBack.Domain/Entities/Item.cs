using InternshipBack.Domain.Types;

namespace InternshipBack.Domain.Entities;

public class Item : Entity
{
    public required ItemsEnum ItemType { get; set; }
    public required string Identifier { get; set; }
    public string? Comment { get; set; }
    public DateTime PurchaseDate { get; set; }
    
    public Guid AssignedUserId { get; set; }
    public User AssignedUser { get; set; } = null!;
    
    public bool IsDeleted { get; set; }
}