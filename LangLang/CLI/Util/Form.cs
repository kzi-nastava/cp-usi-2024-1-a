using System;
using System.Linq;
using System.Reflection;

namespace LangLang.CLI.Util;

// where T : new() specifies that every T must have a public parameterless constructor
public class Form<T> where T : new()
{
    public T CreateObject()
    {
        T obj = new();
        PopulateProperties(obj);
        return obj;
       
    }

    private void PopulateProperties(object obj)
    {
        var properties = obj.GetType().GetProperties().Where(property => !Attribute.IsDefined(property, typeof(SkipAttribute))).ToList();

        foreach (var property in properties)
        {
            if(Attribute.IsDefined(property, typeof(SkipInFormAttribute)))
            {
                // set default values
                SetDefaultValue(obj, property);
            }
            else
            {
                Type propertyType = property.PropertyType;
                Type[] parsableTypes = new[] { typeof(string), typeof(DateTime), typeof(DateOnly), typeof(TimeOnly) };
                if (property.PropertyType.IsClass && !parsableTypes.Contains(propertyType))
                {
                    var nestedObj = Activator.CreateInstance(propertyType);
                    if (nestedObj == null)
                    {
                        return;
                    }
                    PopulateProperties(nestedObj);
                    property.SetValue(obj, nestedObj);
                }
                else
                {
                    string value = "";
                    while (value == "")
                    {
                        Console.Write($"Enter {property.Name} >>");
                        value = Console.ReadLine() ?? "";
                    }
                    var type = property.PropertyType;
                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = Nullable.GetUnderlyingType(property.PropertyType)!;
                    }
                    object convertedValue = ConvertValue(type, value);

                    // sets the property value of a specified object
                    property.SetValue(obj, convertedValue);
                }
            }
        }
    }

    private void SetDefaultValue(object obj, PropertyInfo property)
    {
        if (property.PropertyType == typeof(DateTime))
        {
            property.SetValue(obj, DateTime.Now);
        }
        else if (property.PropertyType == typeof(DateOnly))
        {
            property.SetValue(obj, DateOnly.FromDateTime(DateTime.Now));
        }
        else if (property.PropertyType == typeof(TimeOnly))
        {
            property.SetValue(obj, TimeOnly.FromDateTime(DateTime.Now));
        }
        else if (property.PropertyType == typeof(string))
        {
            property.SetValue(obj, "");
        }
        else if (property.PropertyType.IsEnum)
        {
            property.SetValue(obj, Enum.GetValues(property.PropertyType).GetValue(0));
        }
        else if (property.PropertyType == typeof(int))
        {
            property.SetValue(obj, 0);
        }
        else if (property.PropertyType == typeof(bool))
        {
            property.SetValue(obj, true);
        }
    }

    private object ConvertValue(Type type, string value)
    {
        if (type == typeof(string))
        {
            return value;
        }
        else if (type == typeof(int))
        {
            return int.TryParse(value, out var intValue) ? intValue : 0;
        }
        else if (type == typeof(double))
        {
            return double.TryParse(value, out var doubleValue) ? doubleValue : 0.0;
        }
        else if (type == typeof(DateTime))
        {
            var format = "dd.MM.yyyy. HH:mm";
            return DateTime.TryParseExact(value, format, null, System.Globalization.DateTimeStyles.None, out var dateTimeValue) 
                ? dateTimeValue 
                : DateTime.MinValue;
        }
        else if (type == typeof(DateOnly))
        {
            var dateFormat = "dd.MM.yyyy.";
            return DateOnly.TryParseExact(value, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dateValue) 
                ? dateValue 
                : DateOnly.MinValue;
        }
        else if (type == typeof(TimeOnly))
        {
            var timeFormat = "HH:mm";
            return TimeOnly.TryParseExact(value, timeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var timeValue) 
                ? timeValue 
                : TimeOnly.MinValue;
        }
        else if (type == typeof(bool))
        {
            return bool.TryParse(value, out var boolValue) && boolValue;
        }
        else if (type.IsEnum)
        {
            if (Enum.TryParse(type, value, true, out var enumValue))
            {
                if (enumValue != null)
                {
                    return enumValue;
                }
                return Enum.GetValues(type).GetValue(0)!;
            }
            else
            {
                return Enum.GetValues(type).GetValue(0)!;
            }
        }

        return value;
    }

}
