using InternshipBack.Domain.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InternshipBack.Core.Pdf.Templates;

public class DetailedTemplate
{
    public static byte[] Generate(List<ItemDto> items)
    {
        var userGroups = items
            .GroupBy(i => new { i.UserName, i.UserIdentifier })
            .OrderBy(g => g.Key.UserName)
            .ToList();

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

                // ── One card per user ─────────────────────────────────────
                page.Content().PaddingTop(20).Column(col =>
                {
                    foreach (var group in userGroups)
                    {
                        var userName       = string.IsNullOrWhiteSpace(group.Key.UserName)       ? "Unknown User" : group.Key.UserName;
                        var userIdentifier = string.IsNullOrWhiteSpace(group.Key.UserIdentifier) ? "No ID"        : group.Key.UserIdentifier;
                        var userItems      = group.ToList();

                        col.Item().PaddingBottom(16).Border(1.5f).BorderColor("#222222").Column(card =>
                        {
                            // ── Anchor block: kept together on same page ──
                            // Ensures the card header + column labels + first item
                            // are never separated by a page break.
                            card.Item().ShowEntire().Column(anchor =>
                            {
                                // Dark header bar with item count on the right
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

                                        // Item count — large and visible in the header
                                        hRow.AutoItem()
                                            .AlignMiddle()
                                            .Text($"{userItems.Count} item{(userItems.Count == 1 ? "" : "s")}")
                                            .FontSize(13)
                                            .FontColor("#cccccc")
                                            .Bold();
                                    });

                                // Column label row
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

                                // Divider under column labels
                                anchor.Item()
                                    .PaddingHorizontal(14)
                                    .PaddingTop(4)
                                    .PaddingBottom(6)
                                    .BorderBottom(1f)
                                    .BorderColor("#dddddd")
                                    .Container();

                                // First item row (anchored with the header)
                                if (userItems.Count > 0)
                                {
                                    anchor.Item()
                                        .Background("#ffffff")
                                        .PaddingVertical(5)
                                        .PaddingHorizontal(14)
                                        .Row(row => RenderItemRow(row, userItems[0], 1));
                                }
                            });

                            // ── Remaining items (can flow across pages) ───
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

                            // Bottom breathing room
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