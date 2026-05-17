using InternshipBack.Domain.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InternshipBack.Core.Pdf.Templates;

public class SimpleTemplate
{
    const string AccentBlue  = "#2563eb";
    const string HeaderBg    = "#eff6ff";
    const string RowEven     = "#ffffff";
    const string RowOdd      = "#f8fafc";
    const string CommentBg   = "#f1f5f9";
    const string BorderRow   = "#e2e8f0";
    const string BorderOuter = "#cbd5e1";
    const string MutedText   = "#94a3b8";
    const string CommentText = "#475569";
    const string AccentText  = "#1d4ed8";

    public static byte[] Generate(List<ItemDto> items, ItemFilterDto filters, List<UserDto> selectedUsers)
    {
        var hasFilters =
            (filters.ItemTypes is { Count: > 0 }) ||
            !string.IsNullOrWhiteSpace(filters.Comment) ||
            (filters.UserIds is { Count: > 0 });

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(24);

                // ── Header ────────────────────────────────────────────────
                page.Header().Column(col =>
                {
                    col.Item()
                        .Text("Inventory Snapshot Report")
                        .FontSize(22)
                        .Bold();

                    col.Item()
                        .Text($"Generated: {DateTime.Now:yyyy-MM-dd  HH:mm}  ·  {items.Count} items")
                        .FontSize(10)
                        .FontColor("#666666");

                    col.Item()
                        .PaddingTop(10)
                        .BorderBottom(1.5f)
                        .BorderColor("#cccccc")
                        .Container();
                });

                page.Content().PaddingTop(20).Column(pageCol =>
                {
                    // ── Filters block ─────────────────────────────────────
                    if (hasFilters)
                    {
                        pageCol.Item()
                            .PaddingBottom(16)
                            .Border(1f).BorderColor(BorderOuter)
                            .Column(filterCard =>
                            {
                                filterCard.Item()
                                    .BorderBottom(1.5f).BorderColor(BorderOuter)
                                    .Row(hRow =>
                                    {
                                        hRow.ConstantItem(4)
                                            .Background(AccentBlue)
                                            .Container();

                                        hRow.RelativeItem()
                                            .Background(HeaderBg)
                                            .PaddingVertical(8).PaddingHorizontal(10)
                                            .Text("Applied Filters")
                                            .FontSize(10)
                                            .FontColor(AccentText)
                                            .Bold();
                                    });
                                
                                filterCard.Item()
                                    .PaddingVertical(8).PaddingHorizontal(14)
                                    .Column(rows =>
                                    {
                                        rows.Item().PaddingBottom(5).Row(row =>
                                        {
                                            row.ConstantItem(90)
                                                .Text("Item Types")
                                                .FontSize(8)
                                                .FontColor(MutedText)
                                                .Bold();

                                            if (filters.ItemTypes is { Count: > 0 })
                                                row.RelativeItem()
                                                    .Text(string.Join(", ", filters.ItemTypes))
                                                    .FontSize(8)
                                                    .FontColor(CommentText);
                                            else
                                                row.RelativeItem()
                                                    .Text("No filter")
                                                    .FontSize(8)
                                                    .Italic()
                                                    .FontColor(MutedText);
                                        });
                                        
                                        rows.Item().PaddingBottom(5).Row(row =>
                                        {
                                            row.ConstantItem(90)
                                                .Text("Comment")
                                                .FontSize(8)
                                                .FontColor(MutedText)
                                                .Bold();

                                            if (!string.IsNullOrWhiteSpace(filters.Comment))
                                                row.RelativeItem()
                                                    .Text(filters.Comment)
                                                    .FontSize(8)
                                                    .FontColor(CommentText);
                                            else
                                                row.RelativeItem()
                                                    .Text("No filter")
                                                    .FontSize(8)
                                                    .Italic()
                                                    .FontColor(MutedText);
                                        });
                                        
                                        rows.Item().Row(row =>
                                        {
                                            row.ConstantItem(90)
                                                .Text("Users")
                                                .FontSize(8)
                                                .FontColor(MutedText)
                                                .Bold();

                                            if (selectedUsers is { Count: > 0 })
                                                row.RelativeItem()
                                                    .Text(string.Join(", ", selectedUsers
                                                        .Select(u => $"{u.FirstName} {u.LastName}")))
                                                    .FontSize(8)
                                                    .FontColor(CommentText);
                                            else
                                                row.RelativeItem()
                                                    .Text("No filter")
                                                    .FontSize(8)
                                                    .Italic()
                                                    .FontColor(MutedText);
                                        });
                                    });
                            });
                    }

                    // ── Empty state ───────────────────────────────────────
                    if (items.Count == 0)
                    {
                        pageCol.Item()
                            .Border(1f).BorderColor(BorderOuter)
                            .Column(empty =>
                            {
                                empty.Item()
                                    .Row(hRow =>
                                    {
                                        hRow.ConstantItem(4)
                                            .Background(AccentBlue)
                                            .Container();

                                        hRow.RelativeItem()
                                            .Background(HeaderBg)
                                            .BorderBottom(1.5f).BorderColor(BorderOuter)
                                            .PaddingVertical(8).PaddingHorizontal(10)
                                            .Text("No Results")
                                            .FontSize(10)
                                            .FontColor(AccentText)
                                            .Bold();
                                    });

                                empty.Item()
                                    .PaddingVertical(24)
                                    .AlignCenter()
                                    .Column(msg =>
                                    {
                                        msg.Item()
                                            .AlignCenter()
                                            .Text("No items match the filters")
                                            .FontSize(14)
                                            .Bold()
                                            .FontColor(CommentText);

                                        msg.Item()
                                            .AlignCenter()
                                            .PaddingTop(4)
                                            .Text("Try adjusting or removing some of the applied filters.")
                                            .FontSize(9)
                                            .Italic()
                                            .FontColor(MutedText);
                                    });
                            });

                        return;
                    }

                    // ── Table ─────────────────────────────────────────────
                    pageCol.Item()
                        .Border(1f).BorderColor(BorderOuter)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(4);
                                columns.ConstantColumn(32);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell()
                                    .Background(AccentBlue)
                                    .BorderBottom(1f).BorderColor(BorderOuter)
                                    .Container();

                                string[] labels = { "#", "Type", "Identifier", "User", "Purchase Date" };
                                foreach (var label in labels)
                                {
                                    header.Cell()
                                        .Element(HeaderCell)
                                        .Text(label)
                                        .FontSize(8)
                                        .FontColor(AccentText)
                                        .Bold();
                                }
                            });

                            for (int i = 0; i < items.Count; i++)
                            {
                                var item   = items[i];
                                var rowBg  = i % 2 == 0 ? RowEven : RowOdd;
                                var isLast = i == items.Count - 1;

                                table.Cell()
                                    .RowSpan(2)
                                    .Background(AccentBlue)
                                    .BorderBottom(isLast ? 0 : 1f)
                                    .BorderColor("#1d4ed8")
                                    .Container();

                                table.Cell()
                                    .RowSpan(2)
                                    .Element(c => NumberCell(c, rowBg, isLast))
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text((i + 1).ToString())
                                    .FontSize(9)
                                    .FontColor(MutedText);

                                table.Cell()
                                    .ColumnSpan(4)
                                    .Background(rowBg)
                                    .BorderBottom(0.5f).BorderColor(BorderRow)
                                    .Row(row =>
                                    {
                                        row.RelativeItem()
                                            .Element(DataCell)
                                            .Text(item.ItemType.ToString())
                                            .FontSize(10);

                                        row.RelativeItem()
                                            .Element(DataCell)
                                            .Text(item.Identifier)
                                            .FontSize(10);

                                        row.RelativeItem()
                                            .Element(DataCell)
                                            .Column(col =>
                                            {
                                                col.Item()
                                                    .Text(string.IsNullOrWhiteSpace(item.UserName) ? "-" : item.UserName)
                                                    .FontSize(10);
                                                if (!string.IsNullOrWhiteSpace(item.UserIdentifier))
                                                    col.Item()
                                                        .Text(item.UserIdentifier)
                                                        .FontSize(8)
                                                        .FontColor(MutedText);
                                            });

                                        row.RelativeItem()
                                            .Element(DataCell)
                                            .Column(col =>
                                            {
                                                col.Item()
                                                    .Text(item.PurchaseDate.ToString("yyyy-MM-dd"))
                                                    .FontSize(10);
                                                col.Item()
                                                    .Text(item.PurchaseDate.ToString("HH:mm"))
                                                    .FontSize(8)
                                                    .FontColor(MutedText);
                                            });
                                    });

                                table.Cell()
                                    .ColumnSpan(4)
                                    .Background(CommentBg)
                                    .BorderBottom(isLast ? 0 : 1f)
                                    .BorderColor(BorderRow)
                                    .PaddingVertical(4)
                                    .PaddingHorizontal(10)
                                    .Text(text =>
                                    {
                                        text.Span("Comment: ")
                                            .Bold()
                                            .FontSize(8)
                                            .FontColor(CommentText);
                                        text.Span(string.IsNullOrWhiteSpace(item.Comment)
                                                ? "No comment"
                                                : item.Comment)
                                            .Italic()
                                            .FontSize(8)
                                            .FontColor(CommentText);
                                    });
                            }
                        });
                });

                // ── Footer ────────────────────────────────────────────────
                page.Footer()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.Span("Page ").FontSize(9).FontColor(MutedText);
                        text.CurrentPageNumber().FontSize(9).FontColor(MutedText);
                        text.Span(" of ").FontSize(9).FontColor(MutedText);
                        text.TotalPages().FontSize(9).FontColor(MutedText);
                    });
            });
        });

        return document.GeneratePdf();
    }

    static IContainer HeaderCell(IContainer c) =>
        c.Background(HeaderBg)
         .BorderBottom(1.5f).BorderColor(BorderOuter)
         .PaddingVertical(8).PaddingHorizontal(10)
         .AlignMiddle();

    static IContainer DataCell(IContainer c) =>
        c.BorderRight(0.5f).BorderColor(BorderRow)
         .PaddingVertical(6).PaddingHorizontal(10)
         .AlignMiddle();

    static IContainer NumberCell(IContainer c, string bg, bool isLastRow) =>
        c.Background(bg)
         .BorderRight(0.5f).BorderColor(BorderRow)
         .BorderBottom(isLastRow ? 0 : 0.5f).BorderColor(BorderRow);
}