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
            // Set the DataContext to an instance of LoginViewModel
            DataContext = new LoginViewModel(this);
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null && DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = passwordBox.SecurePassword;
            }
        }

    }
}
