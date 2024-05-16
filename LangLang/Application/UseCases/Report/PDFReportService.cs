using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.Collections.Generic;
using LangLang.Application.UseCases.Report;

public class PDFReportService: IPDFReportService
{
    public PdfDocument GetReportPDF(string title, string introductoryParagraph, List<string> columnNames, List<List<string>> tableData)
    {
        // Create a new PDF document
        PdfDocument pdfDocument = new PdfDocument();

        // Add a page to the document
        PdfPage page = pdfDocument.AddPage();

        // Create a graphics object for drawing on the page
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Define fonts
        XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
        XFont paragraphFont = new XFont("Arial", 12);
        XFont tableHeaderFont = new XFont("Arial", 12, XFontStyle.Bold);
        XFont cellFont = new XFont("Arial", 10);
        XRect contentRect = new XRect(40, 40, page.Width - 80, page.Height - 80);

        //title
        XSize titleSize = gfx.MeasureString(title, titleFont);
        gfx.DrawString(title, titleFont, XBrushes.Black, new XPoint(contentRect.Left + (contentRect.Width - titleSize.Width) / 2, contentRect.Top));

        //Introductory paragraph
        contentRect = new XRect(contentRect.Left, contentRect.Top + 40, contentRect.Width, contentRect.Height - 40);
        gfx.DrawString(introductoryParagraph, paragraphFont, XBrushes.Black, contentRect, XStringFormats.TopLeft);

        // table
        contentRect = new XRect(contentRect.Left, contentRect.Top + 40, contentRect.Width, contentRect.Height - 40);
        DrawTable(gfx, contentRect, columnNames, tableData, tableHeaderFont, cellFont);

        return pdfDocument;
    }


    public PdfDocument GetReportPDF(string title, string introductoryParagraph, List<string> firstTableColumnNames, List<List<string>> firstTableData, List<string> secondTableColumnNames, List<List<string>> secondTableData)
    {
        return null;
    }

    private void DrawTable(XGraphics gfx, XRect rect, List<string> columnNames, List<List<string>> tableData, XFont headerFont, XFont cellFont)
    {
        // Define column widths
        double columnWidth = rect.Width / columnNames.Count;

        // Draw column headers
        double xPosition = rect.Left;
        double yPosition = rect.Top;
        for (int i = 0; i < columnNames.Count; i++)
        {
            gfx.DrawString(columnNames[i], headerFont, XBrushes.Black, new XRect(xPosition, yPosition, columnWidth, 20), XStringFormats.Center);
            xPosition += columnWidth;
        }

        // Draw table data
        yPosition += 20;
        foreach (var rowData in tableData)
        {
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
