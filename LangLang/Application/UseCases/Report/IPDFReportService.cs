
using System.Collections.Generic;
using LangLang.Domain.Model;
using PdfSharpCore.Pdf;


namespace LangLang.Application.UseCases.Report;

public interface IPDFReportService
{
    public PdfDocument GetReportPDF(string title, string introductoryParagraph, ReportTableData tableData);

    public PdfDocument GetReportPDF(string title, string introductoryParagraph, List<ReportTableData> tables);

}
