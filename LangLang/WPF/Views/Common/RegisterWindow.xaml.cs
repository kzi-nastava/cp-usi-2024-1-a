using System;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Common
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
            datePicker.SelectedDate = DateTime.Today.AddYears(-16);
        }
    }
}
