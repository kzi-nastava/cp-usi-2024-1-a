using LangLang.DAO;
using LangLang.DAO.JsonDao;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LangLang.ViewModel
{
    public class ActiveCourseInfoViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }
        private readonly CurrentCourseStore _currentCourseStore;
        private readonly IStudentDAO _studentDAO;
        public RelayCommand AcceptStudentCommand { get; }
        public RelayCommand DenyStudentCommand { get; }
        public RelayCommand GivePenaltyPointCommand { get; }

        private string courseName = "";
        private string name = "";
        private string surname = "";
        private string email = "";
        private uint penaltyPts;
        private string sender = "";
        private string dropMessage = "";
        public string Name
        {
            get => name;
            set => SetField(ref name, value);
        }
        public string Surname
        {
            get => surname;
            set => SetField(ref surname, value);
        }
        public string Email
        {
            get => email;
            set => SetField(ref email, value);
        }
        public uint PenaltyPts
        {
            get => penaltyPts;
            set => SetField(ref penaltyPts, value);
        }
        public string CourseName
        {
            get => courseName;
            set
            {
                SetField(ref courseName, value);
            }
        }
        public string Sender
        {
            get => sender;
            set
            {
                SetField(ref sender, value);
            }
        }
        public string DropMessage
        {
            get => dropMessage;
            set
            {
                SetField(ref dropMessage, value);
            }
        }
        private string? selectedDropRequest;
        public string? SelectedDropRequest
        {
            get => selectedDropRequest;
            set
            {
                SetField(ref selectedDropRequest, value);
                SelectDropRequest();
            }
        }
        private Student? selectedStudent;
        public Student? SelectedStudent
        {
            get => selectedStudent;
            set
            {
                SetField(ref selectedStudent, value);
                SelectStudent();
            }
        }

        public ObservableCollection<Student> Students { get; set; }

        public ActiveCourseInfoViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, IStudentDAO studentDAO)
        {
            NavigationStore = navigationStore;
            _currentCourseStore = currentCourseStore;
            _studentDAO = studentDAO;
            Students = new ObservableCollection<Student>(LoadStudents());
            CourseName = _currentCourseStore.CurrentCourse!.Name;
            AcceptStudentCommand = new RelayCommand(AcceptStudent, canExecute => SelectedDropRequest != null);
            DenyStudentCommand = new RelayCommand(DenyStudent, canExecute => SelectedDropRequest != null);
            GivePenaltyPointCommand = new RelayCommand(GivePenaltyPoint, canExecute => SelectedStudent != null);
        }

        private List<Student> LoadStudents()
        {
            List<Student> students = new List<Student>();
            foreach (Student student in _studentDAO.GetAllStudents().Values)
            {
                students.Add(student);
            }
            return students;
        }
        private void SelectStudent()
        {
            if (SelectedStudent == null) return;
            Name = SelectedStudent.Name;
            Surname = SelectedStudent.Surname;
            Email = SelectedStudent.Email;
            PenaltyPts = SelectedStudent.PenaltyPts;

        }
        private void SelectDropRequest()
        {
            if (SelectDropRequest == null) return;
            Sender = "proba";
            DropMessage = "molim te nemoj da mi das penal";

        }
        private void GradeStudent(object? obj)
        {
            throw new NotImplementedException();
        }

        private void GivePenaltyPoint(object? obj)
        {
            MessageBox.Show("Penalty point added.");
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
