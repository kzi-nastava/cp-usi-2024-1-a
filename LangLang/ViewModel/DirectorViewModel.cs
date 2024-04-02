using LangLang.MVVM;
using LangLang.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ViewModel
{
    internal class DirectorViewModel
    {
        private readonly DirectorWindow window;
        public RelayCommand OpenTutorTableCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        public DirectorViewModel(DirectorWindow window)
        {
            OpenTutorTableCommand = new RelayCommand(execute => OpenTutorTable());
            LogoutCommand = new RelayCommand(execute => Logout());
            this.window = window;   
        }

        private void OpenTutorTable()
        {
            TutorTableWindow tutorTable = new TutorTableWindow();
            tutorTable.Show();
            window.Close();
        }

        private void Logout()
        {
            LoginWindow login = new LoginWindow(); 
            login.Show();
            window.Close();
        }
    }
}
