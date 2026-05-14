using MediatR;

namespace InternshipBack.Core.Commands;

public class DeleteItemCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}