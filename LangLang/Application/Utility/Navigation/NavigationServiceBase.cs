using LangLang.Application.Stores;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.Application.Utility.Navigation;

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