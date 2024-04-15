using System;
using LangLang.MVVM;
using LangLang.ViewModel.Factories;

namespace LangLang.Stores;

public class NavigationStore
{
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }
    
    public ViewType ViewType { get; set; }

    public event Action? CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}