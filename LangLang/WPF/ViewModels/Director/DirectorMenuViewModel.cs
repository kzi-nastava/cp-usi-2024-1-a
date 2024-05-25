﻿using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.ViewModels.Tutor.Exam;
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
        private CourseOverviewForDirectorViewModel? courseOverviewViewModel;
        private ExamOverviewForDirectorViewModel? examOverviewViewModel;
        private TutorOverviewViewModel TutorOverviewViewModel
        {
            get
            {
                if (tutorOverviewViewModel == null)
                {
                    tutorOverviewViewModel = (TutorOverviewViewModel)_viewModelFactory.CreateViewModel(ViewType.TutorTable);
                }

                return tutorOverviewViewModel;
            }
        }
        private ExamOverviewForDirectorViewModel ExamOverviewViewModel
        {
            get
            {
                if (examOverviewViewModel == null)
                {
                    examOverviewViewModel = (ExamOverviewForDirectorViewModel)_viewModelFactory.CreateViewModel(ViewType.ExamDirector);
                }

                return examOverviewViewModel;
            }
        }
        private CourseOverviewForDirectorViewModel CourseOverviewViewModel
        {
            get
            {
                if (courseOverviewViewModel == null)
                {
                    courseOverviewViewModel = (CourseOverviewForDirectorViewModel)_viewModelFactory.CreateViewModel(ViewType.CourseDirector);
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
            currentViewModel = TutorOverviewViewModel;
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
                "tutors" => TutorOverviewViewModel,
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
