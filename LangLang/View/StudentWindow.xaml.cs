using LangLang.Model;
using LangLang.Services;
using LangLang.ViewModel;
using System.Windows;
using LangLang.MVVM;
using LangLang.Services.UserServices;
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
        
        /*
        private void CancelAtendingCourseButton(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Course course)
            {
                if (DataContext is StudentViewModel viewModel)
                {
                    viewModel.CancelAttendingCourseCommand.Execute(course);
                }
            }
        }
        */



        private void ApplyButtonCourse(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Course course)
            {
                string courseId = course.Id;
                if (DataContext is StudentViewModel viewModel)
                {
                    viewModel.ApplyCourseCommand.Execute(courseId);
                }
            }
        }

        private void CancelButtonCourse(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Course course)
            {
                string courseId = course.Id;
                if (DataContext is StudentViewModel viewModel)
                {
                    viewModel.CancelCourseCommand.Execute(courseId);
                }
            }
        }

        private void ApplyButtonExam(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Exam exam)
            {
                string examId = exam.Id;
                if (DataContext is StudentViewModel viewModel)
                {
                    viewModel.ApplyExamCommand.Execute(examId);
                }
            }
        }

        private void CancelButtonExam(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Exam exam)
            {
                string examId = exam.Id;
                if (DataContext is StudentViewModel viewModel)
                {
                    viewModel.CancelExamCommand.Execute(examId);
                }
            }
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
