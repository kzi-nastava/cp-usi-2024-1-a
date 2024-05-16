using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace LangLang.Application.UseCases.Report.CoursePensaltyReport;

public class PDFReport
{
    public PdfDocument GetPenaltyPointReportPDF(Dictionary<Domain.Model.Course, uint> penaltyPointsByCourse)
    {
        PdfDocument document = new PdfDocument();

        // Add a page to the document
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);

        XFont titleFont = new XFont("Arial", 24, XFontStyle.Bold);
        XFont regularFont = new XFont("Arial", 12, XFontStyle.Regular);

        // Title
        gfx.DrawString("Report about penalty points", titleFont, XBrushes.Black, new XRect(0, 40, page.Width, page.Height), XStringFormats.Center);

        // Paragraph
        XTextFormatter tf = new XTextFormatter(gfx);
        XRect rect = new XRect(40, 100, page.Width - 80, page.Height);
        tf.DrawString("This report provides details about penalty points for different courses.", regularFont, XBrushes.Black, rect);

        // Table
        const int columnWidth = 150;
        const int rowHeight = 20;
        int startX = 40;
        int startY = 160;

        DrawTableHeader(gfx, startX, startY, columnWidth, rowHeight);
        DrawTableData(gfx, startX, startY + rowHeight, columnWidth, rowHeight, penaltyPointsByCourse);

        return document;
    }

    private void DrawTableHeader(XGraphics gfx, int startX, int startY, int columnWidth, int rowHeight)
    {
        // Column names
        string[] columnNames = { "Course name", "Language", "Level", "Penalty points" };

        XFont headerFont = new XFont("Arial", 14, XFontStyle.Bold);

        // Draw header cells
        for (int i = 0; i < columnNames.Length; i++)
        {
            gfx.DrawRectangle(XBrushes.LightGray, startX + i * columnWidth, startY, columnWidth, rowHeight);
            gfx.DrawString(columnNames[i], headerFont, XBrushes.Black, new XRect(startX + i * columnWidth, startY, columnWidth, rowHeight), XStringFormats.Center);
        }
    }

    private void DrawTableData(XGraphics gfx, int startX, int startY, int columnWidth, int rowHeight, Dictionary<Domain.Model.Course, uint> penaltyPointsByCourse)
    {
        XFont regularFont = new XFont("Arial", 12, XFontStyle.Regular);

        // Draw data cells
        int row = 0;
        foreach (var kvp in penaltyPointsByCourse)
        {
            var course = kvp.Key;
            uint penaltyPoints = kvp.Value;

            gfx.DrawRectangle(XBrushes.White, startX, startY + row * rowHeight, columnWidth, rowHeight);
            gfx.DrawString(course.Name, regularFont, XBrushes.Black, new XRect(startX, startY + row * rowHeight, columnWidth, rowHeight), XStringFormats.CenterLeft);

            gfx.DrawRectangle(XBrushes.White, startX + columnWidth, startY + row * rowHeight, columnWidth, rowHeight);
            gfx.DrawString(course.Language.ToString(), regularFont, XBrushes.Black, new XRect(startX + columnWidth, startY + row * rowHeight, columnWidth, rowHeight), XStringFormats.CenterLeft);

            gfx.DrawRectangle(XBrushes.White, startX + 2 * columnWidth, startY + row * rowHeight, columnWidth, rowHeight);
            gfx.DrawString(course.Level.ToString(), regularFont, XBrushes.Black, new XRect(startX + 2 * columnWidth, startY + row * rowHeight, columnWidth, rowHeight), XStringFormats.Center);

            gfx.DrawRectangle(XBrushes.White, startX + 3 * columnWidth, startY + row * rowHeight, columnWidth, rowHeight);
            gfx.DrawString(penaltyPoints.ToString(), regularFont, XBrushes.Black, new XRect(startX + 3 * columnWidth, startY + row * rowHeight, columnWidth, rowHeight), XStringFormats.Center);

            row++;
        }
    }

}
