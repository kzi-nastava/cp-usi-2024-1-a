using LangLang.Model;

namespace LangLang.Services.UtilityServices;

public interface IGradeService
{
    public bool IsPassingGrade(ExamGrade examGrade);
}