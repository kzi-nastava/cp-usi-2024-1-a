using System.Collections.Generic;
using LangLang.Application.DTO;

namespace LangLang.Application.UseCases.Report;

public interface ILanguageReportService
{
    public List<ReportTableDto> GetReport();
}