using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Exam;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Tutor.Exam
{
    public partial class ActiveExamWindow : NavigableWindow
    {
        public ActiveExamWindow(ActiveExamViewModel activeExamViewModel, ILangLangWindowFactory windowFactory)
            : base(activeExamViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = activeExamViewModel;
        }
    }
}
