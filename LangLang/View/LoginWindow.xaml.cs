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
            DataContext = new LoginViewModel();
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                ((LoginViewModel)DataContext).Password = passwordBox.SecurePassword;
            }
        }

    }
}
