using LangLang.Stores;

namespace LangLang.MVVM;

public interface INavigableDataContext
{
    public NavigationStore NavigationStore { get; }
}