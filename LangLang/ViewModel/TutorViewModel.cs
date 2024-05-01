using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;

namespace LangLang.ViewModel
{
    internal class TutorViewModel : ViewModelBase
    {
        private Tutor loggedInUser;
        
        private ViewModelBase currentViewModel;

        private ExamViewModel? examViewModel;
        private CourseViewModel? courseViewModel;

        private string tutorName = "";
        public string TutorName
        {
            get { return tutorName; }
            set
            {
                tutorName = value;
                OnPropertyChanged();
            }
        }
        private LanguageService? languageService;
        public LanguageService LanguageService => languageService ??= new LanguageService();
        
        private TimetableService? timetableService;
        public TimetableService TimetableService => timetableService ??= new TimetableService();

        public ExamViewModel ExamViewModel
        {
            get
            {
                if (examViewModel == null)
                {
                    ExamService examService = new ExamService();
                    examViewModel = new ExamViewModel(loggedInUser, examService, TimetableService);
                }
        
                return examViewModel;
            }
        }
        
        public CourseViewModel CourseViewModel
        {
            get
            {
                if (courseViewModel == null)
                {
                    CourseService courseService = new CourseService();
                    courseViewModel = new CourseViewModel(loggedInUser,courseService, LanguageService, TimetableService);
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

        public TutorViewModel(Tutor loggedInUser)
        {
            this.loggedInUser = loggedInUser;
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
