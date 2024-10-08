using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace LangLang.Application.Utility.PDF;

public class PDFReportService : IPDFReportService
{
    public PdfDocument GetReportPDF(string title, ReportTableDto table)
    {
        PdfDocument pdfDocument = new PdfDocument();

        PdfPage page = pdfDocument.AddPage();

        XGraphics gfx = XGraphics.FromPdfPage(page);

        XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
        XFont paragraphFont = new XFont("Arial", 12);
        XFont tableHeaderFont = new XFont("Arial", 12, XFontStyle.Bold);
        XFont cellFont = new XFont("Arial", 10);
        XRect contentRect = new XRect(40, 40, page.Width - 80, page.Height - 80);

        //title
        XSize titleSize = gfx.MeasureString(title, titleFont);
        gfx.DrawString(title, titleFont, XBrushes.Black, new XPoint(contentRect.Left + (contentRect.Width - titleSize.Width) / 2, contentRect.Top));

        // table
        contentRect = new XRect(contentRect.Left, contentRect.Top + 40, contentRect.Width, contentRect.Height - 40);
        DrawTable(gfx, contentRect, table.ColumnNames, table.Rows, tableHeaderFont, cellFont, pdfDocument);

        return pdfDocument;
    }

    public PdfDocument GetReportPDF(List<string> titles, List<ReportTableDto> tables)
    {
        if (titles.Count != tables.Count)
        {
            throw new ArgumentException("The number of titles must match the number of tables.");
        }

        PdfDocument pdfDocument = new PdfDocument();

        XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
        XFont tableHeaderFont = new XFont("Arial", 12, XFontStyle.Bold);
        XFont cellFont = new XFont("Arial", 10);

        for (int i = 0; i < tables.Count; i++)
        {
            string title = titles[i];
            ReportTableDto table = tables[i];

            PdfPage page = pdfDocument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XRect contentRect = new XRect(40, 40, page.Width - 80, page.Height - 80);

            // Title
            XSize titleSize = gfx.MeasureString(title, titleFont);
            gfx.DrawString(title, titleFont, XBrushes.Black,
                new XPoint(contentRect.Left + (contentRect.Width - titleSize.Width) / 2, contentRect.Top));

            // Table
            contentRect = new XRect(contentRect.Left, contentRect.Top + 40, contentRect.Width, contentRect.Height - 40);
            DrawTable(gfx, contentRect, table.ColumnNames, table.Rows, tableHeaderFont, cellFont, pdfDocument);
        }

        return pdfDocument;
    }

    private void DrawTable(XGraphics gfx, XRect rect, List<string> columnNames, List<List<string>> tableData, XFont headerFont, XFont cellFont, PdfDocument document)
    {
        double columnWidth = rect.Width / columnNames.Count;
        double rowHeight = 20;

        double xPosition = rect.Left;
        double yPosition = rect.Top;
        for (int i = 0; i < columnNames.Count; i++)
        {
            gfx.DrawString(columnNames[i], headerFont, XBrushes.Black, new XRect(xPosition, yPosition, columnWidth, rowHeight), XStringFormats.Center);
            xPosition += columnWidth;
        }

        yPosition += 20;
        foreach (var rowData in tableData)
        {
            // Check if the table exceeds the available space on the current page
            if (yPosition + rowHeight > rect.Bottom)
            {
                // Add a new page
                PdfPage page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                // Reset yPosition
                yPosition = rect.Top;
            }

            xPosition = rect.Left;
            foreach (var cellData in rowData)
            {
                gfx.DrawString(cellData, cellFont, XBrushes.Black, new XRect(xPosition, yPosition, columnWidth, 20), XStringFormats.Center);
                xPosition += columnWidth;
            }
            yPosition += 20;
        }
    }

}