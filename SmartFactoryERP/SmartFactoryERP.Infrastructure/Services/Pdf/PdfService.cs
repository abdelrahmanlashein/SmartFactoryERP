using SmartFactoryERP.Domain.Entities.Sales;
using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SmartFactoryERP.Infrastructure.Services.Pdf
{
    public class PdfService
    {
        public byte[] GenerateInvoicePdf(Invoice invoice)
        {
            // 1. Explicitly reference the class to avoid name collisions
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);

                    // --- Header ---
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text($"Invoice #{invoice.InvoiceNumber}").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text($"Date: {invoice.InvoiceDate:yyyy-MM-dd}");
                            col.Item().Text($"Due Date: {invoice.DueDate:yyyy-MM-dd}");
                        });

                        row.ConstantItem(100).Height(50).Placeholder();
                    });

                    // --- Content ---
                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Item().Text("Bill To:").Bold();
                        col.Item().Text(invoice.SalesOrder?.Customer?.CustomerName ?? "Unknown Customer");
                        col.Item().Text(invoice.SalesOrder?.Customer?.Address ?? "No Address");
                        col.Item().PaddingBottom(20);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(25);
                                columns.RelativeColumn();
                                columns.ConstantColumn(50);
                                columns.ConstantColumn(75);
                                columns.ConstantColumn(75);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#");
                                header.Cell().Element(CellStyle).Text("Item");
                                header.Cell().Element(CellStyle).Text("Qty");
                                header.Cell().Element(CellStyle).Text("Unit Price");
                                header.Cell().Element(CellStyle).Text("Total");
                            });

                            if (invoice.SalesOrder?.Items != null)
                            {
                                int i = 1;
                                foreach (var item in invoice.SalesOrder.Items)
                                {
                                    table.Cell().Element(CellStyle).Text(i++);
                                    table.Cell().Element(CellStyle).Text($"Material ID: {item.MaterialId}");
                                    table.Cell().Element(CellStyle).Text(item.Quantity);
                                    table.Cell().Element(CellStyle).Text($"{item.UnitPrice:C}");
                                    table.Cell().Element(CellStyle).Text($"{item.TotalPrice:C}");
                                }
                            }
                        });

                        col.Item().PaddingTop(10).AlignRight().Text($"Grand Total: {invoice.TotalAmount:C}").FontSize(16).Bold().FontColor(Colors.Green.Medium);
                    });

                    // --- Footer ---
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }

        // 2. Move the cell styling into a separate clean helper method
        private static IContainer CellStyle(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
        }
    }
}