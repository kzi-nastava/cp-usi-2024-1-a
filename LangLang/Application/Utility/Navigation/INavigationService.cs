using LangLang.WPF.ViewModels.Factories;

namespace LangLang.Application.Utility.Navigation;

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