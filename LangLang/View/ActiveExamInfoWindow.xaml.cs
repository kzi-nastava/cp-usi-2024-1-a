using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    public partial class ActiveExamInfoWindow : NavigableWindow
    {
        public ActiveExamInfoWindow(ActiveExamInfoViewModel activeExamInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(activeExamInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = activeExamInfoViewModel;
        }
    }
}
