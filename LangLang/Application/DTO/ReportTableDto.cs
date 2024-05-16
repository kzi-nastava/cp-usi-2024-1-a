using System.Collections.Generic;


namespace LangLang.Application.DTO;

public class ReportTableDto
{
    public List<string> ColumnNames { get; set; }
    public List<List<string>> Rows { get; set; }


    public ReportTableDto(List<string> columnNames, List<List<string>> rows)
    {
        ColumnNames = columnNames;
        Rows = rows;
    }

}
