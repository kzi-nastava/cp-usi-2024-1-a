using System;
using System.Windows;
using LangLang.MVVM;
using LangLang.Services;
using LangLang.Services.AuthenticationServices;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View
{
    public partial class RegisterWindow : NavigableWindow
    {
        public RegisterWindow(RegisterViewModel registerViewModel, ILangLangWindowFactory windowFactory)
            : base(registerViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = registerViewModel;

            //initialize datepicker
            datePicker.DisplayDateStart = new DateTime(1924, 1, 1);
            datePicker.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16
        }
    }
}
