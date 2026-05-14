using InternshipBack.Domain.Types;

namespace InternshipBack.Domain.Dtos;

public class ItemDto
{
    public Guid Id { get; set; }
    public required string Identifier { get; set; }
    
    public ItemsEnum ItemType { get; set; }
    public string? Comment { get; set; }
    public DateTime PurchaseDate { get; set; }
    
    public string? UserName { get; set; }
    public string? UserIdentifier  { get; set; }
    
}