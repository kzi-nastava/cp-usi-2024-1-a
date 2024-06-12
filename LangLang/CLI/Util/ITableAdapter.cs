namespace LangLang.CLI.Util;

public interface ITableAdapter
{
    public int GetRowCount();
    public int GetColumnCount();
    public string GetColumnName(int column);
    public string GetValueAt(int row, int column);
}