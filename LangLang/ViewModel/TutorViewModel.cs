using System;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.ViewModel.Factories;

namespace LangLang.ViewModel
{
    public class TutorViewModel : ViewModelBase, INavigableDataContext
    {
        private Tutor loggedInUser;
        
        private ViewModelBase currentViewModel;

        private ExamViewModel? examViewModel;
        private CourseViewModel? courseViewModel;
        private LoginViewModel? loginViewModel;

        private ILoginService _loginService;
        private INavigationService _navigationService;

        private ILangLangViewModelFactory _viewModelFactory;
        
        public NavigationStore NavigationStore { get; }
        
        private string tutorName = "";
        public string TutorName
        {
            get => tutorName;
            set
            {
                tutorName = value;
                OnPropertyChanged();
            }
        }

        private ExamViewModel ExamViewModel
        {
            get
            {
                if (examViewModel == null)
                {
                    examViewModel = (ExamViewModel)_viewModelFactory.CreateViewModel(ViewType.Exam);
                }
        
                return examViewModel;
            }
        }
        private LoginViewModel LoginViewModel
        {
            get
            {
                if (loginViewModel == null)
                {
                    loginViewModel = (LoginViewModel)_viewModelFactory.CreateViewModel(ViewType.Login);
                }

                return loginViewModel;
            }
        }

        private CourseViewModel CourseViewModel
        {
            get
            {
                if (courseViewModel == null)
                {
                    courseViewModel = (CourseViewModel)_viewModelFactory.CreateViewModel(ViewType.Course);
                }
        
                return courseViewModel;
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            private set => SetField(ref currentViewModel, value);
        }
        public RelayCommand NavCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        public TutorViewModel(
            IAuthenticationStore authenticationStore, ILoginService loginService,
            INavigationService navigationService, NavigationStore navigationStore,
            ILangLangViewModelFactory viewModelFactory
            )
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
            loggedInUser = (Tutor?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create TutorViewModel without currently logged in tutor");
            currentViewModel = CourseViewModel; // TODO: change to ProfileViewModel
            NavCommand = new RelayCommand(execute => OnNav(execute as string));
            LogoutCommand = new RelayCommand(execute => Logout());
            TutorName = loggedInUser.Name;
        }

        private void Logout()
        {
            _loginService.LogOut();
            _navigationService.Navigate(ViewType.Login);
        }

        private void OnNav(string? destination)
        {
            CurrentViewModel = destination switch
            {
                "courses" => CourseViewModel,
                "exams" => ExamViewModel,
                _ => CurrentViewModel
            };
        }
    }
}
