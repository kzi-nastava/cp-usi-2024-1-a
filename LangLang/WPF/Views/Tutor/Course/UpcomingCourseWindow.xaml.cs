using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Tutor.Course
{
    /// <summary>
    /// Interaction logic for CourseInfoViewModel.xaml
    /// </summary>
    public partial class UpcomingCourseWindow : NavigableWindow
    {
        public UpcomingCourseWindow(UpcomingCourseViewModel upcomingCourseViewModel, ILangLangWindowFactory windowFactory)
            : base(upcomingCourseViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = upcomingCourseViewModel;
        }
    }
}
