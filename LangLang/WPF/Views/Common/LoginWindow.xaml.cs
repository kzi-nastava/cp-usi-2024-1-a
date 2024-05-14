using System.Windows;
using System.Windows.Controls;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Common
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
