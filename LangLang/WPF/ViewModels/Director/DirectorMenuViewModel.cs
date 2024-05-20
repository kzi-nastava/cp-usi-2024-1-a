using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.ViewModels.Factories;
using System;

namespace LangLang.WPF.ViewModels.Director
{
    public class DirectorMenuViewModel : ViewModelBase, INavigableDataContext
    {
        public RelayCommand NavCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }

        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;
        private readonly Domain.Model.Director _loggedInUser;
        private readonly ILangLangViewModelFactory _viewModelFactory;

        private ViewModelBase currentViewModel;

        private TutorOverviewViewModel? tutorOverviewViewModel;
        private CourseOverviewViewModel? courseOverviewViewModel;
        private ExamOverviewViewModel? examOverviewViewModel;
        private TutorOverviewViewModel TutorOverviewViewModel
        {
            get
            {
                if (tutorOverviewViewModel == null)
                {
                    tutorOverviewViewModel = (TutorOverviewViewModel)_viewModelFactory.CreateViewModel(ViewType.Tutor);
                }

                return tutorOverviewViewModel;
            }
        }
        private ExamOverviewViewModel ExamOverviewViewModel
        {
            get
            {
                if (examOverviewViewModel == null)
                {
                    examOverviewViewModel = (ExamOverviewViewModel)_viewModelFactory.CreateViewModel(ViewType.Exam);
                }

                return examOverviewViewModel;
            }
        }
        private CourseOverviewViewModel CourseOverviewViewModel
        {
            get
            {
                if (courseOverviewViewModel == null)
                {
                    courseOverviewViewModel = (CourseOverviewViewModel)_viewModelFactory.CreateViewModel(ViewType.Course);
                }

                return courseOverviewViewModel;
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            private set => SetField(ref currentViewModel, value);
        }

        public NavigationStore NavigationStore { get; }

        public DirectorMenuViewModel(ILoginService loginService, INavigationService navigationService, NavigationStore navigationStore, 
            IAuthenticationStore authenticationStore, ILangLangViewModelFactory viewModelFactory)
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            NavCommand = new RelayCommand(execute => OnNav(execute as string));
            _viewModelFactory = viewModelFactory;
            CurrentViewModel = CourseOverviewViewModel;
            //OpenTutorTableCommand = new RelayCommand(execute => OpenTutorTable());
            LogoutCommand = new RelayCommand(execute => Logout());
            _loggedInUser = (Domain.Model.Director?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create DirectorViewModel without currently logged in director");
        }


        private void OnNav(string? destination)
        {
            CurrentViewModel = destination switch
            {
                "courses" => CourseOverviewViewModel,
                "exams" => ExamOverviewViewModel,
                "tutor" => TutorOverviewViewModel,
                _ => CurrentViewModel
            };
        }


        private void Logout()
        {
            _loginService.LogOut();
            _navigationService.Navigate(ViewType.Login);
        }
    }
}
