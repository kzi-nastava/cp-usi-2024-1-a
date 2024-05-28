using LangLang.Application.DTO;
using LangLang.WPF.ViewModels.Tutor.Exam;

namespace LangLang.Application.UseCases.Exam;

public interface IExamResultsService
{
    public EmailSendingResultDto SendExamResult(ExamViewModel examViewModel);
}

