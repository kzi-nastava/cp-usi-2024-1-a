using LangLang.Model;
using LangLang.MVVM;
using LangLang.Stores;
using System;

namespace LangLang.ViewModel
{
    public class UpcomingCourseInfoViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }
        private readonly CurrentCourseStore _currentCourseStore;
        public RelayCommand AcceptStudentCommand { get; }
        public RelayCommand DenyStudentCommand { get; }
        public RelayCommand GivePenaltyPointCommand { get; }
        public RelayCommand GradeStudentCommand { get; }

        private string courseName = "";
        public string CourseName
        {
            get => courseName;
            set
            {
                SetField(ref courseName, value);
            }
        }

        private Student? selectedStudent;
        public Student? SelectedStudent
        {
            get => selectedStudent;
            set
            {
                SetField(ref selectedStudent, value);
            }
        }
        //public ObservableCollection<Student> Students { get; set; }

        public UpcomingCourseInfoViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore)
        {
            NavigationStore = navigationStore;
            _currentCourseStore = currentCourseStore;
            CourseName = _currentCourseStore.CurrentCourse!.Name;
            AcceptStudentCommand = new RelayCommand(AcceptStudent, canExecute => SelectedStudent != null);
            DenyStudentCommand = new RelayCommand(DenyStudent, canExecute => SelectedStudent != null);
            GivePenaltyPointCommand = new RelayCommand(GivePenaltyPoint, canExecute => SelectedStudent != null);
            GradeStudentCommand = new RelayCommand(GradeStudent, canExecute => SelectedStudent != null);
        }

        private void GradeStudent(object? obj)
        {
            throw new NotImplementedException();
        }

        private void GivePenaltyPoint(object? obj)
        {
            throw new NotImplementedException();
        }

        private void DenyStudent(object? obj)
        {
            throw new NotImplementedException();
        }

        private void AcceptStudent(object? obj)
        {
            throw new NotImplementedException();
        }
    }
}
