using System;
using System.Collections.Generic;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.Application.Stores;

public class NavigationStore
{
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }
    
    public ViewType ViewType { get; set; }

    public event Action? CurrentViewModelChanged;
    

    private readonly Stack<ViewModelBase> _popupStack = new();

    public ViewModelBase? CurrentPopupViewModel => _popupStack.Count > 0 ? _popupStack.Peek() : null;

    public void AddPopup(ViewModelBase viewModel)
    {
        _popupStack.Push(viewModel);
        PopupOpened?.Invoke();
    }

    public void ClosePopup()
    {
        _popupStack.Pop();
        PopupClosed?.Invoke();
    }
    
    public event Action? PopupOpened;
    public event Action? PopupClosed;
}