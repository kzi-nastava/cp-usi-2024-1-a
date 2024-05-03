using LangLang.Model;

namespace LangLang.Services.UtilityServices;

public interface IPenaltyService
{
    public void AddPenaltyPoint(Student student, Person? sender=null);
}