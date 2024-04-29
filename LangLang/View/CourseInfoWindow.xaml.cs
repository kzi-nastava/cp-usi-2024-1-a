using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for CourseInfoViewModel.xaml
    /// </summary>
    public partial class CourseInfoWindow : NavigableWindow
    {
        public CourseInfoWindow(CourseInfoViewModel courseInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(courseInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = courseInfoViewModel;
        }
    }
}
