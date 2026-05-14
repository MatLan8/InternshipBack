using InternshipBack.Core.Commands;
using InternshipBack.Domain.Entities;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers;


public class CreateItemCommandHandler(InternshipBackDbContext context) : IRequestHandler<CreateItemCommand, bool>
{
    public async Task<bool> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var userExists = await context.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new Exception("User not found");
        
        var item = new Item
        {
            ItemType = request.ItemType,
            Comment = request.Comment,
            PurchaseDate = request.PurchaseDate,
            AssignedUserId = request.UserId,
            IsDeleted = false,
            Identifier = "1"
        };
      
        await context.Items.AddAsync(item, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    } 
}