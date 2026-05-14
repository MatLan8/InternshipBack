using InternshipBack.Domain.Dtos;
using InternshipBack.Domain.Types;
using MediatR;

namespace InternshipBack.Core.Queries;

public class GetAllItemsFilteredQuery : IRequest<List<ItemDto>>
{
    public List<ItemsEnum>? ItemTypes { get; set; }
    public string? Comment { get; set; }
    public List<Guid>? UserIds { get; set; }
}