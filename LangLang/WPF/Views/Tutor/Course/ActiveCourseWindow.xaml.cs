using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Tutor.Course
{
    public partial class ActiveCourseWindow : NavigableWindow
    {
        public ActiveCourseWindow(ActiveCourseViewModel activeCourseViewModel, ILangLangWindowFactory windowFactory)
            : base(activeCourseViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = activeCourseViewModel;
        }
    }
}
