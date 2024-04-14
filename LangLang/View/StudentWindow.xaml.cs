using LangLang.Model;
using LangLang.Services;
using LangLang.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View
{
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
            DataContext = new StudentViewModel(this);
        }

        private void OpenStudentProfile(object sender, RoutedEventArgs e)
        {
            StudentAccountWindow view = new StudentAccountWindow();
            view.Show();
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


        private void DeleteProfile(object sender, RoutedEventArgs e)
        {
            StudentService studentService = StudentService.GetInstance();
            studentService.DeleteMyAccount();
            MessageBox.Show("Your profile has been successfully deleted");

            LoginWindow view = new LoginWindow();
            view.Show();
            this.Close();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
