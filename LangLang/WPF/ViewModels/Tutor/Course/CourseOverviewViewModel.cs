using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Course;
using LangLang.Application.Utility.Navigation;
using LangLang.Application.Utility.Timetable;
using LangLang.Domain.Model;
using LangLang.Domain;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;
using static LangLang.Domain.Model.Course;

namespace LangLang.WPF.ViewModels.Tutor.Course
{
    public class CourseOverviewViewModel : ViewModelBase
    {
        private readonly ICourseService _courseService;
        private readonly ITimetableService _timetableService;
        private readonly ICourseApplicationService _courseApplicationService;
        private readonly IPopupNavigationService _popupNavigationService;
        private readonly CurrentCourseStore _currentCourseStore;
        private readonly Domain.Model.Tutor _loggedInUser;

        public bool IsSelectTutorButtonVisible { get; }
        private bool _filterIsActive;
        
        public RelayCommand OpenCourseInfoCommand { get; }
        public RelayCommand AddCourseCommand { get; }
        public RelayCommand DeleteCourseCommand { get; }
        public RelayCommand UpdateCourseCommand { get; }
        public RelayCommand ToggleMaxStudentsCommand { get; }
        public RelayCommand ClearFiltersCommand { get; }
        public RelayCommand PreviousPageCommand { get; }
        public RelayCommand NextPageCommand { get; }
        public RelayCommand SelectedCourseChangedCommand { get; set; }
        private ObservableCollection<CourseViewModel> _courses = null!;
        public ObservableCollection<CourseViewModel> Courses { 
            get => _courses;
            private set => SetField(ref _courses, value);
        }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLevel> LanguageLevels { get; set; }
        public ObservableCollection<LanguageLevel> Levels { get; set; }
        public ObservableCollection<Domain.Model.Course.CourseState> States { get; set; }
        public ObservableCollection<int?> Durations { get; set; }
        public ObservableCollection<WorkDay?> WorkDays { get; set; }
        public ObservableCollection<TimeOnly?> mondayHours = new ObservableCollection<TimeOnly?>();
        public ObservableCollection<TimeOnly?> MondayHours
        {
            get => mondayHours;
            set => SetField(ref mondayHours, value);
        }
        public ObservableCollection<TimeOnly?> tuesdayHours = new ObservableCollection<TimeOnly?>();
        public ObservableCollection<TimeOnly?> TuesdayHours
        {
            get => tuesdayHours;
            set => SetField(ref tuesdayHours, value);
        }

        public ObservableCollection<TimeOnly?> wednesdayHours = new ObservableCollection<TimeOnly?>();
        public ObservableCollection<TimeOnly?> WednesdayHours
        {
            get => wednesdayHours;
            set => SetField(ref wednesdayHours, value);
        }

        public ObservableCollection<TimeOnly?> thursdayHours = new ObservableCollection<TimeOnly?>();
        public ObservableCollection<TimeOnly?> ThursdayHours
        {
            get => thursdayHours;
            set => SetField(ref thursdayHours, value);
        }

        public ObservableCollection<TimeOnly?> fridayHours = new ObservableCollection<TimeOnly?>();
        public ObservableCollection<TimeOnly?> FridayHours
        {
            get => fridayHours;
            set => SetField(ref fridayHours, value);
        }

        private TimeOnly? monday;
        public TimeOnly? Monday
        {
            get => monday;
            set => SetField(ref monday, value);
        }

        private TimeOnly? tuesday;
        public TimeOnly? Tuesday
        {
            get => tuesday;
            set => SetField(ref tuesday, value);
        }

        private TimeOnly? wednesday;
        public TimeOnly? Wednesday
        {
            get => wednesday;
            set => SetField(ref wednesday, value);
        }

        private TimeOnly? thursday;
        public TimeOnly? Thursday
        {
            get => thursday;
            set => SetField(ref thursday, value);
        }

        private TimeOnly? friday;
        public TimeOnly? Friday
        {
            get => friday;
            set => SetField(ref friday, value);
        }

        private string name = "";
        public string Name
        {
            get => name;
            set => SetField(ref name, value);
        }

