using LangLang.Services;
using LangLang.ViewModel;
using System.Windows;
using LangLang.MVVM;
using LangLang.Services.UserServices;
using LangLang.View.Factories;

namespace LangLang.View
{
    public partial class StudentWindow : NavigableWindow
    {
        public StudentWindow(StudentViewModel studentViewModel, ILangLangWindowFactory windowFactory)
            : base(studentViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = studentViewModel;
        }
    }
}
