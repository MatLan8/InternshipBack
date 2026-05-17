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
        
        item.IsDeleted = true;
        
        await context.SaveChangesAsync(cancellationToken);

        return true;
    } 
}