using InternshipBack.Domain.Types;

namespace InternshipBack.Domain.Dtos;

public class ItemFilterDto
{
    public List<ItemsEnum>? ItemTypes { get; set; }
    public string? Comment { get; set; }
    public List<Guid>? UserIds { get; set; }
}