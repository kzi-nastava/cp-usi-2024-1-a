using LangLang.ViewModel.Factories;

namespace LangLang.Services.UtilityServices;

public interface INavigationService
{
    public void Navigate(ViewType viewType);
}