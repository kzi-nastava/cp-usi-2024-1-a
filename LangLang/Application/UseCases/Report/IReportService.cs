using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Application.UseCases.Report;

public interface IReportService
{
    public List<List<string>> GetCoursePenaltyReport();

}
