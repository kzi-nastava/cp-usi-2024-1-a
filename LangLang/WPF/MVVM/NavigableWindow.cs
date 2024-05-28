using System.ComponentModel;
using System.Windows;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.MVVM;

public abstract class NavigableWindow : Window
{
    private readonly IWindowFactory _windowFactory;
    private readonly INavigableDataContext _navigableDataContext;

    private bool _isUserClosing = true; // indicates whether the window close is triggered on "X" button of via event

    protected NavigableWindow(INavigableDataContext navigableDataContext, IWindowFactory windowFactory)
    {
        _navigableDataContext = navigableDataContext;
        _windowFactory = windowFactory;
        Subscribe();
    }

    private void OnNavigationChange()
    {
        Unsubscribe();
        var currentViewModel = _navigableDataContext.NavigationStore.CurrentViewModel;
        if (currentViewModel != null)
        {
            Window w = _windowFactory.CreateWindow(currentViewModel);
            w.Show();
        }
        _isUserClosing = false;
        Close();
    }

    private void OnPopupOpen()
    {
        Unsubscribe();
        var currentViewModel = _navigableDataContext.NavigationStore.CurrentPopupViewModel;
        if (currentViewModel != null)
            _windowFactory.CreateWindow(currentViewModel).ShowDialog();
        Subscribe();
    }

    private void OnPopupClose()
    {
        Unsubscribe();
        _isUserClosing = false;
        Close();
    }

    private void Subscribe()
    {
        _navigableDataContext.NavigationStore.CurrentViewModelChanged += OnNavigationChange;
        _navigableDataContext.NavigationStore.PopupOpened += OnPopupOpen;
        _navigableDataContext.NavigationStore.PopupClosed += OnPopupClose;
    }

    private void Unsubscribe()
    {
        _navigableDataContext.NavigationStore.CurrentViewModelChanged -= OnNavigationChange;
        _navigableDataContext.NavigationStore.PopupOpened -= OnPopupOpen;
        _navigableDataContext.NavigationStore.PopupClosed -= OnPopupClose;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (_isUserClosing && _navigableDataContext.NavigationStore.CurrentPopupViewModel != null)
        {
            // user closing a popup window on "X" button
            Unsubscribe();
            _isUserClosing = false;
            _navigableDataContext.NavigationStore.ClosePopup();
        }
        base.OnClosing(e);
    }
}