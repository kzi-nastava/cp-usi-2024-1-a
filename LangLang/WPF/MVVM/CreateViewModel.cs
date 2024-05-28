namespace LangLang.WPF.MVVM;

public delegate TViewModel CreateViewModel<out TViewModel>() where TViewModel : ViewModelBase;