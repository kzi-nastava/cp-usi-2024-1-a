using System;
using System.Collections.Generic;
using LangLang.CLI.Views;
using LangLang.Core;

namespace LangLang.CLI.Util;

public class Table<T> : ICliMenu
{
    private const int ColumnWidth = 11;
    
    private readonly TableAdapter<T> _tableAdapter;
    private readonly bool _showHeader;
    
    public Table(TableAdapter<T> tableAdapter, bool showHeader = true)
    {
        _tableAdapter = tableAdapter;
        _showHeader = showHeader;
    }

    public void Show()
    {
        if(_showHeader)
            PrintHeader();
        PrintRows();
    }

    private void PrintHeader()
    {
        PrintRowSeparator();
        for (var col = 0; col < _tableAdapter.GetColumnCount(); col++)
        {
            Console.Write("|");
            Console.Write(_tableAdapter.GetColumnName(col).Truncate(ColumnWidth).PadRight(ColumnWidth));
        }
        Console.WriteLine("|");
        PrintRowSeparator();
    }

    private void PrintRows()
    {
        for (var row = 0; row < _tableAdapter.GetRowCount(); row++)
        {
            for (var col = 0; col < _tableAdapter.GetColumnCount(); col++)
            {
                Console.Write("|");
                Console.Write(_tableAdapter.GetValueAt(row, col).Truncate(ColumnWidth).PadRight(ColumnWidth));
            }
            Console.WriteLine("|");
            PrintRowSeparator();
        }
    }
    
    private void PrintRowSeparator()
    {
        for (var col = 0; col < _tableAdapter.GetColumnCount(); col++)
        {
            Console.Write("+");
            for (var i = 0; i < ColumnWidth; i++)
            {
                Console.Write("-");
            }
        }
        Console.WriteLine("+");
    }
    
    public T GetItem(int row) => _tableAdapter.GetItem(row);

    public void AddItem(T item) => _tableAdapter.AddItem(item);
    
    public void RemoveItem(T item) => _tableAdapter.RemoveItem(item);
    
    public void UpdateItem(T oldItem, T newItem) => _tableAdapter.UpdateItem(oldItem, newItem);
    
    public void ClearItems() => _tableAdapter.ClearItems();
    
    public void SetItems(List<T> items) => _tableAdapter.SetItems(items);
}