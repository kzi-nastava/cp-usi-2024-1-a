using LangLang.Domain.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LangLang.Repositories.SQL.ComplexDataConverters
{
    public class ScheduleConverter : ValueConverter<Dictionary<WorkDay, Tuple<TimeOnly, int>>, string>
    {
        public ScheduleConverter() : base(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<Dictionary<WorkDay, Tuple<TimeOnly, int>>>(v, (JsonSerializerOptions)null))
        {
        }
    }
}
