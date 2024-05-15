using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Course;
using LangLang.Application.Utility.Authentication;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Tutor.Course
{
    public class UpcomingCourseViewModel : ViewModelBase, INavigableDataContext
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
        private ObservableCollection<Domain.Model.Student> attendees = new ObservableCollection<Domain.Model.Student>();
        private ObservableCollection<Domain.Model.Student> students = new ObservableCollection<Domain.Model.Student>();
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

        private Domain.Model.Student? selectedStudent;
        public Domain.Model.Student? SelectedStudent
        {
            get => selectedStudent;
            set
            {
                SetField(ref selectedStudent, value);
                SelectStudent();
            }
        }

        public ObservableCollection<Domain.Model.Student> Students 
        {
            get => students;
            set
            {
                SetField(ref students, value);
            }
        }
        public ObservableCollection<Domain.Model.Student> Attendees
        {
            get => attendees;
            set
            {
                SetField(ref attendees, value);
            }
        }

        public UpcomingCourseViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, 
            IStudentDAO studentDAO, IUserProfileMapper userProfileMapper, IStudentCourseCoordinator studentCourseCoordinator)
        {
            NavigationStore = navigationStore;
            _studentCourseCoordinator = studentCourseCoordinator;
            _userProfileMapper = userProfileMapper;
            _currentCourseStore = currentCourseStore;
            _studentDAO = studentDAO;
            Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());
            Attendees = new ObservableCollection<Domain.Model.Student>(LoadAttendees());
            CourseName = _currentCourseStore.CurrentCourse!.Name;
            AcceptStudentCommand = new RelayCommand(AcceptStudent, canExecute => SelectedStudent != null);
            DenyStudentCommand = new RelayCommand(DenyStudent, canExecute => SelectedStudent != null);
        }

        private List<Domain.Model.Student> LoadAttendees()
        {
            return _studentCourseCoordinator.GetAttendanceStudentsCourse(_currentCourseStore.CurrentCourse!.Id);
        }

        private List<Domain.Model.Student> LoadStudents()
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
            PenaltyPts = SelectedStudent.PenaltyPoints;

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
            Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());

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

            Students.Clear();
            Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());
            Attendees.Clear();
            Attendees = new ObservableCollection<Domain.Model.Student>(LoadAttendees());

            SelectedStudent = null;

            MessageBox.Show("Student is accepted.", "Success");
        }
    }
}
