using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private readonly LoginService _loginService;

        public LoginViewModel()
        {
            _loginService = LoginService.GetInstance();
            LoginCommand = new RelayModel(Login);
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        private void Login(object parameter)
        {
            MessageBox.Show($"Email inputted: {Email}\nPassword: {Password}");

            _loginService.LogIn(Email, Password);

            if (!_loginService.validUser)
            {
                if (!_loginService.validEmail)
                {
                    MessageBox.Show("User doesn't exist", "", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Wrong password", "", MessageBoxButton.OK);
                }
            }
            else
            {
                StudentService ss = StudentService.GetInstance();
                MessageBox.Show($"Successful login Email: {Email}\nPassword: {Password}\nName: {ss.LoggedUser.Name}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
