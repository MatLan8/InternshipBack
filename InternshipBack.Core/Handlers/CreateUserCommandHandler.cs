using MediatR;
using InternshipBack.Core.Commands;
using InternshipBack.Domain.Entities;
using InternshipBack.Infrastructure;

namespace InternshipBack.Core.Handlers;

public class CreateUserCommandHandler(InternshipBackDbContext context) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = request.Name,
            Surname = request.Surname,
            Identifier = "1"
        };
      
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    } 
}