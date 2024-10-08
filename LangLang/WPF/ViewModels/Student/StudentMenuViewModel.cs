﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.UseCases.Common;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Navigation;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;
using LangLang.WPF.Views.Student;

namespace LangLang.WPF.ViewModels.Student
{
    public class StudentMenuViewModel : ViewModelBase, INavigableDataContext
    {
        private readonly Domain.Model.Student _loggedInUser;
        private readonly ICourseService _courseService;
        private readonly ILanguageService _languageService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly CurrentCourseStore _currentCourseStore;
        private readonly IExamService _examService;
        private readonly IStudentCourseCoordinator _courseCoordinator;
        private readonly IAccountService _accountService;
        private readonly IExamCoordinator _examCoordinator;
        private readonly IExamApplicationService _examApplicationService;
        public ICommand ClearExamFiltersCommand { get; }
        public ICommand ClearCourseFiltersCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand DeleteProfileCommand { get; }
        public ICommand OpenStudentProfileCommand { get; }
        public ICommand ApplyCourseCommand { get; }
        public ICommand CancelCourseCommand{ get; }
        public ICommand ApplyExamCommand { get; }
        public ICommand CancelExamCommand { get; }
        public ICommand RateTutorCommand { get; }
        public ICommand CancelAttendingCourseCommand { get; }
        public ICommand CancelAttendingExamCommand { get; }
        public ICommand OpenNotificationWindowCommand { get; }
        public ObservableCollection<CourseViewModel> Courses { get; set; }
        public ObservableCollection<CourseViewModel> FinishedCourses { get; set; }
        public ObservableCollection<CourseViewModel> AppliedCourses { get; set; }
        public ObservableCollection<CourseViewModel> AttendingCourse { get; set; }
        public ObservableCollection<ExamViewModel> AvailableExams { get; set; }
        public ObservableCollection<ExamViewModel> AppliedExams { get; set; }
        public ObservableCollection<ExamViewModel> FinishedExams { get; set; }
        public ObservableCollection<ExamViewModel> AttendingExams { get; set; }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLevel> Levels { get; set; }
        public ObservableCollection<int?> Durations { get; set; }

        private string name = "";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string languageName = "";
        public string LanguageName
        {
            get { return languageName; }
            set
            {
                languageName = value;
                OnPropertyChanged();
            }
        }

