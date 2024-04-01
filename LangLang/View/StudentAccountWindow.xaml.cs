using LangLang.ViewModel;
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
    /// <summary>
    /// Interaction logic for StudentAccountWindow.xaml
    /// </summary>
    public partial class StudentAccountWindow : Window
    {
        public StudentAccountWindow()
        {
            InitializeComponent();
            DataContext = new StudentAccountViewModel(this);

            datePicker.DisplayDateStart = new DateTime(1924, 1, 1);
            datePicker.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16

        }
    }
}
