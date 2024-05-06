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

    public class FinishedExamInfoViewModel: ViewModelBase, INavigableDataContext
    {
    public NavigationStore NavigationStore { get; }

    private readonly CurrentExamStore _currentExamStore;

    private readonly IStudentDAO _studentDAO;

    private readonly IUserProfileMapper _userProfileMapper;
    public RelayCommand GradeStudentCommand { get; }

    private string name = "";
    private string surname = "";
    private string email = "";
    private uint penaltyPts;
    private string readingScore = "";
    private string writingScore = "";
    private string listeningScore = "";
    private string speakingScore = "";
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
    public string ReadingScore
    {
        get => readingScore;
        set => SetField(ref readingScore, value);
    }
    public string WritingScore
    {
        get => writingScore;
        set => SetField(ref writingScore, value);
    }
    public string ListeningScore
    {
        get => listeningScore;
        set => SetField(ref listeningScore, value);
    }
    public string SpeakingScore
    {
        get => speakingScore;
        set => SetField(ref speakingScore, value);
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
    public FinishedExamInfoViewModel(NavigationStore navigationStore, CurrentExamStore currentExamStore, IStudentDAO studentDAO, IUserProfileMapper userProfileMapper)
    {
        NavigationStore = navigationStore;
        _currentExamStore = currentExamStore;
        _studentDAO = studentDAO;
        _userProfileMapper = userProfileMapper;
        Students = new ObservableCollection<Student>(LoadStudents());
        GradeStudentCommand = new RelayCommand(GradeStudent, canExecute => SelectedStudent != null);
    }

    private void GradeStudent(object? obj)
    {
        if (ReadingScore == "" || WritingScore == "" || ListeningScore == "" || SpeakingScore == "")
        {
            MessageBox.Show("Fill all grade fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        uint reading, writing, listening, speaking;
        try
        {
            reading = uint.Parse(ReadingScore);
            writing = uint.Parse(WritingScore);
            listening = uint.Parse(ListeningScore);
            speaking = uint.Parse(SpeakingScore);
        }
        catch
        {
            MessageBox.Show("Grades must be positive numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (reading > 60 || writing > 60 || listening > 40 || speaking > 50)
        {
            MessageBox.Show("Grades not in bounds!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // TODO: Store grade
        MessageBox.Show($"Student {SelectedStudent!.Name} graded successfully!", "Success", MessageBoxButton.OK);
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

