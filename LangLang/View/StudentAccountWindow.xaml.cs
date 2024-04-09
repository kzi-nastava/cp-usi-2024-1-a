using LangLang.ViewModel;
using System;
using System.Windows;

namespace LangLang.View
{
    public partial class StudentAccountWindow : Window
    {
        public StudentAccountWindow()
        {
            InitializeComponent();
            DataContext = new StudentAccountViewModel();

            //initialize datepicker requirements
            datePicker.DisplayDateStart = new DateTime(1924, 1, 1);
            datePicker.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16

        }
    }
}
