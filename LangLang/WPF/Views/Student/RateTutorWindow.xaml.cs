using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Student;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Student
{
    public partial class RateTutorWindow : NavigableWindow
    {
        public RateTutorWindow(RateTutorViewModel rateTutorViewModel, ILangLangWindowFactory windowFactory)
            : base(rateTutorViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = rateTutorViewModel;
        }

    }
}
