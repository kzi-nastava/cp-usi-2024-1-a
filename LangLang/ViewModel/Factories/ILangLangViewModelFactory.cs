using LangLang.MVVM;

namespace LangLang.ViewModel.Factories;

public interface ILangLangViewModelFactory
{
    public ViewModelBase CreateViewModel(ViewType viewType);
}