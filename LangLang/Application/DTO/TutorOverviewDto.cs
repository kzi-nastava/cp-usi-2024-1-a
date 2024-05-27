using LangLang.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LangLang.Application.DTO;

public class TutorOverviewDto
{
    public string Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public string KnownLanguages { get; }
    public DateTime DateAdded { get; }

    public TutorOverviewDto(Tutor tutor)
    {
        Id = tutor.Id; 
        Name = tutor.Name;
        Surname = tutor.Surname;
        KnownLanguages = ToString(tutor.KnownLanguages);
        DateAdded = tutor.DateAdded;
    }

    private static string ToString(List<Tuple<Language, LanguageLevel>> knownLanguages)
    {
        StringBuilder builder = new();
        foreach (var tuple in knownLanguages)
            builder.AppendLine($"{tuple.Item1.Name} - {tuple.Item2.ToStr()}");
        return builder.ToString().TrimEnd();
    }
}
