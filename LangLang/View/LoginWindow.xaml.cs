using LangLang.ViewModel;
using System.Windows;
using System.Windows.Controls;
using LangLang.MVVM;
using LangLang.View.Factories;

namespace LangLang.View
{
    public partial class LoginWindow : NavigableWindow
    {
        public LoginWindow(LoginViewModel loginViewModel, ILangLangWindowFactory windowFactory)
            : base(loginViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = loginViewModel;
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
