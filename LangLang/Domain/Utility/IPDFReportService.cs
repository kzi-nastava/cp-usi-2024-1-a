
using System.Collections.Generic;
using LangLang.Application.DTO;
using PdfSharpCore.Pdf;


namespace LangLang.Domain.Utility;

public interface IPDFReportService
{
    public PdfDocument GetReportPDF(string title, string introductoryParagraph, ReportTableDto tableData);

    public PdfDocument GetReportPDF(string title, string introductoryParagraph, List<ReportTableDto> tables);

}
