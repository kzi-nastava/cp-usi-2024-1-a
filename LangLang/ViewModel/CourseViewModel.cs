using Consts;
using LangLang.MVVM;
using LangLang.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LangLang.Model;

namespace LangLang.ViewModel
{
    internal class CourseViewModel
    {
        private readonly Window _window;
        private readonly CourseService _courseService = new CourseService();
        private readonly LanguageService _languageService = new LanguageService();
        public ICommand AddCourseCommand { get; }
        public ICommand LoadLanguagesCommand { get; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }

        public ObservableCollection<CourseState> States { get; set; }

        public CourseViewModel(Window window)
        {
            _window = window;
            Courses = new ObservableCollection<Course>();
            Languages = new ObservableCollection<string>();
            Levels = new ObservableCollection<LanguageLvl>();
            States = new ObservableCollection<CourseState>();
            LoadLanguages();
            LoadCourses();
            LoadLanguageLevels();
            LoadCourseStates();
            AddCourseCommand = new RelayModel(saveCourse, canSaceCourse);
        }

        private bool canSaceCourse(object arg)
        {
            return true;
        }

        private void saveCourse(object obj)
        {
            //Course course = new Course();
            //_courseService.AddCourse(course);
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
    }
}
