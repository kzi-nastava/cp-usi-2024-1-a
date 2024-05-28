using LangLang.Application.DTO;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.TutorSelection;
public interface IAutoExamTutorSelector
{
    public Tutor? Select(ExamTutorSelectionDto dto);
}

