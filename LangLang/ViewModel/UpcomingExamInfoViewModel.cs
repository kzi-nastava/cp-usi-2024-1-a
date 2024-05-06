using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.ExamServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.NotificationServices;
using LangLang.Stores;
using LangLang.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace LangLang.ViewModel
{
    public class UpcomingExamInfoViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }

        private readonly CurrentExamStore _currentExamStore;

        private readonly string AcceptMessage = "Application accepted!";

        private readonly IUserProfileMapper _userProfileMapper;

        private readonly IExamCoordinator _examCoordinator;

        private readonly IStudentDAO _studentDAO;
        private readonly IClosePopupNavigationService _closepopupNavigationService;
        public RelayCommand AddStudentCommand { get; }
        public RelayCommand RemoveStudentCommand { get; }
        public RelayCommand ConfirmListCommand { get; }

        private string name = "";
        private string surname = "";
        private string email = "";
        private string message = "";
        private uint penaltyPts;
        private ObservableCollection<Student> students = new ObservableCollection<Student>();
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
        public string Message
        {
            get => message;
            set
            {
                SetField(ref message, value);
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

        public ObservableCollection<Student> Students 
        {
            get => students;
            set
            {
                SetField(ref students, value);
            }
        }
        public Dictionary<string, bool> AddedStudents { get; set; } = new();
        public Dictionary<string, string> DenyMessages { get; set; } = new();

        public UpcomingExamInfoWindow? Window { get; set; }

        public UpcomingExamInfoViewModel(NavigationStore navigationStore, CurrentExamStore currentExamStore, 
            IStudentDAO studentDAO, IUserProfileMapper userProfileMapper, IExamCoordinator examCoordinator,
            IClosePopupNavigationService closepopupNavigationService)
        {
            NavigationStore = navigationStore;
            _examCoordinator = examCoordinator;
            _userProfileMapper = userProfileMapper;
            _currentExamStore = currentExamStore;
            _studentDAO = studentDAO;
            _closepopupNavigationService = closepopupNavigationService;
            Students = new ObservableCollection<Student>(LoadStudents());
            foreach (Student student in Students)
            {
                AddedStudents.Add(student.Id, true);
                DenyMessages.Add(student.Id, "");
            }
            AddStudentCommand = new RelayCommand(AddStudent, canExecute => SelectedStudent != null);
            RemoveStudentCommand = new RelayCommand(RemoveStudent, canExecute => SelectedStudent != null);
            ConfirmListCommand = new RelayCommand(ConfirmList);
        }

        private List<Student> LoadStudents()
        {
            return _examCoordinator.GetAppliedStudents(_currentExamStore.CurrentExam!.Id);
        }
        private void SelectStudent()
        {
            if (SelectedStudent == null) return;
            Profile? profile = _userProfileMapper.GetProfile(new UserDto(selectedStudent, UserType.Student));
            if (profile == null) return;
            Email = profile.Email;

            Name = SelectedStudent.Name;
            Surname = SelectedStudent.Surname;
            PenaltyPts = SelectedStudent.PenaltyPts;
        }

        private void RemoveStudent(object? obj)
        {
            if (Message == "" && DenyMessages[SelectedStudent!.Id] == "")
            {
                MessageBox.Show("You need to create deny message!", "Error");
                return;
            }
            if (Message != "")
                DenyMessages[SelectedStudent!.Id] = Message;
            AddedStudents[SelectedStudent!.Id] = false;
            Window.RemoveClick();
            OnPropertyChanged();
        }
        private void DenyStudent(Student student)
        {
            _examCoordinator.CancelApplication(student!.Id, _currentExamStore.CurrentExam!.Id);
            _examCoordinator.SendNotification(message, student!.Id);
        }
        public void AddStudent(object? obj)
        {
            AddedStudents[SelectedStudent!.Id] = true;
            Window.AddClick();
            OnPropertyChanged();
        }

        private void AcceptStudent(Student student)
        {
            _examCoordinator.Accept(student!.Id, _currentExamStore.CurrentExam!.Id);
            _examCoordinator.SendNotification(AcceptMessage, student!.Id);
        }
        private void ConfirmList(object? obj)
        {
            int acceptedCount = AddedStudents.Count(item => item.Value == true);
            if (acceptedCount > _currentExamStore.CurrentExam!.MaxStudents)
            {
                MessageBox.Show($"Exam only has {_currentExamStore.CurrentExam!.MaxStudents} slots", "Error");
                return;
            }
            
            foreach (Student student in Students)
            {
                if (AddedStudents[student.Id])
                    AcceptStudent(student);
                else
                    DenyStudent(student);
            }

            _examCoordinator.ConfirmExam(_currentExamStore.CurrentExam);
            _closepopupNavigationService.Navigate(Factories.ViewType.Exam);
        }
    }
}
