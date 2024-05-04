using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using LangLang.Model.Display;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.CourseServices;
using LangLang.Services.DropRequestServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LangLang.ViewModel
{
    public class ActiveCourseInfoViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }
        public IAuthenticationStore AuthenticationStore { get; }
        private readonly CurrentCourseStore _currentCourseStore;
        private readonly IStudentDAO _studentDAO;
        private readonly IUserProfileMapper _userProfileMapper;
        private readonly IPenaltyService _penaltyService;
        private readonly IStudentCourseCoordinator _studentCourseCoordinator;
        private readonly IDropRequestService _dropRequestService;
        private readonly IDropRequestInfoService _dropRequestInfoService;
        private readonly IProfileService _profileService;
        public RelayCommand AcceptStudentCommand { get; }
        public RelayCommand DenyStudentCommand { get; }
        public RelayCommand GivePenaltyPointCommand { get; }

        private string _courseName = "";
        private string _name = "";
        private string _surname = "";
        private string _email = "";
        private uint _penaltyPts;
        private string _sender = "";
        private string _dropMessage = "";
        private ObservableCollection<Student> _students = new ObservableCollection<Student>();
        private ObservableCollection<DropRequestDisplay> _dropRequests = new ObservableCollection<DropRequestDisplay>();
        private ObservableCollection<string> _dropRequestsPreview = new ObservableCollection<string>();
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
        public string CourseName
        {
            get => _courseName;
            set
            {
                SetField(ref _courseName, value);
            }
        }
        public string Sender
        {
            get => _sender;
            set
            {
                SetField(ref _sender, value);
            }
        }
        public string DropMessage
        {
            get => _dropMessage;
            set
            {
                SetField(ref _dropMessage, value);
            }
        }

        private DropRequestDisplay? _selectedDropRequestDisplay;
        public DropRequestDisplay? SelectedDropRequestDisplay
        {
            get => _selectedDropRequestDisplay;
            set
            {
                SetField(ref _selectedDropRequestDisplay, value);
                SelectDropRequest();
            }
        }

        private Student? _selectedStudent;
        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                SetField(ref _selectedStudent, value);
                SelectStudent();
            }
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                SetField(ref _students, value);
            }
        }
        public ObservableCollection<DropRequestDisplay> DropRequests
        {
            get => _dropRequests;
            set
            {
                SetField(ref _dropRequests, value);
            }
        }
        public ObservableCollection<string> DropRequestsPreview
        {
            get => _dropRequestsPreview;
            set
            {
                SetField(ref _dropRequestsPreview, value);
            }
        }

        public ActiveCourseInfoViewModel(NavigationStore navigationStore, 
            CurrentCourseStore currentCourseStore, IStudentDAO studentDAO, 
            IUserProfileMapper userProfileMapper, IPenaltyService penaltyService,
            IStudentCourseCoordinator studentCourseCoordinator, IDropRequestService dropRequestService,
            IAuthenticationStore authenticationStore, IDropRequestInfoService dropRequestInfoService,
            IProfileService profileService)
        {
            AuthenticationStore = authenticationStore;
            NavigationStore = navigationStore;
            _profileService = profileService;
            _dropRequestInfoService = dropRequestInfoService;
            _studentCourseCoordinator = studentCourseCoordinator;
            _currentCourseStore = currentCourseStore;
            _userProfileMapper = userProfileMapper;
            _penaltyService = penaltyService;
            _dropRequestService = dropRequestService;
            _studentDAO = studentDAO;
            Students = new ObservableCollection<Student>(LoadStudents());
            DropRequests = new ObservableCollection<DropRequestDisplay>(LoadDropRequests());
            CourseName = _currentCourseStore.CurrentCourse!.Name;
            AcceptStudentCommand = new RelayCommand(AcceptStudent, canExecute => SelectedDropRequestDisplay != null);
            DenyStudentCommand = new RelayCommand(DenyStudent, canExecute => SelectedDropRequestDisplay != null);
            GivePenaltyPointCommand = new RelayCommand(GivePenaltyPoint, canExecute => SelectedStudent != null);
        }

        private List<DropRequestDisplay> LoadDropRequests()
        {
            var dropRequests = _dropRequestService.GetInReviewDropRequests(_currentCourseStore.CurrentCourse!.Id);

            var senderNames = _dropRequestInfoService.GetSenderNames(dropRequests);

            return new List<DropRequestDisplay>(dropRequests
                        .Select(dropRequest => new DropRequestDisplay(senderNames[dropRequest.Id], dropRequest)));

        }

        private List<Student> LoadStudents()
        {
            return _studentCourseCoordinator.GetAttendanceStudentsCourse(_currentCourseStore.CurrentCourse!.Id);
        }
        private void SelectStudent()
        {
            if (SelectedStudent == null) return;
            Profile? profile = _userProfileMapper.GetProfile(new UserDto(SelectedStudent, UserType.Student));
            if (profile == null) return;
            Name = SelectedStudent.Name;
            Surname = SelectedStudent.Surname;
            Email = profile.Email;
            PenaltyPts = SelectedStudent.PenaltyPts;

        }
        private void SelectDropRequest()
        {
            if (SelectedDropRequestDisplay == null) return;

            Sender = SelectedDropRequestDisplay!.Sender;
            DropMessage = SelectedDropRequestDisplay!.DropRequest.Message;

        }
        private void GivePenaltyPoint(object? obj)
        {
            _penaltyService.AddPenaltyPoint(SelectedStudent!);
            MessageBox.Show("Penalty point added.", "Success");
            Students.Clear();
            Students = new ObservableCollection<Student>(LoadStudents());
        }

        private void DenyStudent(object? obj)
        {
            if(SelectedDropRequestDisplay == null)
            {
                MessageBox.Show("Select drop request first!", "Error");
                return;
            }
            _dropRequestService.Deny(SelectedDropRequestDisplay!.DropRequest);

            UserDto student = _userProfileMapper.GetPerson(_profileService.GetProfile(SelectedDropRequestDisplay.DropRequest.SenderId)!);
            
            _penaltyService.AddPenaltyPoint((Student)student.Person!);
            MessageBox.Show("Drop request is denied successfully", "Success");

        }

        private void AcceptStudent(object? obj)
        {
            if (SelectedDropRequestDisplay == null)
            {
                MessageBox.Show("Select drop request first!", "Error");
                return;
            }
            _dropRequestService.Accept(SelectedDropRequestDisplay!.DropRequest);
            MessageBox.Show("Drop request is accepted successfully", "Success");
        }
    }
}
