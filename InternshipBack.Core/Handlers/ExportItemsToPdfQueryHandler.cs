using InternshipBack.Core.Pdf.Templates;
using InternshipBack.Core.Queries;
using InternshipBack.Core.Services;
using InternshipBack.Domain.Dtos;
using InternshipBack.Domain.Types;
using InternshipBack.Infrastructure;
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers;


public class ExportItemsToPdfQueryHandler(InternshipBackDbContext dbContext) : IRequestHandler<ExportItemsToPdfQuery, byte[]>
{
    public async Task<byte[]> Handle(ExportItemsToPdfQuery request, CancellationToken cancellationToken)
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
        
        var items = await query
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
        
        var selectedUsers = await dbContext.Users
            .Where(u => request.UserIds != null && request.UserIds.Contains(u.Id))
            .Select(u => new UserDto
            {
                Id = u.Id,
                Identifier = u.Identifier,
                FirstName = u.FirstName,
                LastName = u.LastName,
                
            })
            .ToListAsync(cancellationToken);

        return request.TemplateType switch
        {
            ExportTemplateType.Simple => SimpleTemplate.Generate(items, filters, selectedUsers),
            ExportTemplateType.User => UserTemplate.Generate(items, filters, selectedUsers),
            _ => throw new Exception("Invalid template")
        };
    }
}