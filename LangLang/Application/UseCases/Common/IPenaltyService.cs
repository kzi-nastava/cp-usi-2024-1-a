using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Common;

public interface IPenaltyService
{
    public void AddPenaltyPoint(Student student, Person? sender=null);

    public void AddPenaltyPoint(Student student, Domain.Model.Course course, Person? sender = null);
    
    // Called once a month to reset penalty points for each student
    public void RemovePenaltyPoints();
}