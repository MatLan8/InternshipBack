namespace InternshipBack.Domain.Entities;

public class User : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Identifier  { get; set; }
    
    public List<Item> Items { get; set; } = new List<Item>();
}