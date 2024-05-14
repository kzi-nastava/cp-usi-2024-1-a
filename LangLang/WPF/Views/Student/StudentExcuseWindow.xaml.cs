using System.Windows;

namespace LangLang.WPF.Views.Student
{
    public partial class StudentExcuseWindow : Window
    {
        public string UserMessage { get; private set; }

        public StudentExcuseWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            UserMessage = txtMessage.Text;
            Close();
        }

    }
}
