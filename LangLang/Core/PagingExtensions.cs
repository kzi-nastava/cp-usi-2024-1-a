using System;
using System.Collections.Generic;

namespace LangLang.Core;

public static class PagingExtensions
{
    public static List<T> GetPage<T>(this List<T> original, int pageNumber, int recordsPerPage)
    {
        if (pageNumber <= 0 || recordsPerPage <= 0)
            throw new ArgumentException("Arguments must be positive integers.");
        if (pageNumber * recordsPerPage + recordsPerPage >= original.Count)
            return new List<T>();
        return original.GetRange((pageNumber - 1) * recordsPerPage, recordsPerPage);
    } 
}