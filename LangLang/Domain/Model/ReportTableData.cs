using System.Collections.Generic;


namespace LangLang.Domain.Model;

public class ReportTableData
{
    public List<string> ColumnNames { get; set; }
    public List<List<string>> Rows { get; set; }


    public ReportTableData(List<string> columnNames, List<List<string>> rows)
    {
        ColumnNames = columnNames;
        Rows = rows;
    }

}
