using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    public partial class FinishedExamInfoWindow : NavigableWindow
    {
        public FinishedExamInfoWindow(FinishedExamInfoViewModel finishedExamInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(finishedExamInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = finishedExamInfoViewModel;
        }
    }
}
