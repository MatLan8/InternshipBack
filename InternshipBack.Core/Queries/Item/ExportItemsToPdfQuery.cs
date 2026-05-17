using InternshipBack.Domain.Types;
using MediatR;

namespace InternshipBack.Core.Queries.Item;

public class ExportItemsToPdfQuery : IRequest<byte[]>
{
    public List<ItemsEnum>? ItemTypes { get; set; }
    public string? Comment { get; set; }
    public List<Guid>? UserIds { get; set; }
    public required ExportTemplateType TemplateType { get; set; }
}