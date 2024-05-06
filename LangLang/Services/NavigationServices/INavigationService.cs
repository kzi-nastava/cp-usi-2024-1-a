using LangLang.ViewModel.Factories;

namespace LangLang.Services.NavigationServices;

public interface INavigationService
{
    public void Navigate(ViewType viewType);
}

public interface IPopupNavigationService : INavigationService
{
}

public interface IClosePopupNavigationService : INavigationService
{
}