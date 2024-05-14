using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Factories;

public interface ILangLangViewModelFactory
{
    public ViewModelBase CreateViewModel(ViewType viewType);
}