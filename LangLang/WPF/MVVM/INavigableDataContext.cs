using LangLang.Application.Stores;

namespace LangLang.WPF.MVVM;

public interface INavigableDataContext
{
    public NavigationStore NavigationStore { get; }
}