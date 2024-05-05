using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    public partial class FinishedCourseInfoWindow : NavigableWindow
    {
        public FinishedCourseInfoWindow(FinishedCourseInfoViewModel finishedCourseInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(finishedCourseInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = finishedCourseInfoViewModel;
        }
    }
}
