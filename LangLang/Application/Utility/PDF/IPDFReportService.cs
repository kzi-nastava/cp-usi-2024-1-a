using System.Collections.Generic;
using LangLang.Application.DTO;
using PdfSharpCore.Pdf;

namespace LangLang.Application.Utility.PDF;

public interface IPDFReportService
{
    public PdfDocument GetReportPDF(string title, ReportTableDto table);
    public PdfDocument GetReportPDF(List<string> title, List<ReportTableDto> tables);
}
