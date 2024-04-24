using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.EntityServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.ViewModel.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LangLang.ViewModel
{
    public class CourseViewModel : ViewModelBase
    {
        private readonly ICourseService _courseService;
        private readonly ITimetableService _timetableService;
        private readonly INavigationService _navigationService;
        private readonly IPopupNavigationService _popupNavigationService;
        private readonly Tutor _loggedInUser;
        public RelayCommand OpenCourseInfoCommand { get; }
        public RelayCommand AddCourseCommand { get; }
        public RelayCommand DeleteCourseCommand { get; }
        public RelayCommand UpdateCourseCommand { get; }
        public RelayCommand ToggleMaxStudentsCommand { get; }
        public RelayCommand ClearFiltersCommand { get; }
        public RelayCommand SelectedCourseChangedCommand { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLvl> LanguageLevels { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }
        public ObservableCollection<CourseState> States { get; set; }
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

        private LanguageLvl level;
        public LanguageLvl Level
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
        // Schedule day list of selected days
        private ObservableCollection<WorkDay> scheduleDays = new ObservableCollection<WorkDay>();
        public ObservableCollection<WorkDay> ScheduleDays
        {
            get => scheduleDays;
            set => SetField(ref scheduleDays, value);
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

        private CourseState state;
        public CourseState State
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

        private LanguageLvl? levelFilter;
        public LanguageLvl? LevelFilter
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

        private Course? selectedItem;
        public Course? SelectedItem
        {
            get => selectedItem;
            set {
                SetField(ref selectedItem, value);
                SelectCourse(value);
            }
        }
        public CourseViewModel(AuthenticationStore authenticationStore, ICourseService courseService, ITimetableService timetableService, INavigationService navigationService, IPopupNavigationService popupNavigationService)
        {
            _loggedInUser = (Tutor?)authenticationStore.CurrentUser ??
                           throw new InvalidOperationException(
                               "Cannot create CourseViewModel without currently logged in tutor");
            _courseService = courseService;
            _navigationService = navigationService;
            _timetableService = timetableService;
            _popupNavigationService = popupNavigationService;
            Courses = new ObservableCollection<Course>();
            Languages = new ObservableCollection<string?>();
            LanguageLevels = new ObservableCollection<LanguageLvl>();
            Levels = new ObservableCollection<LanguageLvl>();
            States = new ObservableCollection<CourseState>();
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
        }

        private void OpenCourseInfo(object? obj)
        {
            _popupNavigationService.Navigate(ViewType.CourseInfo);
        }

        private void SelectCourse(object? obj)
        {
            if (selectedItem != null)
            {

                Name = selectedItem.Name;
                LanguageName = selectedItem.Language.Name;
                Level = selectedItem.Level;
                Duration = selectedItem.Duration;
                Schedule = selectedItem.Schedule;
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
                Start = selectedItem.Start;
                Online = selectedItem.Online;
                if (Online)
                {
                    MaxStudents = int.MaxValue;
                    IsMaxStudentsDisabled = true;
                }
                else
                {
                    MaxStudents = selectedItem.MaxStudents;
                    IsMaxStudentsDisabled = false;
                }
                NumStudents = selectedItem.NumStudents;
                State = selectedItem.State;

            }
        }

        private void ClearFilters(object? obj)
        {
            LanguageFilter = "";
            LevelFilter = null;
            StartFilter = null;
            OnlineFilter = null;
            DurationFilter = 0;
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
                Course? updatedCourse = _courseService.ValidateInputs(Name, LanguageName, Level, Duration, Schedule, ScheduleDays, Start, Online, NumStudents, State, MaxStudents);
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
                Courses.Add(updatedCourse);
                RemoveInputs();
            }
        }
        private void DeleteCourse(object? args)
        {
            if(SelectedItem != null)
            {
                string keyToDelete = SelectedItem.Id;
                _courseService.DeleteCourse(keyToDelete, _loggedInUser);
                Courses.Remove(SelectedItem);
                RemoveInputs();
            }
        }
        private void SaveCourse(object? obj)
        {
            CreateSchedule();
            Course? course = _courseService.ValidateInputs(Name, LanguageName, Level, Duration, Schedule, ScheduleDays, Start, Online, NumStudents, State, MaxStudents);

            if (course == null)
            {
                MessageBox.Show("There was an error saving the course!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _courseService.AddCourse(course, _loggedInUser);
            Courses.Add(course);
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
            var courses = _courseService.GetCoursesByTutor(_loggedInUser);
            foreach(Course course in courses.Values){
                Courses.Add(course);
            }

        }
        public void LoadLanguages()
        {
            foreach(Tuple<Language,LanguageLvl> languageTuple in _loggedInUser.KnownLanguages){
                Languages.Add(languageTuple.Item1.Name);
            }
            Languages.Add("");

        }
        public void LoadLanguageLevels(string language = "")
        {
            if(language == "")
            {
                Levels.Clear();
                foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
                {
                    Levels.Add(lvl);
                }
            }
            else
            {
                LanguageLevels.Clear();
                foreach (Tuple<Language, LanguageLvl> languageTuple in _loggedInUser.KnownLanguages)
                {
                    if(languageTuple.Item1.Name == language)
                    {
                        foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
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
            foreach (CourseState state in Enum.GetValues(typeof(CourseState)))
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
            Level = LanguageLvl.A1;
            Duration = null;
            ScheduleDays = new ObservableCollection<WorkDay>();
            Online = false;
            selectedItem = null;
            MaxStudents = 0;
            NumStudents = 0;
            State = CourseState.Active;
            Start = DateTime.Now;


        }
        public void FilterCourses()
        {
            Courses.Clear();
            var courses = _courseService.GetCoursesByTutor(_loggedInUser);
            foreach (Course course in courses.Values)
            {
                if ((course.Language.Name == LanguageFilter || LanguageFilter == "") && (course.Level == LevelFilter || LevelFilter == null))
                {
                    if(startFilter == null || (startFilter != null && course.Start == ((DateTime)startFilter)))
                    {
                        if(course.Online == OnlineFilter || OnlineFilter == null)
                        { 
                            if(course.Duration == DurationFilter || DurationFilter == 0)
                            {
                                Courses.Add(course);
                            }
                        }
                    }
                }
            }
        }
    }
}
