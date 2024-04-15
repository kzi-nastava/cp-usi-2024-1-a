namespace LangLang.MVVM;

public delegate TViewModel CreateViewModel<out TViewModel>() where TViewModel : ViewModelBase;