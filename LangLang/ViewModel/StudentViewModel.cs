using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using LangLang.DAO;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;
using Consts;
using System.Xml.Linq;

namespace LangLang.ViewModel
{
    internal class StudentViewModel : ViewModelBase
    {
        private readonly Window _window;
        private readonly CourseService _courseService = new();
        private readonly LanguageService _languageService = new();
        public ICommand ClearFiltersCommand { get; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }
        public ObservableCollection<int?> Durations { get; set; }


        private List<Exam> _exams;
        public List<Exam> Exams
        {
            get { return _exams; }
            set
            {
                _exams = value;
                OnPropertyChanged(nameof(Exams));
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
                    Start = selectedItem.Start;
                    Online = selectedItem.Online;
                }
                OnPropertyChanged();
            }
        }
        public StudentViewModel(Window window)
        {
            _window = window;
            Courses = new ObservableCollection<Course>();
            Languages = new ObservableCollection<string?>();
            Levels = new ObservableCollection<LanguageLvl>();
            Durations = new ObservableCollection<int?> { null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            Start = DateTime.Now.ToShortDateString();
            LoadExams();
            LoadLanguages();
            LoadCourses();
            LoadLanguageLevels();
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



        public void LoadCourses()
        {
            var courses = _courseService.GetAll();
            foreach (Course course in courses.Values)
            {
                Courses.Add(course);
            }

        }

        public void LoadExams()
        {
            ExamDAO examDAO = ExamDAO.GetInstance();
            var examsDictionary = examDAO.GetAllExams();
            Exams = new List<Exam>(examsDictionary.Values);
        }


        public void LoadLanguages()
        {
            var languages = _languageService.GetAll();
            foreach (Language language in languages.Values)
            {
                Languages.Add(language.Name);
            }
            Languages.Add("");
        }

        public void LoadLanguageLevels()
        {
            foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
            {
                Levels.Add(lvl);
            }
        }


        public void RemoveInputs()
        {
            Name = "";
            LanguageName = "";
            Level = LanguageLvl.A1;
            Duration = null;
            Online = false;
            selectedItem = null;
            Start = DateTime.Now.ToShortDateString();


        }
        public void FilterCourses()
        {
            Courses.Clear();
            var courses = _courseService.GetAll();
            foreach (Course course in courses.Values)
            {
                if ((course.Language.Name == LanguageFilter || LanguageFilter == "") && (course.Level == LevelFilter || LevelFilter == null))
                {
                    if (startFilter == null || (startFilter != null && course.Start == ((DateTime)startFilter).ToShortDateString()))
                    {
                        if (course.Online == OnlineFilter || OnlineFilter == null)
                        {
                            if (course.Duration == DurationFilter || DurationFilter == 0)
                            {
                                Courses.Add(course);
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

        private LanguageLvl? examLevelFilter;
        public LanguageLvl? ExamLevelFilter
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
            Exams.Clear();

            // Get all exams
            var exams = ExamDAO.GetInstance().GetAllExams().Values;

            // Filter exams based on criteria
            foreach (Exam exam in exams)
            {
                if ((exam.Language.Name == ExamLanguageFilter || ExamLanguageFilter == "") &&
                    (exam.LanguageLvl == ExamLevelFilter || ExamLevelFilter == null))
                {
                    if (ExamStartFilter == null || (ExamStartFilter != null && exam.Time.Date == ExamStartFilter.Value.Date))
                    {
                        Exams.Add(exam);
                    }
                }
            }
        }













    }
}
