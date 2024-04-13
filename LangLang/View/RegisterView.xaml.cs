using System;
using System.Windows;
using LangLang.ViewModel;

namespace LangLang.View
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel(this);

            //initialize datepicker
            datePicker.DisplayDateStart = new DateTime(1924, 1, 1);
            datePicker.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16
        }


        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            LoginWindow view = new LoginWindow();
            view.Show();
            this.Close();
        }

    }
}
