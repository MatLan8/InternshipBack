using InternshipBack.Core.Commands.User;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers.User;

public class CreateUserCommandHandler(InternshipBackDbContext context) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        if (request.FirstName.Length is < 3 or > 20)
        {
            throw new Exception("First name has to be between 4 and 20 characters long");
        }
        
        if (request.LastName.Length is < 3 or > 25)
        {
            throw new Exception("Last name has to be between 4 and 25 characters long");
        }
        
        var lastIdentifier = await context.Users
            .OrderByDescending(u => u.Identifier)
            .Select(u => u.Identifier)
            .FirstOrDefaultAsync(cancellationToken);
        
        int nextNumber = 1;

        if (!string.IsNullOrWhiteSpace(lastIdentifier))
        {
            var numericPart = lastIdentifier.Replace("USR-", "");

            if (int.TryParse(numericPart, out var parsedNumber))
            {
                nextNumber = parsedNumber + 1;
            }
        }

        var newIdentifier = $"USR-{nextNumber:D4}";
        
        var user = new Domain.Entities.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Identifier = newIdentifier
        };
      
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    } 
}