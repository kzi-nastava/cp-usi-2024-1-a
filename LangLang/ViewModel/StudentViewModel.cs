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
        private readonly StudentService studentService = StudentService.GetInstance();
        private readonly ExamService examService = new();
        public ICommand ClearExamFiltersCommand { get; }
        public ICommand ClearCourseFiltersCommand { get; }
        public ObservableCollection<Course> Courses { get; set; }

        public ObservableCollection<Exam> Exams { get; set; }

        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }
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

        private LanguageLvl? courseLevelFilter;
        public LanguageLvl? CourseLevelFilter
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
        public StudentViewModel(Window window)
        {
            _window = window;
            Courses = new ObservableCollection<Course>();
            Exams = new ObservableCollection<Exam>();
            Languages = new ObservableCollection<string?>();
            Levels = new ObservableCollection<LanguageLvl>();
            Durations = new ObservableCollection<int?> { null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            Start = DateTime.Now.ToShortDateString();
            LoadExams();
            LoadLanguages();
            LoadCourses();
            LoadLanguageLevels();
            ClearCourseFiltersCommand = new RelayCommand(ClearCourseFilters);
            ClearExamFiltersCommand = new RelayCommand(ClearExamFilters);

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
            Exams.Clear();
            LoadExams();
            OnPropertyChanged();
        }


        public void LoadCourses()
        {
            var courses = _courseService.GetAvailableCourses(studentService.LoggedUser);
            foreach (Course course in courses)
            {
                Courses.Add(course);
            }

        }

        public void LoadExams()
        {
            ExamDAO examDAO = ExamDAO.GetInstance();
            var examsDictionary = examService.GetAvailableExamsForStudent(studentService.LoggedUser);
            foreach (Exam exam in examsDictionary)
            {
                Exams.Add(exam);
            }
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
                if ((course.Language.Name == CourseLanguageFilter || CourseLanguageFilter == "") && (course.Level == CourseLevelFilter || CourseLevelFilter == null))
                {
                    if (CourseStartFilter == null || (CourseStartFilter != null && course.Start == ((DateTime)CourseStartFilter).ToShortDateString()))
                    {
                        if (course.Online == CourseOnlineFilter || CourseOnlineFilter == null)
                        {
                            if (course.Duration == CourseDurationFilter || CourseDurationFilter == 0)
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
