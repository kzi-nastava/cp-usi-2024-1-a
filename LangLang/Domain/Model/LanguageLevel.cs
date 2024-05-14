namespace LangLang.Domain.Model;

public enum LanguageLevel
{
    A1, A2, B1, B2, C1, C2
}

public static class Extensions
{
    public static string ToStr(this LanguageLevel level)
    {
        return level switch
        {
            LanguageLevel.A1 => "A1",
            LanguageLevel.A2 => "A2",
            LanguageLevel.B1 => "B1",
            LanguageLevel.B2 => "B2",
            LanguageLevel.C1 => "C1",
            LanguageLevel.C2 => "C2",
            _ => "",
        };
    }
}