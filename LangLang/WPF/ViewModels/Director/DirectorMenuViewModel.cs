using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.WPF.ViewModels.Director
{
    public class DirectorMenuViewModel : ViewModelBase, INavigableDataContext
    {
        public RelayCommand OpenTutorTableCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;

        public NavigationStore NavigationStore { get; }

        public DirectorMenuViewModel(ILoginService loginService, INavigationService navigationService, NavigationStore navigationStore)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            OpenTutorTableCommand = new RelayCommand(execute => OpenTutorTable());
            LogoutCommand = new RelayCommand(execute => Logout());
        }

        private void OpenTutorTable()
        {
            _navigationService.Navigate(ViewType.TutorTable);
        }

        private void Logout()
        {
            _loginService.LogOut();
            _navigationService.Navigate(ViewType.Login);
        }
    }
}
