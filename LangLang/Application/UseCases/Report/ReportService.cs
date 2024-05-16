using LangLang.Application.DTO;
using System.Collections.Generic;


namespace LangLang.Application.UseCases.Report;

public class ReportService: IReportService
{
    public ReportService()
    {
    }

    public ReportTableDto GetCoursePenaltyReport()
    {
        //temporary table data
        List<string> columnNames = new List<string>()
        {
            "name",
            "language",
            "level"
        };
        List<List<string>> list2 = new List<List<string>>();
        for (int i = 0; i < 100; i++)
        {
            list2.Add(new List<string>()
            {
                "marija",
                "parezanin",
                "hey"
            });

        }
        return new ReportTableDto(columnNames, list2);
    }


}
