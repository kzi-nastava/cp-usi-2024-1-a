using LangLang.Domain.Model;
using System;
using System.Collections.Generic;

namespace LangLang.Application.UseCases.Report;

public interface IReportService
{
    public ReportTableData GetCoursePenaltyReport();

}
