using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.Services.NavigationServices;

public class ClosePopupNavigationService : NavigationServiceBase, IClosePopupNavigationService
{
    public ClosePopupNavigationService(NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory)
        : base(navigationStore, viewModelFactory) {}

    public override void Navigate(ViewType viewType)
    {
        NavigationStore.ClosePopup();
    }
}