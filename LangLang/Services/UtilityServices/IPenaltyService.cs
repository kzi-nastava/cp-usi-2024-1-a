using LangLang.Model;

namespace LangLang.Services.UtilityServices;

public interface IPenaltyService
{
    public void AddPenaltyPoint(Student student, Person? sender=null);
    
    // Called once a month to reset penalty points for each student
    public void RemovePenaltyPoints();
}