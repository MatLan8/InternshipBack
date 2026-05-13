namespace InternshipBack.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Identifier  { get; set; }
}