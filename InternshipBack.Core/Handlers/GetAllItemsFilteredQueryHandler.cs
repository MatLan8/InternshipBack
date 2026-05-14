using InternshipBack.Core.Queries;
using InternshipBack.Domain.Dtos;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers;

public class GetAllItemsFilteredQueryHandler(InternshipBackDbContext dbContext) : IRequestHandler<GetAllItemsFilteredQuery, List<ItemDto>>
{
    public async Task<List<ItemDto>> Handle(GetAllItemsFilteredQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Items
            .Include(i => i.AssignedUser)
            .Where(i => !i.IsDeleted);
        
        if (request.ItemTypes is not null && request.ItemTypes.Count != 0)
        {
            query = query.Where(i =>
                request.ItemTypes.Contains(i.ItemType));
        }
        
        if (!string.IsNullOrWhiteSpace(request.Comment))
        {
            var commentFilter = request.Comment.ToLower();

            query = query.Where(i =>
                i.Comment != null &&
                i.Comment.ToLower().StartsWith(commentFilter));
        }
        
        if (request.UserIds is not null && request.UserIds.Count != 0)
        {
            query = query.Where(i =>
                request.UserIds.Contains(i.AssignedUserId));
        }
        
        return await query
            .Select(i => new ItemDto
            {
                Id = i.Id,
                Identifier = i.Identifier,
                ItemType = i.ItemType,
                Comment = i.Comment,
                PurchaseDate = i.PurchaseDate,
                UserName = i.AssignedUser.FirstName + " " + i.AssignedUser.LastName,
                UserIdentifier = i.AssignedUser.Identifier,
            })
            .ToListAsync(cancellationToken);
    }
}