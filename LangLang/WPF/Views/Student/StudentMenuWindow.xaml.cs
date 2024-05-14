using System.Windows;
using System.Windows.Controls;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Student;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Student
{
    public partial class StudentMenuWindow : NavigableWindow
    {
        public StudentMenuWindow(StudentMenuViewModel studentMenuViewModel, ILangLangWindowFactory windowFactory)
            : base(studentMenuViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = studentMenuViewModel;
        }
        

        private void RateTutorButton(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Course course)
            {
                string courseId = course.Id;
                if (DataContext is StudentMenuViewModel viewModel)
                {
                    viewModel.RateTutorCommand.Execute(courseId);
                }
            }
        }
    }
}
