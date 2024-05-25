using LangLang.Application.DTO;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.TutorSelection;

public interface IAutoCourseTutorSelector
{
    public Tutor? Select(CourseTutorSelectionDto dto);
}