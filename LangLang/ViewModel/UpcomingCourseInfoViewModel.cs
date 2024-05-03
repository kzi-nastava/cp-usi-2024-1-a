using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.ViewModel
{
    public class UpcomingCourseInfoViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }

        private readonly CurrentCourseStore _currentCourseStore;

        private readonly IUserProfileMapper _userProfileMapper;

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

        public UpcomingCourseInfoViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, IStudentDAO studentDAO, IUserProfileMapper userProfileMapper)
        {
            NavigationStore = navigationStore;
            _userProfileMapper = userProfileMapper;
            _currentCourseStore = currentCourseStore;
            _studentDAO = studentDAO;
            Students = new ObservableCollection<Student>(LoadStudents());
            CourseName = _currentCourseStore.CurrentCourse!.Name;
            AcceptStudentCommand = new RelayCommand(AcceptStudent, canExecute => SelectedStudent != null);
            DenyStudentCommand = new RelayCommand(DenyStudent, canExecute => SelectedStudent != null);
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
            Profile? profile = _userProfileMapper.GetProfile(new UserDto(selectedStudent, UserType.Student));
            if (profile == null) return;
            Email = profile.Email;

            Name = SelectedStudent.Name;
            Surname = SelectedStudent.Surname;
            PenaltyPts = SelectedStudent.PenaltyPts;

        }

        private void DenyStudent(object? obj)
        {
            // TODO: call course coordinator to reject student
            // TODO: refresh the list
            MessageBox.Show("Student is denied!", "Success");
        }

        private void AcceptStudent(object? obj)
        {
            // TODO: call course coordinator to transform course application to course attendandce
            // TODO: change course number
            MessageBox.Show("Student is accepted.", "Success");
        }
    }
}
