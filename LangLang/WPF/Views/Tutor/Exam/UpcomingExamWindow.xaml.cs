using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Exam;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Tutor.Exam
{
    /// <summary>
    /// Interaction logic for ExamInfoViewModel.xaml
    /// </summary>
    public partial class UpcomingExamWindow : NavigableWindow
    {
        public UpcomingExamWindow(UpcomingExamViewModel upcomingExamViewModel, ILangLangWindowFactory windowFactory)
            : base(upcomingExamViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = upcomingExamViewModel;
        }
    }
}
