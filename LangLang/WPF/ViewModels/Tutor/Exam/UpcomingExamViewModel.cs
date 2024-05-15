using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.Utility.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Tutor.Exam
{
    public class UpcomingExamViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }

        private readonly CurrentExamStore _currentExamStore;

        private readonly string AcceptMessage = "Application accepted!";

        private readonly IUserProfileMapper _userProfileMapper;

        private readonly IExamCoordinator _examCoordinator;

        private readonly IStudentRepository _studentRepository;
        private readonly IClosePopupNavigationService _closepopupNavigationService;
        public RelayCommand AddStudentCommand { get; }
        public RelayCommand RemoveStudentCommand { get; }
        public RelayCommand ConfirmListCommand { get; }

        private string name = "";
        private string surname = "";
        private string email = "";
        private string message = "";
        private uint penaltyPts;
        private ObservableCollection<StudentAddedStatusViewModel> studentsStatuses = new();
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

        private StudentAddedStatusViewModel? selectedStudent;
        public StudentAddedStatusViewModel? SelectedStudent
        {
            get => selectedStudent;
            set
            {
                SetField(ref selectedStudent, value);
                SelectStudent();
            }
        }

        public ObservableCollection<StudentAddedStatusViewModel> StudentsStatuses
        {
            get => studentsStatuses;
            set
            {
                SetField(ref studentsStatuses, value);
            }
        }
        public Dictionary<string, string> DenyMessages { get; set; } = new();

        public UpcomingExamViewModel(NavigationStore navigationStore, CurrentExamStore currentExamStore, 
            IStudentRepository studentRepository, IUserProfileMapper userProfileMapper, IExamCoordinator examCoordinator,
            IClosePopupNavigationService closepopupNavigationService)
        {
            NavigationStore = navigationStore;
            _examCoordinator = examCoordinator;
            _userProfileMapper = userProfileMapper;
            _currentExamStore = currentExamStore;
            _studentRepository = studentRepository;
            _closepopupNavigationService = closepopupNavigationService;
            StudentsStatuses = new ObservableCollection<StudentAddedStatusViewModel>();
            LoadStudents();
            foreach (var dto in StudentsStatuses)
            {
                DenyMessages.Add(dto.Student.Id, "");
            }
            AddStudentCommand = new RelayCommand(AddStudent, canExecute => SelectedStudent != null);
            RemoveStudentCommand = new RelayCommand(RemoveStudent, canExecute => SelectedStudent != null);
            ConfirmListCommand = new RelayCommand(ConfirmList);
        }

        private void LoadStudents()
        {
            foreach (Domain.Model.Student student in _examCoordinator.GetAppliedStudents(_currentExamStore.CurrentExam!.Id))
                StudentsStatuses.Add(new StudentAddedStatusViewModel(student, true));
        }
        private void SelectStudent()
        {
            if (SelectedStudent == null) return;
            Profile? profile = _userProfileMapper.GetProfile(new UserDto(selectedStudent.Student, UserType.Student));
            if (profile == null) return;
            Email = profile.Email;

            Name = SelectedStudent.Student.Name;
            Surname = SelectedStudent.Student.Surname;
            PenaltyPts = SelectedStudent.Student.PenaltyPoints;
        }

        private void RemoveStudent(object? obj)
        {
            if (Message == "" && DenyMessages[SelectedStudent!.Student.Id] == "")
            {
                MessageBox.Show("You need to create deny message!", "Error");
                return;
            }
            if (Message != "")
                DenyMessages[SelectedStudent!.Student.Id] = Message;
            for (int i = 0; i < StudentsStatuses.Count; i++)
            {
                if (StudentsStatuses[i] == SelectedStudent)
                {
                    StudentAddedStatusViewModel newViewModel = new(StudentsStatuses[i].Student, false);
                    StudentsStatuses[i] = newViewModel;
                    break;
                }
            }
            OnPropertyChanged(nameof(StudentsStatuses));
        }
        private void DenyStudent(Domain.Model.Student student)
        {
            _examCoordinator.CancelApplication(student!.Id, _currentExamStore.CurrentExam!.Id);
            _examCoordinator.SendNotification(message, student!.Id);
        }
        public void AddStudent(object? obj)
        {
            for (int i = 0; i < StudentsStatuses.Count; i++)
            {
                if (StudentsStatuses[i] == SelectedStudent)
                {
                    StudentAddedStatusViewModel newViewModel = new(StudentsStatuses[i].Student, true);
                    StudentsStatuses[i] = newViewModel;
                    break;
                }
            }
            OnPropertyChanged(nameof(StudentsStatuses));
        }

        private void AcceptStudent(Domain.Model.Student student)
        {
            _examCoordinator.Accept(student!.Id, _currentExamStore.CurrentExam!.Id);
            _examCoordinator.SendNotification(AcceptMessage, student!.Id);
        }
        private void ConfirmList(object? obj)
        {
            int acceptedCount = StudentsStatuses.Count(item => item.Added == true);
            if (acceptedCount > _currentExamStore.CurrentExam!.MaxStudents)
            {
                MessageBox.Show($"Exam only has {_currentExamStore.CurrentExam!.MaxStudents} slots", "Error");
                return;
            }
            
            foreach (var dto in StudentsStatuses)
            {
                if (dto.Added)
                    AcceptStudent(dto.Student);
                else
                    DenyStudent(dto.Student);
            }

            _examCoordinator.ConfirmExam(_currentExamStore.CurrentExam);
            _closepopupNavigationService.Navigate(Factories.ViewType.Exam);
        }
    }
}
