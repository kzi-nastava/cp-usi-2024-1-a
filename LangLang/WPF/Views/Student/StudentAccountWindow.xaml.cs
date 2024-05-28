using System;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Student;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Student
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
