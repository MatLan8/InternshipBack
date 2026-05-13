using MediatR;

namespace InternshipBack.Core.Commands;

public class CreateUserCommand : IRequest<bool>
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
}