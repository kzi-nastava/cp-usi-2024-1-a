using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.Services.NavigationServices;

public class PopupNavigationService : NavigationServiceBase, IPopupNavigationService
{
    public PopupNavigationService(NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory)
        : base(navigationStore, viewModelFactory) {}

    public override void Navigate(ViewType viewType)
    {
        NavigationStore.AddPopup(ViewModelFactory.CreateViewModel(viewType));
    }
}