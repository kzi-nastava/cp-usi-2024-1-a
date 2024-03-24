using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace LangLang.View
{
    public partial class LoginWindow : Window
    {
        private LoginService loginService;
        public LoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            loginService = LoginService.GetInstance();
        }




        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            MessageBox.Show($"Email inputed: {email}\nPassword: {password}");



            loginService.LogIn(email, password);


            //LoginFailed
            if (!loginService.validUser)
            {
                if (!loginService.validEmail)
                {
                    MessageBox.Show($"User doesn't exist", "", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show($"Wrong password", "", MessageBoxButton.OK);
                }
            }
            else
            {
                StudentService ss = StudentService.GetInstance();
                MessageBox.Show($"Successful login Email: {email}\nPassword: {password}\n Name{ss.LoggedUser.Name}");
            }


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Placeholder for handling text change event
        }
    }
}

