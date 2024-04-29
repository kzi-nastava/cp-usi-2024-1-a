using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    public partial class ActiveCourseInfoWindow : NavigableWindow
    {
        public ActiveCourseInfoWindow(ActiveCourseInfoViewModel activeCourseInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(activeCourseInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = activeCourseInfoViewModel;
        }
    }
}
