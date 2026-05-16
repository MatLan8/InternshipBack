using InternshipBack.Core.Queries;
using InternshipBack.Core.Services;
using InternshipBack.Domain.Dtos;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers;

public class GetAllItemsFilteredQueryHandler(InternshipBackDbContext dbContext) : IRequestHandler<GetAllItemsFilteredQuery, List<ItemDto>>
{
    public async Task<List<ItemDto>> Handle(GetAllItemsFilteredQuery request, CancellationToken cancellationToken)
    {
        var filters = new ItemFilterDto
        {
            ItemTypes = request.ItemTypes,
            Comment = request.Comment,
            UserIds = request.UserIds,
        };
        var query = dbContext.Items
            .Include(i => i.AssignedUser)
            .ApplyFilters(filters);
        
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