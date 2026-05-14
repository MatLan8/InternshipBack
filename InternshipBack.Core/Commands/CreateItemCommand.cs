using InternshipBack.Domain.Types;
using MediatR;

namespace InternshipBack.Core.Commands;

public class CreateItemCommand : IRequest<bool>
{
    public required ItemsEnum ItemType { get; set; }
    public string? Comment { get; set; }
    public required DateTime PurchaseDate { get; set; }
    public required Guid UserId { get; set; }
}