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

        public TutorViewModel(
            AuthenticationStore authenticationStore, ILoginService loginService,
            INavigationService navigationService, NavigationStore navigationStore,
            ILangLangViewModelFactory viewModelFactory
            )
        {
            _loginService = loginService;
            _navigationService = navigationService;
            NavigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
            loggedInUser = (Tutor?)authenticationStore.CurrentUser ??
                                throw new InvalidOperationException(
                                    "Cannot create TutorViewModel without currently logged in tutor");
            currentViewModel = CourseViewModel; // TODO: change to ProfileViewModel
            NavCommand = new RelayCommand(execute => OnNav(execute as string));
            TutorName = loggedInUser.Name;
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
