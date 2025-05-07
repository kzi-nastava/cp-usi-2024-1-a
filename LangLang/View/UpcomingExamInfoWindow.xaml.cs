using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for ExamInfoViewModel.xaml
    /// </summary>
    public partial class UpcomingExamInfoWindow : NavigableWindow
    {
        public UpcomingExamInfoWindow(UpcomingExamInfoViewModel upcomingExamInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(upcomingExamInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = upcomingExamInfoViewModel;
        }
    }
}
