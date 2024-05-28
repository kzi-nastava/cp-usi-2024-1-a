using System;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.ViewModels.Factories;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.ViewModels.Tutor.Exam;

namespace LangLang.WPF.ViewModels.Tutor
{
    public class TutorMenuViewModel : ViewModelBase, INavigableDataContext
    {
        private Domain.Model.Tutor loggedInUser;
        
        private ViewModelBase currentViewModel;

        private ExamOverviewViewModel? examViewModel;
        private CourseOverviewViewModel? courseViewModel;
        private LoginViewModel? loginViewModel;

        private ILoginService _loginService;
        private INavigationService _navigationService;
        private IPopupNavigationService _popupNavigationService;

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

        private ExamOverviewViewModel ExamOverviewViewModel
        {
            get
            {
                if (examViewModel == null)
                {
                    examViewModel = (ExamOverviewViewModel)_viewModelFactory.CreateViewModel(ViewType.ExamTutor);
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

        private CourseOverviewViewModel CourseOverviewViewModel
        {
            get
            {
                if (courseViewModel == null)
                {
                    courseViewModel = (CourseOverviewViewModel)_viewModelFactory.CreateViewModel(ViewType.CourseTutor);
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
        public RelayCommand NotificationsCommand { get; }
        public RelayCommand LogoutCommand { get; set; }

        public TutorMenuViewModel(
            IAuthenticationStore authenticationStore, ILoginService loginService,
            INavigationService navigationService, IPopupNavigationService popupNavigationService,
            NavigationStore navigationStore, ILangLangViewModelFactory viewModelFactory
            )
        {
            _loginService = loginService;
            _navigationService = navigationService;
            _popupNavigationService = popupNavigationService;
            NavigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
            loggedInUser = (Domain.Model.Tutor?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create TutorViewModel without currently logged in tutor");
            currentViewModel = CourseOverviewViewModel; // TODO: change to ProfileViewModel
            NavCommand = new RelayCommand(execute => OnNav(execute as string));
            NotificationsCommand = new RelayCommand(_ => OpenNotificationWindow());
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
                "courses" => CourseOverviewViewModel,
                "exams" => ExamOverviewViewModel,
                _ => CurrentViewModel
            };
        }
        
        private void OpenNotificationWindow()
        {
            _popupNavigationService.Navigate(ViewType.Notifications);
        }
    }
}
