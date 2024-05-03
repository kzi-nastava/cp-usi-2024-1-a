using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.CourseServices;
using LangLang.Services.NotificationServices;
using LangLang.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.ViewModel
{
    public class UpcomingCourseInfoViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }

        private readonly CurrentCourseStore _currentCourseStore;

        private readonly string AcceptMessage = "Application accepted!";

        private readonly IUserProfileMapper _userProfileMapper;

        private readonly IStudentCourseCoordinator _studentCourseCoordinator;

        private readonly IStudentDAO _studentDAO;
        public RelayCommand AcceptStudentCommand { get; }
        public RelayCommand DenyStudentCommand { get; }

        private string courseName = "";
        private string name = "";
        private string surname = "";
        private string email = "";
        private string message = "";
        private uint penaltyPts;
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

        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Student> Attendees { get; set; }

        public UpcomingCourseInfoViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, 
            IStudentDAO studentDAO, IUserProfileMapper userProfileMapper, IStudentCourseCoordinator studentCourseCoordinator)
        {
            NavigationStore = navigationStore;
            _studentCourseCoordinator = studentCourseCoordinator;
            _userProfileMapper = userProfileMapper;
            _currentCourseStore = currentCourseStore;
            _studentDAO = studentDAO;
            Students = new ObservableCollection<Student>(LoadStudents());
            Attendees = new ObservableCollection<Student>(LoadAttendees());
            CourseName = _currentCourseStore.CurrentCourse!.Name;
            AcceptStudentCommand = new RelayCommand(AcceptStudent, canExecute => SelectedStudent != null);
            DenyStudentCommand = new RelayCommand(DenyStudent, canExecute => SelectedStudent != null);
        }

        private List<Student> LoadAttendees()
        {
            return _studentCourseCoordinator.GetAttendanceStudentsCourse(_currentCourseStore.CurrentCourse!.Id);
        }

        private List<Student> LoadStudents()
        {
            return _studentCourseCoordinator.GetAppliedStudentsCourse(_currentCourseStore.CurrentCourse!.Id);
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

        private void DenyStudent(object? obj)
        {
            if (Message == "")
            {
                MessageBox.Show("You need to create deny message!", "Error");
                return;
            }
            _studentCourseCoordinator.CancelApplication(SelectedStudent!.Id, _currentCourseStore.CurrentCourse!.Id);
            _studentCourseCoordinator.SendNotification(Message, SelectedStudent!.Id);


            Students.Clear();
            Students = new ObservableCollection<Student>(LoadStudents());

            SelectedStudent = null;

            MessageBox.Show("Student is denied!", "Success");
        }

        private void AcceptStudent(object? obj)
        {
            if(_currentCourseStore.CurrentCourse!.IsFull())
            {
                // Maybe reject an application
                MessageBox.Show("Course is full.", "Error");
                return;
            }
            _studentCourseCoordinator.Accept(SelectedStudent!.Id, _currentCourseStore.CurrentCourse!.Id);


            _studentCourseCoordinator.SendNotification(AcceptMessage, SelectedStudent!.Id);

            SelectedStudent = null;

            MessageBox.Show("Student is accepted.", "Success");
        }
    }
}
