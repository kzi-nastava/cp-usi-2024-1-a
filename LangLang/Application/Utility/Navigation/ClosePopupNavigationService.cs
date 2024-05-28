using LangLang.Application.Stores;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.Application.Utility.Navigation;

public class ClosePopupNavigationService : NavigationServiceBase, IClosePopupNavigationService
{
    public ClosePopupNavigationService(NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory)
        : base(navigationStore, viewModelFactory) {}

    public override void Navigate(ViewType viewType)
    {
        NavigationStore.ClosePopup();
    }
}