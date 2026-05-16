using InternshipBack.Domain.Dtos;
using InternshipBack.Domain.Entities;

namespace InternshipBack.Core.Services;

public static class ItemQueryFilter
{
    public static IQueryable<Item> ApplyFilters(
        this IQueryable<Item> query,
        ItemFilterDto filters)
    {
        query = query.Where(i => !i.IsDeleted);

        if (filters.ItemTypes is not null && filters.ItemTypes.Count != 0)
        {
            query = query.Where(i => filters.ItemTypes.Contains(i.ItemType));
        }

        if (!string.IsNullOrWhiteSpace(filters.Comment))
        {
            var commentFilter = filters.Comment.ToLower();

            query = query.Where(i =>
                i.Comment != null &&
                i.Comment.ToLower().Contains(commentFilter));
        }

        if (filters.UserIds is not null && filters.UserIds.Count != 0)
        {
            query = query.Where(i =>
                filters.UserIds.Contains(i.AssignedUserId));
        }

        return query;
    }
}