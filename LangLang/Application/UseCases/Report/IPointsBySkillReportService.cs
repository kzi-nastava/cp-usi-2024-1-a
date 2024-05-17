using System.Collections.Generic;
using LangLang.Application.DTO;

namespace LangLang.Application.UseCases.Report;

public interface IPointsBySkillReportService
{
    public List<ReportTableDto> GetReport();
}