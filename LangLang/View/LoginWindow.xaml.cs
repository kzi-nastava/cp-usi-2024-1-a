using LangLang.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this);
        }

        private void OpenRegister(object sender, RoutedEventArgs e)
        {
            RegisterView view = new RegisterView();
            view.Show();
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = passwordBox.SecurePassword;
            }
        }

    }
}
