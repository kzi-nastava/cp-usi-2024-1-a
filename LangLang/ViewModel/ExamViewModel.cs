using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;

namespace LangLang.ViewModel;

internal class ExamViewModel : ViewModelBase
{
    private ExamService examService;
    private LanguageService languageService;
    
    public RelayCommand AddCommand { get; set; }

    private ObservableCollection<Exam> exams;
    private ObservableCollection<Language> languages;
    private ObservableCollection<LanguageLvl> languageLevels;
    private ObservableCollection<TimeOnly> availableTimes;
    
    private Exam? selectedExam;

    private Language? language;
    private LanguageLvl? languageLvl;
    private String? examDate;
    private TimeOnly? examTime;
    private int maxStudents;
    private int numStudents;
    private string state = string.Empty; // TODO: change after enum definition
    
    private Language? filterLanguage;

    public ObservableCollection<Exam> Exams
    {
        get => exams;
        set => SetField(ref exams, value);
    }
    public ObservableCollection<Language> Languages
    {
        get => languages;
        set => SetField(ref languages, value);
    }
    public ObservableCollection<LanguageLvl> LanguageLevels
    {
        get => languageLevels;
        set => SetField(ref languageLevels, value);
    }
    public ObservableCollection<TimeOnly> AvailableTimes
    {
        get => availableTimes;
        set => SetField(ref availableTimes, value);
    }
    
    public Exam? SelectedExam
    {
        get => selectedExam;
        set => SetField(ref selectedExam, value);
    }
    
    public Language? Language
    {
        get => language;
        set => SetField(ref language, value);
    }
    public LanguageLvl? LanguageLvl
    {
        get => languageLvl;
        set => SetField(ref languageLvl, value);
    }
    public string? ExamDate
    {
        get => examDate;
        set => SetField(ref examDate, value);
    }
    public TimeOnly? ExamTime
    {
        get => examTime;
        set => SetField(ref examTime, value);
    }
    public int MaxStudents
    {
        get => maxStudents;
        set => SetField(ref maxStudents, value);
    }
    public int NumStudents
    {
        get => numStudents;
        set => SetField(ref numStudents, value);
    }
    public string State
    {
        get => state;
        set => SetField(ref state, value);
    }
    
    public Language? FilterLanguage
    {
        get => filterLanguage;
        set => SetField(ref filterLanguage, value);
    }
    
    public ExamViewModel(ExamService examService, LanguageService languageService)
    {
        this.examService = examService;
        this.languageService = languageService;
        
        exams = new ObservableCollection<Exam>(examService.GetAllExams());
        languages = new ObservableCollection<Language>(languageService.GetAll());
        languageLevels = new ObservableCollection<LanguageLvl>(Enum.GetValues<LanguageLvl>());
        
        // TODO: load from TimetableService
        availableTimes = new ObservableCollection<TimeOnly>{TimeOnly.Parse("08:00"), TimeOnly.Parse("12:00"), TimeOnly.Parse("16:00")};
        
        AddCommand = new RelayCommand(execute => AddExam(), execute => CanAddExam());
    }
    
    private void AddExam()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(DateTime.ParseExact(ExamDate, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)) : null;
        try
        {
            Exam exam = examService.AddExam(Language, LanguageLvl, selectedDate, ExamTime, MaxStudents);
            Exams.Add(exam);
            ResetFields();
        }
        catch (ArgumentException e)
        {
            MessageBox.Show($"Error adding exam: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanAddExam()
    {
        return true;
    }
    
    private void ResetFields()
    {
        Language = null;
        LanguageLvl = null;
        ExamDate = null;
        ExamTime = null;
        MaxStudents = 0;
    }
}