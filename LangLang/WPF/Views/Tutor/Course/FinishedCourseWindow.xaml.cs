using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Tutor.Course
{
    public partial class FinishedCourseWindow : NavigableWindow
    {
        public FinishedCourseWindow(FinishedCourseViewModel finishedCourseViewModel, ILangLangWindowFactory windowFactory)
            : base(finishedCourseViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = finishedCourseViewModel;
        }
    }
}