        private LanguageLevel level;
        public LanguageLevel Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged();
            }
        }

        private int? duration;
        public int? Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }

        private DateTime? start;
        public DateTime? Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged();
            }
        }

        private bool online;
        public bool Online
        {
            get { return online; }
            set
            {
                online = value;
                OnPropertyChanged();
            }
        }

        // FILTER VALUES
        private string courseLanguageFilter = "";
        public string CourseLanguageFilter
        {
            get { return courseLanguageFilter; }
            set
            {
                courseLanguageFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private LanguageLevel? courseLevelFilter;
        public LanguageLevel? CourseLevelFilter
        {
            get { return courseLevelFilter; }
            set
            {
                courseLevelFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private DateTime? courseStartFilter;
        public DateTime? CourseStartFilter
        {
            get { return courseStartFilter; }
            set
            {
                courseStartFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private bool? courseOnlineFilter;
        public bool? CourseOnlineFilter
        {
            get { return courseOnlineFilter; }
            set
            {
                courseOnlineFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private int courseDurationFilter;
        public int CourseDurationFilter
        {
            get { return courseDurationFilter; }
            set
            {
                courseDurationFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private Course? selectedItem;
        public Course? SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (selectedItem != null)
                {
                    Name = selectedItem.Name;
                    LanguageName = selectedItem.Language.Name;
                    Level = selectedItem.Level;
                    Duration = selectedItem.Duration;
                    Start = selectedItem.Start;
                    Online = selectedItem.Online;
                }
                OnPropertyChanged();
            }
        }

        private readonly ILoginService _loginService;
        private readonly INavigationService _navigationService;
        private readonly IPopupNavigationService _popupNavigationService;
        public NavigationStore NavigationStore { get; }
        
        public StudentMenuViewModel(IStudentService studentService, CurrentCourseStore currentCourseStore,IAccountService accountService, ILoginService loginService, IStudentCourseCoordinator courseCoordinator, INavigationService navigationService, ITutorService tutorService, IPopupNavigationService popupNavigationService, NavigationStore navigationStore, ICourseService courseService, ILanguageService languageService, IExamService examService, IAuthenticationStore authenticationStore, IExamCoordinator examCoordinator, IExamApplicationService examApplicationService)
        {
            _loggedInUser = (Domain.Model.Student?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create StudentViewModel without currently logged in student");
            NavigationStore = navigationStore;
            _currentCourseStore = currentCourseStore;
            _courseService = courseService;
            _accountService = accountService;
            _languageService = languageService;
            _tutorService = tutorService;
            _examService = examService;
            _examCoordinator = examCoordinator;
            _examApplicationService = examApplicationService;
            _studentService = studentService;
            _loginService = loginService;
            _courseCoordinator = courseCoordinator;
            _navigationService = navigationService;
            _popupNavigationService = popupNavigationService;

            Courses = new ObservableCollection<CourseViewModel>();
            FinishedCourses = new ObservableCollection<CourseViewModel>();
            AttendingCourse = new ObservableCollection<CourseViewModel>(); 
            AppliedCourses = new ObservableCollection<CourseViewModel>();
            AvailableExams = new ObservableCollection<ExamViewModel>();
            AppliedExams = new ObservableCollection<ExamViewModel>();
            AttendingExams = new ObservableCollection<ExamViewModel>();
            FinishedExams = new ObservableCollection<ExamViewModel>();
            Languages = new ObservableCollection<string?>();
            Levels = new ObservableCollection<LanguageLevel>();
            Durations = new ObservableCollection<int?> { null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            Start = DateTime.Now;

            LoadAvailableExams();
            LoadAppliedExams();
            LoadAttendingExams();
            LoadFinishedExams();
            LoadLanguages();
            LoadCourses();
            LoadAttendingCourse();
            LoadAppliedCourses();
            LoadFinishedCourses();
            LoadLanguageLevels();
            //initialize commands
            ClearCourseFiltersCommand = new RelayCommand(ClearCourseFilters);
            ClearExamFiltersCommand = new RelayCommand(ClearExamFilters);
            LogOutCommand = new RelayCommand(_ => LogOut());
            DeleteProfileCommand = new RelayCommand(_ => DeleteProfile());
            OpenStudentProfileCommand = new RelayCommand(_ => OpenStudentProfile());
            ApplyCourseCommand = new RelayCommand<string>(ApplyCourse);
            CancelCourseCommand = new RelayCommand<string>(CancelCourse);
            ApplyExamCommand = new RelayCommand<ExamViewModel>(ApplyExam);
            CancelExamCommand = new RelayCommand<ExamViewModel>(CancelExam);
            RateTutorCommand = new RelayCommand<string>(RateTutor);
            CancelAttendingCourseCommand = new RelayCommand<string>(CancelAttendingCourse!);
            CancelAttendingExamCommand = new RelayCommand(CancelAttendingExam!);
            OpenNotificationWindowCommand = new RelayCommand(_ => OpenNotificationWindow());
        }

        private void CancelAttendingCourse(string courseId)
        {
            Course course = _courseService.GetCourseById(courseId)!;
            try
            {
                string message = "";
                if(_courseCoordinator.CanDropCourse(courseId))
                {
                    var messageWindow = new StudentExcuseWindow();
                    messageWindow.ShowDialog();
                    message = messageWindow.UserMessage;
                    if(message == "")
                    {
                        message = "No further explanation";
                    }
                    MessageBox.Show($"You've successfully dropped {course.Name} course.", "Success");
                }
                _courseCoordinator.DropCourse(_loggedInUser.Id, message);
                AttendingCourse.Clear();
            }
            catch
            {
                MessageBox.Show($"It's too early to drop {course.Name}. Wait before trying again.", "Success");

            }
        }

        private void CancelAttendingExam(object parameter)
        {
            MessageBox.Show($"cancelled exam sent!", "Success");

        }

        private void ApplyCourse(string courseId)
        {
            try
            {
                _courseCoordinator.ApplyForCourse(courseId, _loggedInUser.Id);
                Course course = _courseService.GetCourseById(courseId)!;
                MessageBox.Show($"Application sent! You've successfully applied for {course.Name}!", "Success");

                foreach (CourseViewModel courseViewModel in Courses)
                {
                    if (courseViewModel.Id == courseId)
                    {
                        Courses.Remove(courseViewModel);
                        AppliedCourses.Add(courseViewModel);
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show($"Can't apply to other courses when already attending {AttendingCourse[0].Name} course!", "Fail");
            }
        }

        private void CancelCourse(string courseId)
        {
            Course course = _courseService.GetCourseById(courseId)!;
            MessageBox.Show($"Your application for {course.Name} has been cancelled.", "Success");
            _courseCoordinator.CancelApplication(_loggedInUser.Id, courseId);

            Course cancelledCourse = _courseService.GetCourseById(courseId)!;
            string tutorName = _tutorService.GetTutorById(cancelledCourse.TutorId!)!.GetFullName();
            Courses.Add(new CourseViewModel(cancelledCourse, tutorName));

            foreach(CourseViewModel courseViewModel in AppliedCourses) {
                if(courseViewModel.Id == courseId)
                {
                    AppliedCourses.Remove(courseViewModel);
                    return;
                }
            }
        }

        private void ApplyExam(ExamViewModel examViewModel)
        {
            try
            {
                Exam exam = _examService.GetExamById(examViewModel.Id)!;
                _examCoordinator.ApplyForExam(_loggedInUser, exam);
                MessageBox.Show($"Successful apply for exam.", "Success");
                LoadAvailableExams();
                LoadAppliedExams();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to apply for exam: {e.Message}", "Error");
            }
        }

        private void CancelExam(ExamViewModel examViewModel)
        {
            Exam exam = _examService.GetExamById(examViewModel.Id)!;
            var examApplication = _examApplicationService.GetExamApplication(_loggedInUser.Id, exam.Id);
            if (examApplication == null)
            {
                MessageBox.Show($"No exam application to cancel.", "Error");
                return;
            }
            try
            {
                _examApplicationService.CancelApplication(examApplication);
                MessageBox.Show($"Successfully canceled exam application.", "Success");
                LoadAvailableExams();
                LoadAppliedExams();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to cancel exam application: {e.Message}", "Error");
            }
        }

        private void RateTutor(string courseId)
        {
            _currentCourseStore.CurrentCourse = _courseService.GetCourseById(courseId);
            _popupNavigationService.Navigate(ViewType.RateTutor);
        }

        private void ClearCourseFilters(object? obj)
        {
            CourseLanguageFilter = "";
            CourseLevelFilter = null;
            CourseStartFilter = null;
            CourseOnlineFilter = null;
            CourseDurationFilter = 0;
            Courses.Clear();
            LoadCourses();
            OnPropertyChanged();
        }

        private void ClearExamFilters(object? obj)
        {
            ExamLanguageFilter = "";
            ExamLevelFilter = null;
            ExamStartFilter = null;
            AvailableExams.Clear();
            LoadAvailableExams();
            OnPropertyChanged();
        }

        public void LoadCourses()
        {
            var availableCourses = _courseCoordinator.GetAvailableCourses(_loggedInUser.Id);
            foreach (Course course in availableCourses)
            {
                string tutorName = _tutorService.GetTutorById(course.TutorId!)!.GetFullName();
                Courses.Add(new CourseViewModel(course, tutorName));
            }
        }

        private void LoadAttendingCourse()
        {
            //when trying to test attendance, apply for course
            //in files change application state to 2 (accepted), then change course state to 4 (In progress so i can drop it)
            //_courseCoordinator.GenerateAttendance("2");

            Course attendingCourse = _courseCoordinator.GetStudentAttendingCourse(_loggedInUser.Id)!;
            if(attendingCourse != null)
            {
                string tutorName = "tutor not available";
                if (attendingCourse.TutorId != null)
                {
                    Domain.Model.Tutor tutor = _tutorService.GetTutorById(attendingCourse.TutorId!)!;
                    if (tutor != null)
                    {
                        tutorName = _tutorService.GetTutorById(attendingCourse.TutorId!)!.GetFullName();
                    }
                }
                AttendingCourse.Add(new CourseViewModel(attendingCourse, tutorName));
            }
        }

        private void LoadAppliedCourses()
        {
            foreach (Course course in _courseCoordinator.GetAppliedCoursesStudent(_loggedInUser.Id))
            {
                string tutorName = _tutorService.GetTutorById(course.TutorId!)!.GetFullName();;
                AppliedCourses.Add(new CourseViewModel(course, tutorName));
            }
        }

        private void LoadFinishedCourses()
        {
            var finishedCourses = _courseCoordinator.GetFinishedCoursesStudent(_loggedInUser.Id);
            foreach (Course course in finishedCourses)
            {
                string tutorName = _tutorService.GetTutorById(course.TutorId!)!.GetFullName();
                FinishedCourses.Add(new CourseViewModel(course, tutorName));
            }
        }

        private void LoadAvailableExams()
        {
            AvailableExams.Clear();
            foreach (var exam in _examCoordinator.GetAvailableExams(_loggedInUser))
            {
                AvailableExams.Add(new ExamViewModel(exam));
            }
        }
        
        private void LoadAppliedExams()
        {
            AppliedExams.Clear();
            foreach (var exam in _examCoordinator.GetAppliedExams(_loggedInUser))
            {
                AppliedExams.Add(new ExamViewModel(exam));
            }
        }
        private void LoadFinishedExams()
        {
            FinishedExams.Clear();
            foreach (var exam in _examCoordinator.GetFinishedExams(_loggedInUser.Id))
            {
                FinishedExams.Add(new ExamViewModel(exam));
            }
        }

        private void LoadAttendingExams()
        {
            var exam = _examCoordinator.GetAttendingExam(_loggedInUser.Id);
            AttendingExams = exam == null ? new ObservableCollection<ExamViewModel>() : new ObservableCollection<ExamViewModel>{new ExamViewModel(exam)};
        }

        public void LoadLanguages()
        {
            var languages = _languageService.GetAll();
            foreach (Language language in languages)
            {
                Languages.Add(language.Name);
            }
            Languages.Add("");
        }

        public void LoadLanguageLevels()
        {
            foreach (LanguageLevel lvl in Enum.GetValues(typeof(LanguageLevel)))
            {
                Levels.Add(lvl);
            }
        }

        public void RemoveInputs()
        {
            Name = "";
            LanguageName = "";
            Level = LanguageLevel.A1;
            Duration = null;
            Online = false;
            selectedItem = null;
            Start = DateTime.Now;
        }

        public void FilterCourses()
        {
            Courses.Clear();
            var courses = _courseCoordinator.GetAvailableCourses(_loggedInUser.Id);

            foreach (Course course in courses)
            {
                if ((course.Language.Name == CourseLanguageFilter || CourseLanguageFilter == "") && (course.Level == CourseLevelFilter || CourseLevelFilter == null))
                {
                    if (CourseStartFilter == null || (CourseStartFilter != null && course.Start == ((DateTime)CourseStartFilter)))
                    {
                        if (course.Online == CourseOnlineFilter || CourseOnlineFilter == null)
                        {
                            if (course.Duration == CourseDurationFilter || CourseDurationFilter == 0)
                            {
                                string tutorName = _tutorService.GetTutorById(course.TutorId!)!.GetFullName();
                                Courses.Add(new CourseViewModel(course, tutorName));
                            }
                        }
                    }
                }
            }
        }


        //FILTERING EXAM
        // Add properties for exam filtering criteria
        // Define new filter properties for exams
        private string examLanguageFilter = "";
        public string ExamLanguageFilter
        {
            get { return examLanguageFilter; }
            set
            {
                examLanguageFilter = value;
                FilterExams();
                OnPropertyChanged();
            }
        }

        private LanguageLevel? examLevelFilter;
        public LanguageLevel? ExamLevelFilter
        {
            get { return examLevelFilter; }
            set
            {
                examLevelFilter = value;
                FilterExams();
                OnPropertyChanged();
            }
        }

        private DateTime? examStartFilter;
        public DateTime? ExamStartFilter
        {
            get { return examStartFilter; }
            set
            {
                examStartFilter = value;
                FilterExams();
                OnPropertyChanged();
            }
        }

        // Modify FilterExams method to filter exams based on new criteria
        public void FilterExams()
        {
            // Clear existing exams from the list
            AvailableExams.Clear();

            // Get all exams
            var exams = _examService.GetAllExams();

            // Filter exams based on criteria
            foreach (Exam exam in exams)
            {
                if ((exam.Language.Name == ExamLanguageFilter || ExamLanguageFilter == "") &&
                    (exam.LanguageLevel == ExamLevelFilter || ExamLevelFilter == null))
                {
                    if (ExamStartFilter == null || (ExamStartFilter != null && exam.Time.Date == ExamStartFilter.Value.Date))
                    {
                        AvailableExams.Add(new ExamViewModel(exam));
                    }
                }
            }
        }

        private void LogOut()
        {
            _loginService.LogOut();
            _navigationService.Navigate(ViewType.Login);
        }

        private void DeleteProfile()
        {
            _accountService.DeleteStudent(_loggedInUser);
            MessageBox.Show("Your profile has been successfully deleted");
            _navigationService.Navigate(ViewType.Login);
        }

        private void OpenStudentProfile()
        {
            _popupNavigationService.Navigate(ViewType.StudentAccount);
        }

        private void OpenNotificationWindow()
        {
            _popupNavigationService.Navigate(ViewType.Notifications);
        }
    }
}