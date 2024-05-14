using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Tutor;

public partial class TutorMenuWindow : NavigableWindow
{
    public TutorMenuWindow(TutorMenuViewModel tutorMenuViewModel, ILangLangWindowFactory windowFactory)
        : base(tutorMenuViewModel, windowFactory)
    {
        InitializeComponent();
        DataContext = tutorMenuViewModel;
    }
}