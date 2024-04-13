using Consts;
using LangLang.DAO;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    internal class CourseViewModel : ViewModelBase
    {
        private readonly CourseService courseService;
        private readonly LanguageService languageService;
        private readonly TimetableService timetableService;
        private Tutor loggedInUser;
        public ICommand AddCourseCommand { get; }
        public ICommand DeleteCourseCommand { get; }
        public ICommand UpdateCourseCommand { get; }
        public ICommand ToggleMaxStudentsCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLvl> LanguageLevels { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }
        public ObservableCollection<CourseState> States { get; set; }
        public ObservableCollection<int?> Durations { get; set; }
        public ObservableCollection<WorkDay?> WorkDays { get; set; }
        public ObservableCollection<TimeOnly?> mondayHours;
        public ObservableCollection<TimeOnly?> MondayHours
        {
            get { return mondayHours; }
            set
            {
                mondayHours = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TimeOnly?> tuesdayHours;
        public ObservableCollection<TimeOnly?> TuesdayHours
        {
            get { return tuesdayHours; }
            set
            {
                tuesdayHours = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TimeOnly?> wednesdayHours;
        public ObservableCollection<TimeOnly?> WednesdayHours
        {
            get { return wednesdayHours; }
            set
            {
                wednesdayHours = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TimeOnly?> thursdayHours;
        public ObservableCollection<TimeOnly?> ThursdayHours
        {
            get { return thursdayHours; }
            set
            {
                thursdayHours = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TimeOnly?> fridayHours;
        public ObservableCollection<TimeOnly?> FridayHours
        {
            get { return fridayHours; }
            set
            {
                fridayHours = value;
                OnPropertyChanged();
            }
        }

        private TimeOnly monday;
        public TimeOnly Monday
        {
            get { return monday; }
            set
            {
                monday = value;
                OnPropertyChanged();
            }

        }

        private TimeOnly tuesday;
        public TimeOnly Tuesday
        {
            get { return tuesday; }
            set
            {
                tuesday = value;
                OnPropertyChanged();
            }
        }

        private TimeOnly wednesday;
        public TimeOnly Wednesday
        {
            get { return wednesday; }
            set
            {
                wednesday = value;
                OnPropertyChanged();
            }
        }

        private TimeOnly thursday;
        public TimeOnly Thursday
        {
            get { return thursday; }
            set
            {
                thursday = value;
                OnPropertyChanged();
            }
        }

        private TimeOnly friday;
        public TimeOnly Friday
        {
            get { return friday; }
            set
            {
                friday = value;
                OnPropertyChanged();
            }
        }

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
                LoadLanguageLevels(languageName);
                OnPropertyChanged();
            }
        }

        private LanguageLvl level;
        public LanguageLvl Level
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
                LoadHours();
            }
        }
        // Schedule day list of selected days
        private ObservableCollection<WorkDay> scheduleDays = new ObservableCollection<WorkDay>();
        public ObservableCollection<WorkDay> ScheduleDays
        {
            get { return scheduleDays; }
            set
            {
                scheduleDays = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
        public Dictionary<WorkDay, Tuple<TimeOnly, int>> Schedule
        {
            get { return schedule; }
            set
            {
                schedule = value;
                OnPropertyChanged();
            }
        }

        private string start = "";
        public string Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged();
                LoadHours();
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

        private bool isMaxStudentsDisabled;
        public bool IsMaxStudentsDisabled
        {
            get { return isMaxStudentsDisabled; }
            set
            {
                if(isMaxStudentsDisabled != value)
                {
                    isMaxStudentsDisabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private int maxStudents;
        public int MaxStudents
        {
            get { return maxStudents; }
            set
            {
                maxStudents = value;
                OnPropertyChanged();
            }
        }
        private int numStudents;
        public int NumStudents
        {
            get { return numStudents; }
            set
            {
                numStudents = value;
                OnPropertyChanged();
            }
        }

        private CourseState state;
        public CourseState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }
        // FILTER VALUES
        private string languageFilter = "";
        public string LanguageFilter
        {
            get { return languageFilter; }
            set
            {
                languageFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private LanguageLvl? levelFilter;
        public LanguageLvl? LevelFilter
        {
            get { return levelFilter; }
            set
            {
                levelFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private DateTime? startFilter;
        public DateTime? StartFilter
        {
            get { return startFilter; }
            set
            {
                startFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private bool? onlineFilter;
        public bool? OnlineFilter
        {
            get { return onlineFilter; }
            set
            {
                onlineFilter = value;
                FilterCourses();
                OnPropertyChanged();
            }
        }

        private int durationFilter;
        public int DurationFilter
        {
            get { return durationFilter; }
            set
            {
                durationFilter = value;
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
                    Schedule = selectedItem.Schedule;
                    ScheduleDays = new ObservableCollection<WorkDay>();
                    foreach(WorkDay workday in Schedule.Keys)
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
                OnPropertyChanged();
            }
        }
        public CourseViewModel(Tutor loggedInUser,CourseService courseService, LanguageService languageService, TimetableService timetableService)
        {
            this.courseService = courseService;
            this.languageService = languageService;
            this.loggedInUser = loggedInUser;
            this.timetableService = timetableService;
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
            Start = DateTime.Now.ToShortDateString();
            LoadLanguages();
            LoadCourses();
            LoadLanguageLevels();
            LoadCourseStates();
            LoadWorkDays();
            LoadHours();
            AddCourseCommand = new RelayCommand(SaveCourse, CanSaveCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse, CanDeleteCourse);
            UpdateCourseCommand = new RelayCommand(UpdateCourse, CanUpdateCourse);
            ToggleMaxStudentsCommand = new RelayCommand(ToggleMaxStudents);
            ClearFiltersCommand = new RelayCommand(ClearFilters);
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
        private bool CanUpdateCourse(object? arg)
        {
            return SelectedItem != null;
        }
        private void UpdateCourse(object? obj)
        {
            CreateSchedule();
            if(SelectedItem != null)
            {
                Course? updatedCourse = courseService.ValidateInputs(Name, LanguageName, Level, Duration, Schedule, ScheduleDays, Start, Online, NumStudents, State, MaxStudents);
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
                courseService.UpdateCourse(updatedCourse);
                Courses.Add(updatedCourse);
                RemoveInputs();
            }
        }
        private void DeleteCourse(object? args)
        {
            if(SelectedItem != null)
            {
                string keyToDelete = SelectedItem.Id;
                courseService.DeleteCourse(keyToDelete, loggedInUser);
                Courses.Remove(SelectedItem);
                RemoveInputs();
            }
        }
        private bool CanDeleteCourse(object? args)
        {
            return SelectedItem != null;
        }
        private bool CanSaveCourse(object? arg)
        {
            return true;
        }
        private void SaveCourse(object? obj)
        {
            CreateSchedule();
            Course? course = courseService.ValidateInputs(Name, LanguageName, Level, Duration, Schedule, ScheduleDays, Start, Online, NumStudents, State, MaxStudents);

            if (course == null)
            {
                MessageBox.Show("There was an error saving the course!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            courseService.AddCourse(course, loggedInUser);
            Courses.Add(course);
            RemoveInputs();
            MessageBox.Show("The course is added successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private int GetClassroomNumber(TimeOnly? time)
        {
            if (Start != null && time != null)
            {
                var classrooms = timetableService.GetAvailableClassrooms(DateOnly.FromDateTime(DateTime.Parse(Start)), time.Value, Constants.ExamDuration, loggedInUser);
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
                // TODO: get the free classroom for now its set to the first classroom
                if(workDay == WorkDay.Monday)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>(Monday, GetClassroomNumber(Monday));
                }else if(workDay == WorkDay.Tuesday)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>(Tuesday, GetClassroomNumber(Tuesday));
                }
                else if (workDay == WorkDay.Wednesday)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>(Wednesday, GetClassroomNumber(Wednesday));
                }
                else if (workDay == WorkDay.Thursday)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>(Thursday, GetClassroomNumber(Thursday));
                }
                else if (workDay == WorkDay.Friday)
                {
                    Schedule[workDay] = new Tuple<TimeOnly, int>(Friday, GetClassroomNumber(Friday));
                }
            }
        }
        public void LoadCourses()
        {
            var courses = courseService.GetCoursesByTutor(loggedInUser);
            foreach(Course course in courses.Values){
                Courses.Add(course);
            }

        }
        public void LoadLanguages()
        {
            foreach(Tuple<Language,LanguageLvl> languageTuple in loggedInUser.KnownLanguages){
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
                foreach (Tuple<Language, LanguageLvl> languageTuple in loggedInUser.KnownLanguages)
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
            var availableLessonTimes = timetableService.GetAvailableLessonTimes(DateOnly.FromDateTime(DateTime.Parse(start)), Duration.Value, loggedInUser);
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
            Start = DateTime.Now.ToShortDateString();


        }
        public void FilterCourses()
        {
            Courses.Clear();
            var courses = courseService.GetCoursesByTutor(loggedInUser);
            foreach (Course course in courses.Values)
            {
                if ((course.Language.Name == LanguageFilter || LanguageFilter == "") && (course.Level == LevelFilter || LevelFilter == null))
                {
                    if(startFilter == null || (startFilter != null && course.Start == ((DateTime)startFilter).ToShortDateString()))
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
