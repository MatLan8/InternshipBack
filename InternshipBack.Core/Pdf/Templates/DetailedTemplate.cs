using InternshipBack.Domain.Dtos;
using QuestPDF.Fluent;

namespace InternshipBack.Core.Pdf.Templates;

public class DetailedTemplate
{
    public static byte[] Generate(List<ItemDto> items, ItemFilterDto filters, List<UserDto> selectedUsers)
    {
        var userGroups = items
            .GroupBy(i => new { i.UserName, i.UserIdentifier })
            .OrderBy(g => g.Key.UserName)
            .ToList();

        var hasFilters =
            (filters.ItemTypes is { Count: > 0 }) ||
            !string.IsNullOrWhiteSpace(filters.Comment) ||
            (filters.UserIds is { Count: > 0 });

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(24);

                // ── Page Header ───────────────────────────────────────────
                page.Header().Column(col =>
                {
                    col.Item()
                        .Text("Inventory Asset Report")
                        .FontSize(22)
                        .Bold();

                    col.Item()
                        .Text($"Generated: {DateTime.Now:yyyy-MM-dd  HH:mm}  ·  {userGroups.Count} users  ·  {items.Count} items")
                        .FontSize(10)
                        .FontColor("#666666");

                    col.Item()
                        .PaddingTop(10)
                        .BorderBottom(1.5f)
                        .BorderColor("#cccccc")
                        .Container();
                });

                page.Content().PaddingTop(20).Column(col =>
                {
                    // ── Filters block ─────────────────────────────────────
                    if (hasFilters)
                    {
                        col.Item()
                            .PaddingBottom(16)
                            .Border(1f)
                            .BorderColor("#e2e8f0")
                            .Background("#f8fafc")
                            .Column(filterCard =>
                            {
                                // Filter card header
                                filterCard.Item()
                                    .BorderBottom(1f)
                                    .BorderColor("#e2e8f0")
                                    .PaddingVertical(10)
                                    .PaddingHorizontal(14)
                                    .Text("Applied Filters")
                                    .FontSize(15)
                                    .Bold();

                                // Filter rows
                                filterCard.Item()
                                    .PaddingVertical(10)
                                    .PaddingHorizontal(14)
                                    .Column(rows =>
                                    {
                                        // Item Types
                                        rows.Item().PaddingBottom(6).Row(row =>
                                        {
                                            row.ConstantItem(100)
                                                .Text("Item Types")
                                                .FontSize(9)
                                                .FontColor("#888888")
                                                .Bold();

                                            if (filters.ItemTypes is { Count: > 0 })
                                                row.RelativeItem()
                                                    .Text(string.Join(", ", filters.ItemTypes))
                                                    .FontSize(9);
                                            else
                                                row.RelativeItem()
                                                    .Text("No filter")
                                                    .FontSize(9)
                                                    .Italic()
                                                    .FontColor("#aaaaaa");
                                        });

                                        // Comment
                                        rows.Item().PaddingBottom(6).Row(row =>
                                        {
                                            row.ConstantItem(100)
                                                .Text("Comment")
                                                .FontSize(9)
                                                .FontColor("#888888")
                                                .Bold();

                                            if (!string.IsNullOrWhiteSpace(filters.Comment))
                                                row.RelativeItem()
                                                    .Text(filters.Comment)
                                                    .FontSize(9);
                                            else
                                                row.RelativeItem()
                                                    .Text("No filter")
                                                    .FontSize(9)
                                                    .Italic()
                                                    .FontColor("#aaaaaa");
                                        });

                                        // Users
                                        rows.Item().Row(row =>
                                        {
                                            row.ConstantItem(100)
                                                .Text("Users")
                                                .FontSize(9)
                                                .FontColor("#888888")
                                                .Bold();

                                            if (selectedUsers is { Count: > 0 })
                                                row.RelativeItem()
                                                    .Text(string.Join(", ", selectedUsers
                                                        .Select(u => $"{u.FirstName} {u.LastName}")))
                                                    .FontSize(9);
                                            else
                                                row.RelativeItem()
                                                    .Text("No filter")
                                                    .FontSize(9)
                                                    .Italic()
                                                    .FontColor("#aaaaaa");
                                        });
                                    });
                            });
                    }

                    // ── Empty state ───────────────────────────────────────
                    if (items.Count == 0)
                    {
                        col.Item()
                            .AlignCenter()
                            .PaddingTop(40)
                            .Column(empty =>
                            {
                                empty.Item()
                                    .AlignCenter()
                                    .Text("No items match the filters")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor("#888888");

                                empty.Item()
                                    .AlignCenter()
                                    .PaddingTop(6)
                                    .Text("Try adjusting or removing some of the applied filters.")
                                    .FontSize(10)
                                    .Italic()
                                    .FontColor("#aaaaaa");
                            });

                        return;
                    }

                    // ── One card per user ─────────────────────────────────
                    foreach (var group in userGroups)
                    {
                        var userName       = string.IsNullOrWhiteSpace(group.Key.UserName)       ? "Unknown User" : group.Key.UserName;
                        var userIdentifier = string.IsNullOrWhiteSpace(group.Key.UserIdentifier) ? "No ID"        : group.Key.UserIdentifier;
                        var userItems      = group.ToList();

                        col.Item().PaddingBottom(16).Border(1.5f).BorderColor("#222222").Column(card =>
                        {
                            card.Item().ShowEntire().Column(anchor =>
                            {
                                anchor.Item()
                                    .Background("#2b2b2b")
                                    .PaddingVertical(12)
                                    .PaddingHorizontal(14)
                                    .Row(hRow =>
                                    {
                                        hRow.RelativeItem().Column(h =>
                                        {
                                            h.Item()
                                                .Text(userName)
                                                .FontSize(17)
                                                .FontColor("#ffffff")
                                                .Bold();

                                            h.Item()
                                                .Text(userIdentifier)
                                                .FontSize(10)
                                                .FontColor("#aaaaaa");
                                        });

                                        hRow.AutoItem()
                                            .AlignMiddle()
                                            .Text($"{userItems.Count} item{(userItems.Count == 1 ? "" : "s")}")
                                            .FontSize(13)
                                            .FontColor("#cccccc")
                                            .Bold();
                                    });

                                anchor.Item()
                                    .PaddingTop(10)
                                    .PaddingHorizontal(14)
                                    .Row(row =>
                                    {
                                        row.ConstantItem(26).Text("#").FontSize(8).FontColor("#888888").Bold();
                                        row.RelativeItem().Text("TYPE").FontSize(8).FontColor("#888888").Bold();
                                        row.RelativeItem().Text("IDENTIFIER").FontSize(8).FontColor("#888888").Bold();
                                        row.ConstantItem(90).Text("PURCHASED").FontSize(8).FontColor("#888888").Bold();
                                        row.RelativeItem(2).Text("COMMENT").FontSize(8).FontColor("#888888").Bold();
                                    });

                                anchor.Item()
                                    .PaddingHorizontal(14)
                                    .PaddingTop(4)
                                    .PaddingBottom(6)
                                    .BorderBottom(1f)
                                    .BorderColor("#dddddd")
                                    .Container();

                                if (userItems.Count > 0)
                                {
                                    anchor.Item()
                                        .Background("#ffffff")
                                        .PaddingVertical(5)
                                        .PaddingHorizontal(14)
                                        .Row(row => RenderItemRow(row, userItems[0], 1));
                                }
                            });

                            for (int i = 1; i < userItems.Count; i++)
                            {
                                var rowBackground = i % 2 == 0 ? "#ffffff" : "#f7f7f7";

                                card.Item()
                                    .PaddingHorizontal(14)
                                    .BorderBottom(0.5f)
                                    .BorderColor("#eeeeee")
                                    .Container();

                                card.Item()
                                    .ShowEntire()
                                    .Background(rowBackground)
                                    .PaddingVertical(5)
                                    .PaddingHorizontal(14)
                                    .Row(row => RenderItemRow(row, userItems[i], i + 1));
                            }

                            card.Item().Height(10);
                        });
                    }
                });

                // ── Footer ────────────────────────────────────────────────
                page.Footer()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span("Page ").FontSize(9).FontColor("#888888");
                        text.CurrentPageNumber().FontSize(9).FontColor("#888888");
                        text.Span(" of ").FontSize(9).FontColor("#888888");
                        text.TotalPages().FontSize(9).FontColor("#888888");
                    });
            });
        });

        return document.GeneratePdf();
    }

    static void RenderItemRow(RowDescriptor row, ItemDto item, int index)
    {
        row.ConstantItem(26)
            .Text(index.ToString())
            .FontSize(9)
            .FontColor("#888888");

        row.RelativeItem()
            .Text(item.ItemType.ToString())
            .FontSize(9)
            .Bold();

        row.RelativeItem()
            .Text(item.Identifier)
            .FontSize(9);

        row.ConstantItem(90).Column(dateCol =>
        {
            dateCol.Item()
                .Text(item.PurchaseDate.ToString("yyyy-MM-dd"))
                .FontSize(9);

            dateCol.Item()
                .Text(item.PurchaseDate.ToString("HH:mm"))
                .FontSize(8)
                .FontColor("#888888");
        });

        row.RelativeItem(2)
            .Text(string.IsNullOrWhiteSpace(item.Comment) ? "-" : item.Comment)
            .FontSize(9)
            .Italic()
            .FontColor("#444444");
    }
}