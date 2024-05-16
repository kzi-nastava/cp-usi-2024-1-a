using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;
using System;

namespace LangLang.WPF.ViewModels.Director
{
    public class DirectorMenuViewModel : ViewModelBase, INavigableDataContext
    {
        public RelayCommand OpenTutorTableCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;
        private readonly Domain.Model.Director _loggedInUser;


        public NavigationStore NavigationStore { get; }

        public DirectorMenuViewModel(ILoginService loginService, INavigationService navigationService, NavigationStore navigationStore, IAuthenticationStore authenticationStore)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            OpenTutorTableCommand = new RelayCommand(execute => OpenTutorTable());
            LogoutCommand = new RelayCommand(execute => Logout());
            _loggedInUser = (Domain.Model.Director?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create DirectorViewModel without currently logged in director");
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
