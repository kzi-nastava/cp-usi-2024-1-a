using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Common;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.DropRequest;
using LangLang.Application.Utility.Authentication;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Tutor.Course
{
    public class ActiveCourseViewModel : ViewModelBase, INavigableDataContext
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
        private ObservableCollection<Domain.Model.Student> _students = new ObservableCollection<Domain.Model.Student>();
        private ObservableCollection<DropRequestViewModel> _dropRequests = new ObservableCollection<DropRequestViewModel>();
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

        private DropRequestViewModel? _selectedDropRequestDisplay;
        public DropRequestViewModel? SelectedDropRequestDisplay
        {
            get => _selectedDropRequestDisplay;
            set
            {
                SetField(ref _selectedDropRequestDisplay, value);
                SelectDropRequest();
            }
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
        public ObservableCollection<DropRequestViewModel> DropRequests
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

        public ActiveCourseViewModel(NavigationStore navigationStore, 
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
            Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());
            DropRequests = new ObservableCollection<DropRequestViewModel>(LoadDropRequests());
            CourseName = _currentCourseStore.CurrentCourse!.Name;
            AcceptStudentCommand = new RelayCommand(AcceptStudent, canExecute => SelectedDropRequestDisplay != null);
            DenyStudentCommand = new RelayCommand(DenyStudent, canExecute => SelectedDropRequestDisplay != null);
            GivePenaltyPointCommand = new RelayCommand(GivePenaltyPoint, canExecute => SelectedStudent != null);
        }

        private List<DropRequestViewModel> LoadDropRequests()
        {
            var dropRequests = _dropRequestService.GetInReviewDropRequests(_currentCourseStore.CurrentCourse!.Id);

            var senderNames = _dropRequestInfoService.GetSenderNames(dropRequests);

            return new List<DropRequestViewModel>(dropRequests
                        .Select(dropRequest => new DropRequestViewModel(senderNames[dropRequest.Id], dropRequest)));

        }

        private List<Domain.Model.Student> LoadStudents()
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
            PenaltyPts = SelectedStudent.PenaltyPoints;

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
            Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());
        }

        private void DenyStudent(object? obj)
        {
            if(SelectedDropRequestDisplay == null)
            {
                MessageBox.Show("Select drop request first!", "Error");
                return;
            }
            _studentCourseCoordinator.DenyDropRequest(SelectedDropRequestDisplay!.DropRequest);

            UserDto student = _userProfileMapper.GetPerson(_profileService.GetProfile(SelectedDropRequestDisplay.DropRequest.SenderId)!);

            
            _penaltyService.AddPenaltyPoint((Domain.Model.Student)student.Person!);

            DropRequests.Clear();
            DropRequests = new ObservableCollection<DropRequestViewModel>(LoadDropRequests());

            Sender = "";
            DropMessage = "";

            MessageBox.Show("Drop request is denied successfully", "Success");

        }

        private void AcceptStudent(object? obj)
        {
            if (SelectedDropRequestDisplay == null)
            {
                MessageBox.Show("Select drop request first!", "Error");
                return;
            }
            _studentCourseCoordinator.AcceptDropRequest(SelectedDropRequestDisplay!.DropRequest);

            DropRequests.Clear();
            DropRequests = new ObservableCollection<DropRequestViewModel>(LoadDropRequests());

            Sender = "";
            DropMessage = "";

            MessageBox.Show("Drop request is accepted successfully", "Success");
        }
    }
}
