using LangLang.Application.UseCases.Course;
using LangLang.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Application.UseCases.Report;

public class ReportService: IReportService
{
    public ReportService()
    {
    }

    public ReportTableData GetCoursePenaltyReport()
    {
        //temporary table data
        List<string> columnNames = new List<string>()
        {
            "name",
            "language",
            "level"
        };
        List<List<string>> list2 = new List<List<string>>();
        for (int i = 0; i < 20; i++)
        {
            list2.Add(new List<string>()
            {
                "marija",
                "parezanin",
                "hey"
            });

        }
        return new ReportTableData(columnNames, list2);
    }


}
