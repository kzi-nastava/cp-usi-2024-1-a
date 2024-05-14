using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Exam;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Tutor.Exam
{
    public partial class FinishedExamWindow : NavigableWindow
    {
        public FinishedExamWindow(FinishedExamViewModel finishedExamViewModel, ILangLangWindowFactory windowFactory)
            : base(finishedExamViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = finishedExamViewModel;
        }
    }
}
