using System.Windows;
using LangLang.View.Factories;

namespace LangLang.MVVM;

public abstract class NavigableWindow : Window
{
    private readonly IWindowFactory _windowFactory;
    
    protected NavigableWindow(INavigableDataContext navigableDataContext, IWindowFactory windowFactory)
    {
        _windowFactory = windowFactory;
        navigableDataContext.NavigationStore.CurrentViewModelChanged += OnNavigationChange;
    }

    private void OnNavigationChange()
    {
        ((INavigableDataContext)DataContext).NavigationStore.CurrentViewModelChanged -= OnNavigationChange; 
        var currentViewModel = ((INavigableDataContext)DataContext).NavigationStore.CurrentViewModel;
        
        if(currentViewModel != null)
            _windowFactory.CreateWindow(currentViewModel).Show();
        Close();
    }
}