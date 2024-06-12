using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.CLI.Util;

public class TableAdapter<T> : ITableAdapter
{
    private readonly List<string> _columnNames;
    private List<T> _items;

    public TableAdapter(List<T> items)
    {
        _columnNames = typeof(T).GetProperties().Select(propInfo => propInfo.Name).ToList();
        _items = items;
    }

    public int GetRowCount()
    {
        return _items.Count;
    }

    public int GetColumnCount()
    {
        return _columnNames.Count;
    }

    public string GetColumnName(int column)
    {
        return _columnNames[column];
    }

    public string GetValueAt(int row, int column)
    {
        var item = _items[row];
        var propertyInfo = typeof(T).GetProperties()[column];
        
        var value = propertyInfo.GetValue(item);
        if (Attribute.IsDefined(propertyInfo, typeof(SkipAttribute)))
        {
            return "Skipped";
        }
        if (value == null)
        {
            return "N/A";
        }
        
        if (propertyInfo.PropertyType == typeof(DateTime))
        {
            return ((DateTime)value).ToString("dd.MM.yyyy.");
        } else if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string))
        {
            return NestedObjectToString(value);
        }
        
        return value.ToString() ?? "N/A";
    }

    public T GetItem(int row)
    {
        return _items[row];
    }

    public void AddItem(T item)
    {
        _items.Add(item);
    }
    
    public void RemoveItem(T item)
    {
        _items.Remove(item);
    }
    
    public void UpdateItem(T oldItem, T newItem)
    {
        var index = _items.IndexOf(oldItem);
        _items[index] = newItem;
    }
    
    public void ClearItems()
    {
        _items.Clear();
    }
    
    public void SetItems(List<T> items)
    {
        _items = items;
    }
    
    private static string NestedObjectToString(object nestedObject)
    {
        var nestedProperties = nestedObject.GetType().GetProperties();
        var res = "";
        foreach (var nestedProp in nestedProperties)
        {
            res += $"{nestedProp.GetValue(nestedObject)}, ";
        }
        return res;
    }
}