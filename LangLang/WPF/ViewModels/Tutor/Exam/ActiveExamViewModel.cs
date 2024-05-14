using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Common;
using LangLang.Application.UseCases.DropRequest;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Tutor.Exam
{
    public class ActiveExamViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }
        public IAuthenticationStore AuthenticationStore { get; }
        private readonly CurrentExamStore _currentExamStore;
        private readonly IStudentDAO _studentDAO;
        private readonly IUserProfileMapper _userProfileMapper;
        private readonly IPenaltyService _penaltyService;
        private readonly IExamCoordinator _examCoordinator;
        private readonly IDropRequestService _dropRequestService;
        private readonly IDropRequestInfoService _dropRequestInfoService;
        private readonly IProfileService _profileService;
        private readonly IAccountService _accountService;
        private readonly IClosePopupNavigationService _closepopupNavigationService;
        public RelayCommand KickStudentCommand { get; }
        public RelayCommand FinishExamCommand { get; }

        private string _name = "";
        private string _surname = "";
        private string _email = "";
        private uint _penaltyPts;
        private ObservableCollection<Domain.Model.Student> _students = new ObservableCollection<Domain.Model.Student>();
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        public string Surname
        {
            get => _surname;
            set => SetField(ref _surname, value);
        }
        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }
        public uint PenaltyPts
        {
            get => _penaltyPts;
            set => SetField(ref _penaltyPts, value);
        }

        private Domain.Model.Student? _selectedStudent;
        public Domain.Model.Student? SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                SetField(ref _selectedStudent, value);
                SelectStudent();
            }
        }

        public ObservableCollection<Domain.Model.Student> Students
        {
            get => _students;
            set
            {
                SetField(ref _students, value);
            }
        }

        public ActiveExamViewModel(NavigationStore navigationStore, 
            CurrentExamStore currentExamStore, IStudentDAO studentDAO, 
            IUserProfileMapper userProfileMapper, IPenaltyService penaltyService,
            IExamCoordinator examCoordinator, IDropRequestService dropRequestService,
            IAuthenticationStore authenticationStore, IDropRequestInfoService dropRequestInfoService,
            IProfileService profileService, IAccountService accountService, IClosePopupNavigationService closepopupNavigationService)
        {
            AuthenticationStore = authenticationStore;
            NavigationStore = navigationStore;
            _profileService = profileService;
            _dropRequestInfoService = dropRequestInfoService;
            _examCoordinator = examCoordinator;
            _currentExamStore = currentExamStore;
            _userProfileMapper = userProfileMapper;
            _penaltyService = penaltyService;
            _dropRequestService = dropRequestService;
            _accountService = accountService;
            _closepopupNavigationService = closepopupNavigationService;

            _studentDAO = studentDAO;
            Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());
            KickStudentCommand = new RelayCommand(KickStudent, canExecute => SelectedStudent != null);
            FinishExamCommand = new RelayCommand(FinishExam);
        }

        private List<Domain.Model.Student> LoadStudents()
        {
            return _examCoordinator.GetAttendanceStudents(_currentExamStore.CurrentExam!.Id);
        }
        private void SelectStudent()
        {
            if (SelectedStudent == null) return;
            Profile? profile = _userProfileMapper.GetProfile(new UserDto(SelectedStudent, UserType.Student));
            if (profile == null) return;
            Name = SelectedStudent.Name;
            Surname = SelectedStudent.Surname;
            Email = profile.Email;
            PenaltyPts = SelectedStudent.PenaltyPoints;
        }

        private void FinishExam(object? obj)
        {
            _examCoordinator.FinishExam(_currentExamStore.CurrentExam!);
            _closepopupNavigationService.Navigate(Factories.ViewType.Exam);
        }
        private void KickStudent(object? obj)
        {
            _accountService.DeactivateStudentAccount(SelectedStudent!);

            Students.Clear();
            LoadStudents();

            SelectedStudent = null;

            MessageBox.Show("Student is kicked and deleted successfully", "Success");

        }
    }
}