        private string languageName = "";
        public string LanguageName
        {
            get { return languageName; }
            set
            {
                SetField(ref languageName, value);
                LoadLanguageLevels(languageName);
            }
        }

        private LanguageLevel level;
        public LanguageLevel Level
        {
            get =>  level;
            set
            {
                level = value;
                OnPropertyChanged();
            }
        }

        private int? duration;
        public int? Duration
        {
            get => duration; 
            set
            {
                SetField(ref duration, value);
                LoadHours();
            }
        }
        private int _coursesPerPage = 5;
        public int CoursesPerPage
        {
            get => _coursesPerPage;
            set
            {
                SetField(ref _coursesPerPage, value);
                if (PageNumber == 1)
                    LoadCourses();
                else
                    PageNumber = 1;
            }
        }

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            private set
            {
                SetField(ref _pageNumber, value);
                LoadCourses();
            }
        }

        // Schedule day list of selected days
        private ObservableCollection<WorkDay> scheduleDays = new ObservableCollection<WorkDay>();
        public ObservableCollection<WorkDay> ScheduleDays
        {
            get => scheduleDays;
            set
            {
                scheduleDays = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
        public Dictionary<WorkDay, Tuple<TimeOnly, int>> Schedule
        {
            get => schedule;
            set => SetField(ref schedule, value);
        }

        private DateTime? start;
        public DateTime? Start
        {
            get => start;
            set 
            {
                SetField(ref start, value);
                LoadHours();
            }
        }

        private bool online;
        public bool Online
        {
            get => online;
            set => SetField(ref online, value);
        }

        private bool isMaxStudentsDisabled;
        public bool IsMaxStudentsDisabled
        {
            get => isMaxStudentsDisabled;
            set => SetField(ref isMaxStudentsDisabled, value);
        }

        private int maxStudents;
        public int MaxStudents
        {
            get => maxStudents;
            set => SetField(ref maxStudents, value);
        }
        private int numStudents;
        public int NumStudents
        {
            get => numStudents;
            set => SetField(ref numStudents, value);
        }

        private Domain.Model.Course.CourseState state;
        public Domain.Model.Course.CourseState State
        {
            get => state;
            set => SetField(ref state, value);
        }
        // FILTER VALUES
        private string languageFilter = "";
        public string LanguageFilter
        {
            get => languageFilter; 
            set
            {
                SetField(ref languageFilter, value);
                FilterCourses();
            }
        }

        private LanguageLevel? levelFilter;
        public LanguageLevel? LevelFilter
        {
            get => levelFilter; 
            set
            {
                SetField(ref levelFilter, value);
                FilterCourses();
            }
        }

        private DateTime? startFilter;
        public DateTime? StartFilter
        {
            get => startFilter; 
            set
            {
                SetField(ref startFilter, value);
                FilterCourses();
            }
        }

        private bool? onlineFilter;
        public bool? OnlineFilter
        {
            get => onlineFilter; 
            set
            {
                SetField(ref onlineFilter, value);
                FilterCourses();
            }
        }

        private int durationFilter;
        public int DurationFilter
        {
            get => durationFilter;
            set
            {
                SetField(ref durationFilter, value);
                FilterCourses();
            }
        }

        private CourseViewModel? selectedItem;
        public CourseViewModel? SelectedItem
        {
            get => selectedItem;
            set {
                SetField(ref selectedItem, value);
                SelectCourse(value);
            }
        }
        public ObservableCollection<int> PageSizeOptions { get; }
        public CourseOverviewViewModel(IAuthenticationStore authenticationStore, ICourseService courseService, ITimetableService timetableService, IPopupNavigationService popupNavigationService, CurrentCourseStore currentCourseStore, ICourseApplicationService courseApplication)
        {
            _loggedInUser = (Domain.Model.Tutor?)authenticationStore.CurrentUser.Person ??
                           throw new InvalidOperationException(
                               "Cannot create CourseViewModel without currently logged in tutor");
            _currentCourseStore = currentCourseStore;
            _courseApplicationService = courseApplication;
            _courseService = courseService;
            _timetableService = timetableService;
            _popupNavigationService = popupNavigationService;
            _courseService.UpdateStates();
            Courses = new ObservableCollection<CourseViewModel>();
            Languages = new ObservableCollection<string?>();
            LanguageLevels = new ObservableCollection<LanguageLevel>();
            Levels = new ObservableCollection<LanguageLevel>();
            States = new ObservableCollection<Domain.Model.Course.CourseState>();
            Durations = new ObservableCollection<int?> {null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            WorkDays = new ObservableCollection<WorkDay?>();
            MondayHours = new ObservableCollection<TimeOnly?> {null, new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00) };
            TuesdayHours = new ObservableCollection<TimeOnly?>{null,new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            WednesdayHours = new ObservableCollection<TimeOnly?>{null,new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            ThursdayHours = new ObservableCollection<TimeOnly?>{null,new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            FridayHours = new ObservableCollection<TimeOnly?>{null,new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            Start = DateTime.Now;
            LoadLanguages();
            LoadCourses();
            LoadLanguageLevels();
            LoadCourseStates();
            LoadWorkDays();
            LoadHours();
            AddCourseCommand = new RelayCommand(SaveCourse, canExecute => true);
            DeleteCourseCommand = new RelayCommand(DeleteCourse, canExecute => SelectedItem != null);
            UpdateCourseCommand = new RelayCommand(UpdateCourse, canExecute => SelectedItem != null);
            ToggleMaxStudentsCommand = new RelayCommand(ToggleMaxStudents);
            ClearFiltersCommand = new RelayCommand(ClearFilters);
            SelectedCourseChangedCommand = new RelayCommand(SelectCourse);
            OpenCourseInfoCommand = new RelayCommand(OpenCourseInfo, canExecute => SelectedItem != null);
            IsSelectTutorButtonVisible = false;
            PageSizeOptions = new ObservableCollection<int>() { 1, 5, 10, 20 };
            PreviousPageCommand = new RelayCommand(_ => GoToPreviousPage(), _ => CanGoToPreviousPage());
            NextPageCommand = new RelayCommand(_ => GoToNextPage(), _ => CanGoToNextPage());
        }

        private void OpenCourseInfo(object? obj)
        {
            _currentCourseStore.CurrentCourse = _courseService.GetCourseById(SelectedItem!.Id);
            switch (SelectedItem?.State)
            {
                case CourseState.NotStarted:
                _popupNavigationService.Navigate(ViewType.UpcomingCourseInfo);
                    break;
                case CourseState.InProgress:
                _popupNavigationService.Navigate(ViewType.ActiveCourseInfo);
                    break;
                case CourseState.FinishedNotGraded:
                _popupNavigationService.Navigate(ViewType.FinishedCourseInfo);
                    break;
            }
        }

        private void SelectCourse(object? obj)
        {
            if (selectedItem != null)
            {
                Domain.Model.Course course = _courseService.GetCourseById(SelectedItem!.Id)!;
                Name = course.Name;
                LanguageName = course.Language.Name;
                Level = course.Level;
                Duration = course.Duration;
                Schedule = course.Schedule;
                ScheduleDays = new ObservableCollection<WorkDay>();
                foreach (WorkDay workday in Schedule.Keys)
                {
                    ScheduleDays.Add(workday);
                }
                if (Schedule.ContainsKey(WorkDay.Monday))
                {
                    Monday = Schedule[WorkDay.Monday].Item1;
                }
                if (Schedule.ContainsKey(WorkDay.Tuesday))
                {
                    Tuesday = Schedule[WorkDay.Tuesday].Item1;
                }
                if (Schedule.ContainsKey(WorkDay.Wednesday))
                {
                    Wednesday = Schedule[WorkDay.Wednesday].Item1;
                }
                if (Schedule.ContainsKey(WorkDay.Thursday))
                {
                    Thursday = Schedule[WorkDay.Thursday].Item1;
                }
                if (Schedule.ContainsKey(WorkDay.Friday))
                {
                    Friday = Schedule[WorkDay.Friday].Item1;
                }
                Start = course.Start;
                Online = course.Online;
                if (Online)
                {
                    MaxStudents = int.MaxValue;
                    IsMaxStudentsDisabled = true;
                }
                else
                {
                    MaxStudents = course.MaxStudents;
                    IsMaxStudentsDisabled = false;
                }
                NumStudents = course.NumStudents;
                State = course.State;

            }
        }

        private void ClearFilters(object? obj)
        {
            LanguageFilter = "";
            LevelFilter = null;
            StartFilter = null;
            OnlineFilter = null;
            DurationFilter = 0;
            _filterIsActive = false;
            Courses.Clear();
            LoadCourses();
            OnPropertyChanged();
        }

        private void ToggleMaxStudents(object? obj)
        {
            IsMaxStudentsDisabled = !IsMaxStudentsDisabled;
            if(IsMaxStudentsDisabled)
            {
                MaxStudents = int.MaxValue;
            }
            else
            {
                MaxStudents = 0;
            }
        }
        private void UpdateCourse(object? obj)
        {
            CreateSchedule();
            if(SelectedItem != null)
            {
                Domain.Model.Course? updatedCourse = _courseService.ValidateInputs(_loggedInUser, Name, LanguageName, Level, Duration, Schedule, ScheduleDays, Start, Online, NumStudents, State, MaxStudents);
                if(updatedCourse == null)
                {
                    MessageBox.Show("There was an error updating the course!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (SelectedItem != null)
                {
                    string keyToUpdate = SelectedItem.Id;
                    updatedCourse.Id = keyToUpdate;
                    Courses.Remove(SelectedItem);

                }
                _courseService.UpdateCourse(updatedCourse);
                Courses.Add(new CourseViewModel(updatedCourse));
                RemoveInputs();
            }
        }
        private void DeleteCourse(object? args)
        {
            if(SelectedItem != null)
            {
                string keyToDelete = SelectedItem.Id;
                _courseService.DeleteCourse(keyToDelete);
                _courseApplicationService.RemoveCourseApplications(keyToDelete);
                Courses.Remove(SelectedItem);
                RemoveInputs();
            }
        }
        private void SaveCourse(object? obj)
        {
            CreateSchedule();
            Domain.Model.Course? course = _courseService.ValidateInputs(_loggedInUser, Name, LanguageName, Level, Duration, Schedule, ScheduleDays, Start, Online, NumStudents, State, MaxStudents);

            if (course == null)
            {
                MessageBox.Show("There was an error saving the course!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            course.Id = "-1";
            _courseService.AddCourse(course);
            Courses.Add(new CourseViewModel(course));
            RemoveInputs();
            MessageBox.Show("The course is added successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private int GetClassroomNumber(TimeOnly? time)
        {
            if (Start != null && time != null)
            {
                var classrooms = _timetableService.GetAvailableClassrooms(DateOnly.FromDateTime((DateTime)Start), time.Value, Constants.ExamDuration, _loggedInUser);
                if (classrooms.Count > 0)
                {
                    return classrooms[0];
                }
            }

            return -1;
        }
        private void CreateSchedule()
        {
            Schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
            foreach(WorkDay workDay in ScheduleDays)
            {
                if(workDay == WorkDay.Monday && Monday != null)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>((TimeOnly)Monday, GetClassroomNumber(Monday));
                }else if(workDay == WorkDay.Tuesday && Tuesday != null)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>((TimeOnly)Tuesday, GetClassroomNumber(Tuesday));
                }
                else if (workDay == WorkDay.Wednesday && Wednesday != null)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>((TimeOnly)Wednesday, GetClassroomNumber(Wednesday));
                }
                else if (workDay == WorkDay.Thursday && Thursday != null)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>((TimeOnly)Thursday, GetClassroomNumber(Thursday));
                }
                else if (workDay == WorkDay.Friday && Friday != null)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>((TimeOnly)Friday, GetClassroomNumber(Friday));
                }
            }
        }
        public void LoadCourses()
        {
            if (_filterIsActive)
            {
                int? duration = DurationFilter == 0 ? null : DurationFilter;
                string? language = LanguageFilter == "" ? null : LanguageFilter;
                Courses = new ObservableCollection<CourseViewModel>(ConvertToCourseViewModel(_courseService.FilterCoursesForPage(
                    PageNumber, CoursesPerPage, language, LevelFilter, StartFilter, OnlineFilter, duration
                    )));
            }
            else
            {
                Courses = new ObservableCollection<CourseViewModel>(ConvertToCourseViewModel(
                    _courseService.GetCoursesByTutorForPage(_loggedInUser.Id, PageNumber, CoursesPerPage)));
            }

        }
        public void LoadLanguages()
        {
            foreach(Tuple<Language,LanguageLevel> languageTuple in _loggedInUser.KnownLanguages){
                Languages.Add(languageTuple.Item1.Name);
            }
            Languages.Add("");

        }
        public void LoadLanguageLevels(string language = "")
        {
            if(language == "")
            {
                Levels.Clear();
                foreach (LanguageLevel lvl in Enum.GetValues(typeof(LanguageLevel)))
                {
                    Levels.Add(lvl);
                }
            }
            else
            {
                LanguageLevels.Clear();
                foreach (Tuple<Language, LanguageLevel> languageTuple in _loggedInUser.KnownLanguages)
                {
                    if(languageTuple.Item1.Name == language)
                    {
                        foreach (LanguageLevel lvl in Enum.GetValues(typeof(LanguageLevel)))
                        {
                            if (lvl > languageTuple.Item2) break;
                            LanguageLevels.Add(lvl);
                        }
                    }
                }
            }
        }
        public void LoadCourseStates()
        {
            foreach (Domain.Model.Course.CourseState state in Enum.GetValues(typeof(Domain.Model.Course.CourseState)))
            {
                States.Add(state);
            }
        }
        public void LoadWorkDays()
        {
            foreach (WorkDay day in Enum.GetValues(typeof(WorkDay)))
            {
                WorkDays.Add(day);
            }
        }
        public void LoadHours()
        {
            if (Duration == null) return;
            if(start == null)
            {
                return;
            }
            var availableLessonTimes = _timetableService.GetAvailableLessonTimes(DateOnly.FromDateTime(start.Value), Duration.Value, _loggedInUser);
            MondayHours = new ObservableCollection<TimeOnly?>(availableLessonTimes[WorkDay.Monday].Select(t => (TimeOnly?)t));
            MondayHours.Insert(0, null);
            TuesdayHours = new ObservableCollection<TimeOnly?>(availableLessonTimes[WorkDay.Tuesday].Select(t => (TimeOnly?)t));
            TuesdayHours.Insert(0, null);
            WednesdayHours = new ObservableCollection<TimeOnly?>(availableLessonTimes[WorkDay.Wednesday].Select(t => (TimeOnly?)t));
            WednesdayHours.Insert(0, null);
            ThursdayHours = new ObservableCollection<TimeOnly?>(availableLessonTimes[WorkDay.Thursday].Select(t => (TimeOnly?)t));
            ThursdayHours.Insert(0, null);
            FridayHours = new ObservableCollection<TimeOnly?>(availableLessonTimes[WorkDay.Friday].Select(t => (TimeOnly?)t));
            FridayHours.Insert(0, null);
        }
        public void RemoveInputs()
        {
            Name = "";
            LanguageName = "";
            Level = LanguageLevel.A1;
            Duration = null;
            ScheduleDays = new ObservableCollection<WorkDay>();
            Online = false;
            selectedItem = null;
            MaxStudents = 0;
            NumStudents = 0;
            State = Domain.Model.Course.CourseState.InProgress;
            Start = DateTime.Now;


        }
        public void FilterCourses()
        {
            _filterIsActive = true;
            LoadCourses();
        }

        private List<CourseViewModel> ConvertToCourseViewModel(List<Domain.Model.Course> courses)
        {
            return courses.Select(exam => new CourseViewModel(exam)).ToList();
        }

        private void GoToPreviousPage()
        {
            PageNumber--;
        }

        private bool CanGoToPreviousPage()
        {
            return PageNumber > 1;
        }

        private void GoToNextPage()
        {
            PageNumber++;
        }

        private bool CanGoToNextPage()
        {
            return Courses.Count == CoursesPerPage  ;
        }

    }
}
