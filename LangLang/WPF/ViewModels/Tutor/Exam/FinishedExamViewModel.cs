using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.Utility.Authentication;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Tutor.Exam;

    public class FinishedExamViewModel: ViewModelBase, INavigableDataContext
    {
    public NavigationStore NavigationStore { get; }

    private readonly Domain.Model.Exam _exam;

    private readonly IUserProfileMapper _userProfileMapper;

    private readonly IExamAttendanceService _examAttendanceService;
    
    public RelayCommand GradeStudentCommand { get; }

    private string name = "";
    private string surname = "";
    private string email = "";
    private uint penaltyPts;
    private string readingScore = "";
    private string writingScore = "";
    private string listeningScore = "";
    private string speakingScore = "";
    private bool graded = false;
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
    public bool Graded
    {
        get => graded;
        set => SetField(ref graded, value);
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
    public FinishedExamViewModel(NavigationStore navigationStore, CurrentExamStore currentExamStore, IUserProfileMapper userProfileMapper, IExamAttendanceService examAttendanceService)
    {
        NavigationStore = navigationStore;
        _exam = currentExamStore.CurrentExam ??
                throw new InvalidOperationException(
                    "Cannot create FinishedExamInfoViewModel without the current exam set.");
        _userProfileMapper = userProfileMapper;
        _examAttendanceService = examAttendanceService;
        Students = new ObservableCollection<Domain.Model.Student>(LoadStudents());
        GradeStudentCommand = new RelayCommand(GradeStudent, _ => CanGradeStudent());
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

        try
        {
            _examAttendanceService.GradeStudent(SelectedStudent!.Id, _exam.Id,
                new ExamGradeDto(reading, writing, listening, speaking));
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error");
            return;
        }

        Graded = true;
        MessageBox.Show($"Student {SelectedStudent!.Name} graded successfully!", "Success", MessageBoxButton.OK);
    }

    private bool CanGradeStudent()
    {
        return SelectedStudent != null && !IsGraded(SelectedStudent);
    }

    private bool IsGraded(Domain.Model.Student student)
    {
        var attendance = _examAttendanceService.GetAttendance(student.Id, _exam.Id);
        return attendance is { IsGraded: true };
    }

    private List<Domain.Model.Student> LoadStudents()
    {
        List<Domain.Model.Student> students = new List<Domain.Model.Student>();
        foreach (Domain.Model.Student student in _examAttendanceService.GetStudents(_exam.Id))
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
        PenaltyPts = SelectedStudent.PenaltyPoints;
        Graded = IsGraded(SelectedStudent);
    }
}

