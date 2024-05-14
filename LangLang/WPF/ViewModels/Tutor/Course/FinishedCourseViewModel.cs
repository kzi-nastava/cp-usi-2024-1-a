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

namespace LangLang.WPF.ViewModels.Tutor.Course;

    public class FinishedCourseViewModel: ViewModelBase, INavigableDataContext
    {
    public NavigationStore NavigationStore { get; }

    private readonly CurrentCourseStore _currentCourseStore;

    private readonly IStudentDAO _studentDAO;

    private readonly IUserProfileMapper _userProfileMapper;
    private readonly IStudentCourseCoordinator _studentCourseCoordinator;
    private readonly ICourseAttendanceService _courseAttendanceService;
    public RelayCommand GradeStudentCommand { get; }
    public RelayCommand FinishCourseCommand { get; }

    private string courseName = "";
    private string name = "";
    private string surname = "";
    private string email = "";
    private string message = "";
    private uint penaltyPts;
    private ObservableCollection<int> grades = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int activityScore;
    private int knowledgeScore;
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
    public int ActivityScore
    {
        get => activityScore;
        set
        {
            SetField(ref activityScore, value);
        }
    }
    public int KnowledgeScore
    {
        get => knowledgeScore;
        set
        {
            SetField(ref knowledgeScore, value);
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
    public ObservableCollection<Domain.Model.Student> Students { get; set; }
    public ObservableCollection<int> Grades 
    {
        get => grades;
    }
    public FinishedCourseViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, 
        IStudentDAO studentDAO, IUserProfileMapper userProfileMapper, IStudentCourseCoordinator studentCourseCoordinator, 
        ICourseAttendanceService courseAttendanceService)
    {
        NavigationStore = navigationStore;
        _studentCourseCoordinator = studentCourseCoordinator;
        _courseAttendanceService = courseAttendanceService;
        _currentCourseStore = currentCourseStore;
        _studentDAO = studentDAO;
        _userProfileMapper = userProfileMapper;
        Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());
        CourseName = _currentCourseStore.CurrentCourse!.Name;
        GradeStudentCommand = new RelayCommand(GradeStudent, canExecute => SelectedStudent != null);
        FinishCourseCommand = new RelayCommand(FinishCourse, CanFinishCourse);
    }

    private bool CanFinishCourse(object? arg)
    {
        foreach (CourseAttendance attendance in _courseAttendanceService.GetAttendancesForCourse(_currentCourseStore.CurrentCourse!.Id))
        {
            if (!attendance.IsGraded)
            {
                return false;
            }
        }
        if(_currentCourseStore.CurrentCourse!.State == Domain.Model.Course.CourseState.FinishedGraded)
        {
            return false;
        }
        return true;
    }

    private void FinishCourse(object? obj)
    {
        
        _studentCourseCoordinator.FinishCourse(_currentCourseStore.CurrentCourse!.Id, Students);
        MessageBox.Show("Course finished successfully!", "Success", MessageBoxButton.OK);
    }

    private void GradeStudent(object? obj)
    {
        if (ActivityScore == 0 || KnowledgeScore == 0)
        {
            MessageBox.Show("Fill all grade fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _courseAttendanceService.GradeStudent(SelectedStudent!.Id, _currentCourseStore.CurrentCourse!.Id , KnowledgeScore, ActivityScore);

        MessageBox.Show($"Student {SelectedStudent!.Name} graded successfully with: activity {ActivityScore}, knowledge {KnowledgeScore} " +
            $"score", "Success", MessageBoxButton.OK);
    }

    private List<Domain.Model.Student> LoadStudents()
    {
        return _studentCourseCoordinator.GetAttendanceStudentsCourse(_currentCourseStore.CurrentCourse!.Id);
    }
    private void SelectStudent()
    {
        if (SelectedStudent == null) return;
        Profile? profile = _userProfileMapper.GetProfile(new UserDto(selectedStudent, UserType.Student));
        if (profile == null) return;
        Name = SelectedStudent.Name;
        Surname = SelectedStudent.Surname;
        Email = profile.Email;
        PenaltyPts = SelectedStudent.PenaltyPts;
        CourseAttendance? courseAttendance = _courseAttendanceService.GetStudentAttendance(SelectedStudent.Id);
        if (courseAttendance == null) return;
        ActivityScore = courseAttendance.ActivityGrade;
        KnowledgeScore = courseAttendance.KnowledgeGrade;

    }
}

