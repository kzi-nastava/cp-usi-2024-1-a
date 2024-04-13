using LangLang.Services;
using LangLang.ViewModel;
using System.Windows;

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
