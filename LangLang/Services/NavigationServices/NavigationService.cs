using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.Services.NavigationServices;

internal class NavigationService : NavigationServiceBase
{ 
    public NavigationService(NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory)
        : base(navigationStore, viewModelFactory) {}

    public override void Navigate(ViewType viewType)
    {
        NavigationStore.CurrentViewModel = ViewModelFactory.CreateViewModel(viewType);
        NavigationStore.ViewType = viewType;
    }
}