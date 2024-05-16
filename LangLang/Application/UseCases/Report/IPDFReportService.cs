
using System.Collections.Generic;
using PdfSharpCore.Pdf;


namespace LangLang.Application.UseCases.Report;

public interface IPDFReportService
{
    public PdfDocument GetReportPDF(string title, string introductoryParagraph, List<string> columnNames, List<List<string>> tableData);

    public PdfDocument GetReportPDF(string title, string introductoryParagraph, List<string> firstTableColumnNames, List<List<string>> firstTableData, List<string> secondTableColumnNames, List<List<string>> secondTableData);

}
