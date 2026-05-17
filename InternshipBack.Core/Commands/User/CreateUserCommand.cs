using MediatR;

namespace InternshipBack.Core.Commands.User;

public class CreateUserCommand : IRequest<bool>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}