using System.Windows;
using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class TutorWindow : NavigableWindow
{
    public TutorWindow(TutorViewModel tutorViewModel, ILangLangWindowFactory windowFactory)
        : base(tutorViewModel, windowFactory)
    {
        InitializeComponent();
        DataContext = tutorViewModel;
    }
}