using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for CourseInfoViewModel.xaml
    /// </summary>
    public partial class UpcomingCourseInfoWindow : NavigableWindow
    {
        public UpcomingCourseInfoWindow(UpcomingCourseInfoViewModel upcomingCourseInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(upcomingCourseInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = upcomingCourseInfoViewModel;
        }
    }
}
