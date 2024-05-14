using LangLang.Application.Stores;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.Application.Utility.Navigation;

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