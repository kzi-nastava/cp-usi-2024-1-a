namespace LangLang.MVVM;

internal delegate TViewModel CreateViewModel<out TViewModel>() where TViewModel : ViewModelBase;