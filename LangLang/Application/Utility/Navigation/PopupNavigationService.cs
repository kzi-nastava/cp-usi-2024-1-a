using LangLang.Application.Stores;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.Application.Utility.Navigation;

public class PopupNavigationService : NavigationServiceBase, IPopupNavigationService
{
    public PopupNavigationService(NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory)
        : base(navigationStore, viewModelFactory) {}

    public override void Navigate(ViewType viewType)
    {
        NavigationStore.AddPopup(ViewModelFactory.CreateViewModel(viewType));
    }
}