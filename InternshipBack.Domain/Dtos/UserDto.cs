namespace InternshipBack.Domain.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Identifier { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}