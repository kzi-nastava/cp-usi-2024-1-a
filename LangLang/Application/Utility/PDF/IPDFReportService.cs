using System.Collections.Generic;
using LangLang.Application.DTO;
using PdfSharpCore.Pdf;

namespace LangLang.Application.Utility.PDF;

public interface IPDFReportService
{
    public PdfDocument GetReportPDF(string title, ReportTableDto tableData);
    public PdfDocument GetReportPDF(string title, List<ReportTableDto> tables);
}
