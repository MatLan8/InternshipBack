using InternshipBack.Core.Commands.Item;
using InternshipBack.Domain.Types;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers.Item;

public class CreateItemCommandHandler(InternshipBackDbContext context) : IRequestHandler<CreateItemCommand, bool>
{
    public async Task<bool> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var userExists = await context.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (!userExists)
            throw new Exception("User not found");

        if (request.PurchaseDate > DateTime.Now)
        {
            throw new Exception($"Purchase date cannot be in the future");
        }

        if (!string.IsNullOrWhiteSpace(request.Comment))
        {
            if (request.Comment.Length > 150)
            {
                throw new Exception($"Comment cannot exceed 150 characters");
            }
        }
        
        if (!Enum.IsDefined(typeof(ItemsEnum), request.ItemType))
        {
            throw new Exception("Invalid item type");
        }
        
        var lastIdentifier = await context.Items
            .OrderByDescending(i => i.Identifier)
            .Select(i => i.Identifier)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;

        if (!string.IsNullOrWhiteSpace(lastIdentifier))
        {
            var numericPart = lastIdentifier.Replace("ITM-", "");

            if (int.TryParse(numericPart, out var parsedNumber))
            {
                nextNumber = parsedNumber + 1;
            }
        }

        var newIdentifier = $"ITM-{nextNumber:D4}";
        
        var item = new Domain.Entities.Item
        {
            ItemType = request.ItemType,
            Comment = request.Comment,
            PurchaseDate = request.PurchaseDate,
            AssignedUserId = request.UserId,
            IsDeleted = false,
            Identifier = newIdentifier,
        };
      
        await context.Items.AddAsync(item, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    } 
}