using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.Services.UtilityServices;

internal class NavigationService : INavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly ILangLangViewModelFactory _viewModelFactory;

    public NavigationService(NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory)
    {
        _navigationStore = navigationStore;
        _viewModelFactory = viewModelFactory;
    }   

    public void Navigate(ViewType viewType)
    {
        _navigationStore.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
    }
}