using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.ViewModel;

    public class FinishedCourseInfoViewModel: ViewModelBase, INavigableDataContext
    {
    public NavigationStore NavigationStore { get; }

    private readonly CurrentCourseStore _currentCourseStore;

    private readonly IStudentDAO _studentDAO;

    private readonly IUserProfileMapper _userProfileMapper;
    public RelayCommand GradeStudentCommand { get; }

    private string courseName = "";
    private string name = "";
    private string surname = "";
    private string email = "";
    private string message = "";
    private uint penaltyPts;
    private ObservableCollection<uint> grades = new ObservableCollection<uint> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private uint readingScore;
    private uint writingScore;
    private uint listeningScore;
    private uint speakingScore;
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
    public uint ReadingScore
    {
        get => readingScore;
        set
        {
            SetField(ref readingScore, value);
        }
    }
    public uint WritingScore
    {
        get => writingScore;
        set
        {
            SetField(ref writingScore, value);
        }
    }
    public uint ListeningScore
    {
        get => listeningScore;
        set
        {
            SetField(ref listeningScore, value);
        }
    }
    public uint SpeakingScore
    {
        get => speakingScore;
        set
        {
            SetField(ref speakingScore, value);
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
    public ObservableCollection<uint> Grades 
    {
        get => grades;
    }
    public FinishedCourseInfoViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, IStudentDAO studentDAO, IUserProfileMapper userProfileMapper)
    {
        NavigationStore = navigationStore;
        _currentCourseStore = currentCourseStore;
        _studentDAO = studentDAO;
        _userProfileMapper = userProfileMapper;
        Students = new ObservableCollection<Student>(LoadStudents());
        CourseName = _currentCourseStore.CurrentCourse!.Name;
        GradeStudentCommand = new RelayCommand(GradeStudent, canExecute => SelectedStudent != null);
    }

    private void GradeStudent(object? obj)
    {
        if (ReadingScore == 0 || WritingScore == 0 || ListeningScore == 0 || SpeakingScore == 0)
        {
            MessageBox.Show("Fill all grade fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        MessageBox.Show($"Student {SelectedStudent!.Name} graded successfully with: reading {ReadingScore}, writing {WritingScore}, listening {ListeningScore} and " +
            $" speaking {SpeakingScore} score", "Success", MessageBoxButton.OK);
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
        Name = SelectedStudent.Name;
        Surname = SelectedStudent.Surname;
        Email = profile.Email;
        PenaltyPts = SelectedStudent.PenaltyPts;

    }
}

