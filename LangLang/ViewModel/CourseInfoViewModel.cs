using LangLang.MVVM;
using LangLang.Stores;

namespace LangLang.ViewModel
{
    public class CourseInfoViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }
        public CourseInfoViewModel(NavigationStore navigationStore)
        {
            NavigationStore = navigationStore;
        }

    }
}
