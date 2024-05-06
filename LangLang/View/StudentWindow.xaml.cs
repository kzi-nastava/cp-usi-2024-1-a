using LangLang.Model;
using LangLang.ViewModel;
using System.Windows;
using LangLang.MVVM;
using LangLang.View.Factories;
using System.Windows.Controls;

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
        

        private void RateTutorButton(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Course course)
            {
                string courseId = course.Id;
                if (DataContext is StudentViewModel viewModel)
                {
                    viewModel.RateTutorCommand.Execute(courseId);
                }
            }
        }
    }
}
