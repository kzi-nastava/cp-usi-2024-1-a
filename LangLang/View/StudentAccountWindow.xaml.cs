using LangLang.ViewModel;
using System;
using System.Windows;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.View.Factories;

namespace LangLang.View
{
    public partial class StudentAccountWindow : NavigableWindow
    {
        public StudentAccountWindow(StudentAccountViewModel studentAccountViewModel, IWindowFactory windowFactory)
            : base(studentAccountViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = studentAccountViewModel;

            //initialize datepicker requirements
            datePicker.DisplayDateStart = new DateTime(1924, 1, 1);
            datePicker.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16

        }
    }
}
