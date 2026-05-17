using System.ComponentModel.DataAnnotations;
using InternshipBack.Core.Commands.Item;
using InternshipBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InternshipBack.Core.Handlers.Item;


public class DeleteItemCommandHandler(InternshipBackDbContext context) : IRequestHandler<DeleteItemCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        var item = await context.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (item == null)
        {
            throw new KeyNotFoundException("Item not found");
        }

        if (item.IsDeleted)
        {
            throw new ValidationException($"Item {request.Id} is already deleted");
        }
        
        item.IsDeleted = true;
        item.LastModifiedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync(cancellationToken);

        return true;
    } 
}