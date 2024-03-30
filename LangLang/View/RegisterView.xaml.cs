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
using LangLang.ViewModel;

namespace LangLang.View
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel(this);
        }


        private void OpenLogin(object sender, RoutedEventArgs e)
        {
            LoginWindow view = new LoginWindow();
            view.Show();
            this.Close();
        }
















    }
}
