using Consts;
using LangLang.Model;

namespace LangLang.Services.UtilityServices;

public class GradeService : IGradeService
{
    public bool IsPassingGrade(ExamGrade examGrade)
    {
        if (examGrade.ReadingScore < CeilDivide(Constants.MaxReadingScore, 2))
            return false;
        if (examGrade.WritingScore < CeilDivide(Constants.MaxWritingScore, 2))
            return false;
        if (examGrade.ListeningScore < CeilDivide(Constants.MaxListeningScore, 2))
            return false;
        if (examGrade.SpeakingScore < CeilDivide(Constants.MaxSpeakingScore, 2))
            return false;
        var sum = examGrade.ReadingScore + examGrade.WritingScore + examGrade.ListeningScore + examGrade.SpeakingScore;
        return sum > Constants.MinPassingScore;
    }

    private static int CeilDivide(int a, int b)
    {
        return (a + b - 1) / b;
    }
}