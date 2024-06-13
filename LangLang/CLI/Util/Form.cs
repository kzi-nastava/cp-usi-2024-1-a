using LangLang.CLI.Views;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            Type propertyType = property.PropertyType;
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                object nestedObj = Activator.CreateInstance(propertyType);
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
                object convertedValue = ConvertValue(property.PropertyType, value);

                // sets the property value of a specified object
                property.SetValue(obj, convertedValue);
            }
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
            return DateTime.TryParse(value, out var dateTimeValue) ? dateTimeValue : DateTime.MinValue;
        }
        else if (type == typeof(bool))
        {
            return bool.TryParse(value, out var boolValue) ? boolValue : false;
        }
        else if (type.IsEnum)
        {
            if (Enum.TryParse(type, value, true, out var enumValue))
            {
                return enumValue;
            }
            else
            {
                throw new ArgumentException($"Invalid value '{value}' for enum type '{type.Name}'");
            }
        }

        return value.ToString() ?? "N/A";
    }

}
