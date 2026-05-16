using InternshipBack.Domain.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace InternshipBack.Core.Pdf.Templates;

public class SimpleTemplate
{
    // ── Palette — light, minimal, accent-driven ───────────────────────────
    const string AccentBlue    = "#2563eb";   // left stripe & header text
    const string HeaderBg      = "#eff6ff";   // very light blue tint on col headers
    const string RowEven       = "#ffffff";
    const string RowOdd        = "#f8fafc";
    const string CommentBg     = "#f1f5f9";   // cool slate for comment rows
    const string BorderRow     = "#e2e8f0";   // subtle row dividers
    const string BorderOuter   = "#cbd5e1";   // slightly stronger outer edge
    const string MutedText     = "#94a3b8";
    const string CommentText   = "#475569";
    const string AccentText    = "#1d4ed8";

    public static byte[] Generate(List<ItemDto> items)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(24);

                // ── Header ────────────────────────────────────────────────
                page.Header().Column(col =>
                {
                    col.Item()
                        .Text("Simple Inventory Export")
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

                // ── Table ─────────────────────────────────────────────────
                page.Content().PaddingTop(20)
                    .Border(1f).BorderColor(BorderOuter)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(4);    // accent stripe
                            columns.ConstantColumn(32);   // #
                            columns.RelativeColumn();     // Type
                            columns.RelativeColumn();     // Identifier
                            columns.RelativeColumn();     // User
                            columns.RelativeColumn();     // Purchase Date
                        });

                        // ── Column headers ────────────────────────────────
                        table.Header(header =>
                        {
                            // Accent stripe header cell
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

                        // ── Data rows ─────────────────────────────────────
                        for (int i = 0; i < items.Count; i++)
                        {
                            var item   = items[i];
                            var rowBg  = i % 2 == 0 ? RowEven : RowOdd;
                            var isLast = i == items.Count - 1;

                            // Accent stripe — solid blue for every data row
                            table.Cell()
                                .RowSpan(2)
                                .Background(AccentBlue)
                                .BorderBottom(isLast ? 0 : 1f)
                                .BorderColor("#1d4ed8")
                                .Container();

                            // Row number
                            table.Cell()
                                .RowSpan(2)
                                .Element(c => NumberCell(c, rowBg, isLast))
                                .AlignCenter()
                                .AlignMiddle()
                                .Text((i + 1).ToString())
                                .FontSize(9)
                                .FontColor(MutedText);

                            // Data sub-row (spans remaining 4 cols)
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

                            // Comment sub-row
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

    // ── Style helpers ─────────────────────────────────────────────────────

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