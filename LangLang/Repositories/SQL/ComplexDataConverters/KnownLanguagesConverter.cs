using LangLang.Domain.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LangLang.Repositories.SQL.ComplexDataConverters
{
    public class KnownLanguagesConverter : ValueConverter<List<Tuple<Language, LanguageLevel>>, string>
    {
        public KnownLanguagesConverter() : base(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<List<Tuple<Language, LanguageLevel>>>(v, (JsonSerializerOptions)null))
        {
        }
    }
}
