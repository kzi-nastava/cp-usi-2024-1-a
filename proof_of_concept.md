### Proof of Concept for Handling Non-Primitive Data Types in Generic CRUD Applications

This proof of concept (PoC) outlines how to extend a generic CRUD console application using C# to handle non-primitive data types and display data in table format. We leverage reflection for dynamic type management and discuss handling nested and collection types.

---

### Overview

Our application currently uses a generic class `Form<T>` for primitive and date-related types, dynamically setting property values through reflection. We aim to expand this to manage non-primitive types, such as nested objects and collections, and display them in a tabular format.

### Approach

#### 1. **Instantiation of Non-Primitive Types**

For non-primitive types, reflection creates instances dynamically:

```csharp
if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
{
    var nestedObj = Activator.CreateInstance(property.PropertyType);
    PopulateProperties(nestedObj);
    property.SetValue(obj, nestedObj);
}
```

#### 1. **Recursive Property Population**

Nested objects are handled by recursively populating their properties:


```csharp
private void PopulateProperties(object obj)
{
    foreach (var property in obj.GetType().GetProperties())
    {
        if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
        {
            var nestedObj = Activator.CreateInstance(property.PropertyType);
            PopulateProperties(nestedObj);
            property.SetValue(obj, nestedObj);
        }
        else
        {
            string value = Console.ReadLine() ?? "";
            object convertedValue = ConvertValue(property.PropertyType, value);
            property.SetValue(obj, convertedValue);
        }
    }
}

```

#### 3. **Handling Collections**

For collections, determine the element type and handle dynamic addition:

```charp
if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
{
    var itemType = property.PropertyType.GetGenericArguments()[0];
    var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType)) as IList;
    // Logic to add items to the list
}

```

#### 4. **Displaying Data in Tables**

We use a generic TableAdapter<T> class to represent data in a tabular format, handling both primitive and non-primitive types. Here's a brief overview:

```csharp
public class TableAdapter<T> : ITableAdapter
{
    private readonly List<string> _columnNames;
    private List<T> _items;

    public TableAdapter(List<T> items)
    {
        _columnNames = typeof(T).GetProperties().Select(prop => prop.Name).ToList();
        _items = items;
    }

    public int GetRowCount() => _items.Count;
    public int GetColumnCount() => _columnNames.Count;
    public string GetColumnName(int column) => _columnNames[column];

    public string GetValueAt(int row, int column)
    {
        var item = _items[row];
        var propertyInfo = typeof(T).GetProperties()[column];
        var value = propertyInfo.GetValue(item);

        if (value == null) return "N/A";
        if (propertyInfo.PropertyType == typeof(DateTime)) return ((DateTime)value).ToString("dd.MM.yyyy.");
        if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string)) return NestedObjectToString(value);

        return value.ToString() ?? "N/A";
    }

    private static string NestedObjectToString(object nestedObject)
    {
        return string.Join(", ", nestedObject.GetType().GetProperties().Select(prop => prop.GetValue(nestedObject)?.ToString()));
    }
}
```

Reflection and Dynamic Handling

Reflection facilitates flexible interaction with various data types at runtime. Here's a snippet for converting values dynamically:

```csharp
private object ConvertValue(Type type, string value)
{
    if (type == typeof(int)) return int.Parse(value);
    if (type == typeof(DateTime)) return DateTime.Parse(value);
    if (type.IsEnum) return Enum.Parse(type, value);
    return value;
}
```

#### 5. **Conclusion**

This PoC demonstrates handling non-primitive types and displaying them in tables in a generic CRUD application using reflection. The only thing the current implementation lacks is support for collections.