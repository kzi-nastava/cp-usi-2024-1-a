using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.Services.NavigationServices;

public abstract class NavigationServiceBase : INavigationService
{
    protected readonly NavigationStore NavigationStore;
    protected readonly ILangLangViewModelFactory ViewModelFactory;

    protected NavigationServiceBase(NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory)
    {
        NavigationStore = navigationStore;
        ViewModelFactory = viewModelFactory;
    }

    public abstract void Navigate(ViewType viewType);
}