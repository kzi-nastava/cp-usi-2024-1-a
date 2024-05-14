using LangLang.Domain.Model;

namespace LangLang.Domain.Utility;

public interface IGradeService
{
    public bool IsPassingGrade(ExamGrade examGrade);
}