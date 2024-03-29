using Consts;
using LangLang.Model;
using LangLang.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    internal class CourseViewModel : ObservableObject
    {
        private readonly Window _window;
        private readonly CourseService _courseService = new();
        private readonly LanguageService _languageService = new();
        public ICommand AddCourseCommand { get; }
        public ICommand DeleteCourseCommand { get; }
        public ICommand UpdateCourseCommand { get; }
        //public ICommand LoadLanguagesCommand { get; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }
        public ObservableCollection<CourseState> States { get; set; }
        public ObservableCollection<int> Durations { get; set; }
        public ObservableCollection<WorkDay> WorkDays { get; set; }

        public ObservableCollection<TimeOnly> MondayHours { get; set; }
        public ObservableCollection<TimeOnly> TuesdayHours { get; set; }
        public ObservableCollection<TimeOnly> WednesdayHours { get; set; }
        public ObservableCollection<TimeOnly> ThursdayHours { get; set; }
        public ObservableCollection<TimeOnly> FridayHours { get; set; }

        private TimeOnly monday;
        public TimeOnly Monday
        {
            get { return monday; }
            set
            {
                monday = value;
                OnProperyChanged();
            }

        }

        private TimeOnly tuesday;
        public TimeOnly Tuesday
        {
            get { return tuesday; }
            set
            {
                tuesday = value;
                OnProperyChanged();
            }
        }

        private TimeOnly wednesday;
        public TimeOnly Wednesday
        {
            get { return wednesday; }
            set
            {
                wednesday = value;
                OnProperyChanged();
            }
        }

        private TimeOnly thursday;
        public TimeOnly Thursday
        {
            get { return thursday; }
            set
            {
                thursday = value;
                OnProperyChanged();
            }
        }

        private TimeOnly friday;
        public TimeOnly Friday
        {
            get { return friday; }
            set
            {
                friday = value;
                OnProperyChanged();
            }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnProperyChanged();
            }
        }

        private string languageName = "";
        public string LanguageName
        {
            get { return languageName; }
            set
            {
                languageName = value;
                OnProperyChanged();
            }
        }

        private LanguageLvl level;
        public LanguageLvl Level
        {
            get { return level; }
            set
            {
                level = value;
                OnProperyChanged();
            }
        }

        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnProperyChanged();
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
                OnProperyChanged();
            }
        }


        private Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
        public Dictionary<WorkDay, Tuple<TimeOnly, int>> Schedule
        {
            get { return schedule; }
            set
            {
                schedule = value;
                OnProperyChanged();
            }
        }

        private string start = "";
        public string Start
        {
            get { return start; }
            set
            {
                start = value;
                OnProperyChanged();
            }
        }

        private bool online;
        public bool Online
        {
            get { return online; }
            set
            {
                online = value;
                OnProperyChanged();
            }
        }

        private int maxStudents;
        public int MaxStudents
        {
            get { return maxStudents; }
            set
            {
                maxStudents = value;
                OnProperyChanged();
            }
        }
        private int numStudents;
        public int NumStudents
        {
            get { return numStudents; }
            set
            {
                numStudents = value;
                OnProperyChanged();
            }
        }

        private CourseState state;
        public CourseState State
        {
            get { return state; }
            set
            {
                state = value;
                OnProperyChanged();
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
                    MaxStudents = selectedItem.MaxStudents;
                    NumStudents = selectedItem.NumStudents;
                    State = selectedItem.State;

                }
                OnProperyChanged();
                // Notify the command that the state might have changed
                //DeleteCourseCommand.R
            }
        }
        public CourseViewModel(Window window)
        {
            _window = window;
            Courses = new ObservableCollection<Course>();
            Languages = new ObservableCollection<string>();
            Levels = new ObservableCollection<LanguageLvl>();
            States = new ObservableCollection<CourseState>();
            Durations = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            WorkDays = new ObservableCollection<WorkDay>();
            MondayHours = new ObservableCollection<TimeOnly> { new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00) };
            TuesdayHours = new ObservableCollection<TimeOnly>{new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            WednesdayHours = new ObservableCollection<TimeOnly>{new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            ThursdayHours = new ObservableCollection<TimeOnly>{new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            FridayHours = new ObservableCollection<TimeOnly>{new TimeOnly(10, 30, 00), new TimeOnly(12, 30, 00)};
            Start = DateTime.Now.ToShortDateString();
            LoadLanguages();
            LoadCourses();
            LoadLanguageLevels();
            LoadCourseStates();
            LoadWorkDays();
            LoadHours();
            AddCourseCommand = new RelayModel(SaveCourse, CanSaveCourse);
            // TODO: set can execute to work only when item is selected!
            DeleteCourseCommand = new RelayModel(DeleteCourse, CanDeleteCourse);
            UpdateCourseCommand = new RelayModel(UpdateCourse, CanUpdateCourse);
        }

        private bool CanUpdateCourse(object arg)
        {
            return true;
        }
        private void UpdateCourse(object obj)
        {
            CreateSchedule();
            if(SelectedItem != null)
            {
                string keyToUpdate = SelectedItem.Id;
                // TODO: Implement this method
                Course updatedCourse = new Course(
                        keyToUpdate,
                        Name,
                        _languageService.GetLanguageById(LanguageName),
                        level,
                        Duration,
                        Schedule,
                        DateTime.Parse(Start),
                        Online,
                        NumStudents,
                        State,
                        MaxStudents
                );
                _courseService.UpdateCourse(updatedCourse);
                Courses.Remove(SelectedItem);
                Courses.Add(updatedCourse);
            }
        }
        private void DeleteCourse(object args)
        {
            if(SelectedItem != null)
            {
                string keyToDelete = SelectedItem.Id;
                _courseService.DeleteCourse(keyToDelete);
                Courses.Remove(SelectedItem);
            }
        }
        private bool CanDeleteCourse(object args)
        {
            return true;
        }
        private bool CanSaveCourse(object arg)
        {
            return true;
        }
        private void SaveCourse(object obj)
        {
            //Course course = new Course();
            //_courseService.AddCourse(course);
            CreateSchedule();
            Course course = new Course(
                    "0",
                    Name,
                    _languageService.GetLanguageById(LanguageName),
                    level,
                    Duration,
                    Schedule,
                    DateTime.Parse(Start),
                    Online,
                    NumStudents,
                    State,
                    MaxStudents
                );
            _courseService.AddCourse(course);
            Courses.Add(course);
            
        }

        private void CreateSchedule()
        {
            Schedule = new Dictionary<WorkDay, Tuple<TimeOnly, int>>();
            foreach(WorkDay workDay in ScheduleDays)
            {
                // TODO: get the free classroom for now its set to the first classroom
                Schedule[workDay] = new Tuple<TimeOnly, int>(Monday, 1);
            }
        }
        public void LoadCourses()
        {
            var courses = _courseService.GetAll();
            foreach(Course course in courses.Values){
                Courses.Add(course);
            }

        }
        public void LoadLanguages()
        {
            var languages = _languageService.GetAll();
            foreach(Language language in languages.Values)
            {
                Languages.Add(language.Name);
            }

        }

        public void LoadLanguageLevels()
        {
            foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
            {
                Levels.Add(lvl);
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
            // TODO: load free hours
            return;
        }
    }
}
