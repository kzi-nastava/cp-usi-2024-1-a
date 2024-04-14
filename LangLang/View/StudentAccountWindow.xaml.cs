using LangLang.ViewModel;
using System;
using System.Windows;
using LangLang.Services.AuthenticationServices;

namespace LangLang.View
{
    public partial class StudentAccountWindow : Window
    {
        public StudentAccountWindow()
        {
            InitializeComponent();
            DataContext = new StudentAccountViewModel(new RegisterService());

            //initialize datepicker requirements
            datePicker.DisplayDateStart = new DateTime(1924, 1, 1);
            datePicker.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16

        }
    }
}
