using LangLang.MVVM;
using LangLang.View;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.ViewModel
{
    public class DirectorViewModel : ViewModelBase, INavigableDataContext
    {
        public RelayCommand OpenTutorTableCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;

        public NavigationStore NavigationStore { get; }

        public DirectorViewModel(ILoginService loginService, INavigationService navigationService, NavigationStore navigationStore)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            OpenTutorTableCommand = new RelayCommand(execute => OpenTutorTable());
            LogoutCommand = new RelayCommand(execute => Logout());
        }

        private void OpenTutorTable()
        {
            TutorTableWindow tutorTable = new TutorTableWindow();
            tutorTable.Show();
        }

        private void Logout()
        {
            _loginService.LogOut();
            _navigationService.Navigate(ViewType.Login);
        }
    }
}
