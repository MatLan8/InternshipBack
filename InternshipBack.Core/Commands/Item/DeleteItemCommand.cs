using MediatR;

namespace InternshipBack.Core.Commands.Item;

public class DeleteItemCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}