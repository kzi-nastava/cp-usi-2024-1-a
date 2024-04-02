using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public RelayCommand SelectedExamChangedCommand { get; set; }
    public RelayCommand UpdateCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }

    private ObservableCollection<Exam> exams;
    private ObservableCollection<Language> languages;
    private ObservableCollection<LanguageLvl> languageLevels;
    private ObservableCollection<TimeOnly> availableTimes;
    
    private Exam? selectedExam;

    private Language? language;
    private LanguageLvl? languageLvl;
    private DateTime? examDate;
    private TimeOnly? examTime;
    private int maxStudents;
    private int numStudents;
    private string state = string.Empty; // TODO: change after enum definition
    
    private Language? filterLanguage;
    private LanguageLvl? filterLanguageLvl;
    private DateTime? filterDate;

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
    public DateTime? ExamDate
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
        set {
            SetField(ref filterLanguage, value);
            FilterExams();
        }
    }
    
    public LanguageLvl? FilterLanguageLvl
    {
        get => filterLanguageLvl;
        set {
            SetField(ref filterLanguageLvl, value);
            FilterExams();
        }
    }
    
    public DateTime? FilterDate
    {
        get => filterDate;
        set {
            SetField(ref filterDate, value);
            FilterExams();
        }
    }
    
    public ExamViewModel(ExamService examService, LanguageService languageService)
    {
        this.examService = examService;
        this.languageService = languageService;
        
        exams = new ObservableCollection<Exam>(LoadExams());
        languages = new ObservableCollection<Language>(languageService.GetAll());
        languageLevels = new ObservableCollection<LanguageLvl>(Enum.GetValues<LanguageLvl>());
        
        // TODO: load from TimetableService
        availableTimes = new ObservableCollection<TimeOnly>{TimeOnly.Parse("08:00"), TimeOnly.Parse("12:00"), TimeOnly.Parse("16:00")};
        
        AddCommand = new RelayCommand(execute => AddExam(), execute => CanAddExam());
        SelectedExamChangedCommand = new RelayCommand(execute => SelectExam());
        UpdateCommand = new RelayCommand(execute => UpdateExam(), execute => CanUpdateExam());
        DeleteCommand = new RelayCommand(execute => DeleteExam(), execute => CanDeleteExam());
    }
    
    private void AddExam()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
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
        return Language != null && LanguageLvl != null && ExamDate != null && ExamTime != null && MaxStudents > 0;
    }
    
    private void ResetFields()
    {
        Language = null;
        LanguageLvl = null;
        ExamDate = null;
        ExamTime = null;
        MaxStudents = 0;
    }

    private void SelectExam()
    {
        if (SelectedExam == null) return;
        Language = SelectedExam.Language;
        LanguageLvl = SelectedExam.LanguageLvl;
        ExamDate = SelectedExam.Time.Date;
        ExamTime = SelectedExam.TimeOfDay;
        MaxStudents = SelectedExam.MaxStudents;
        NumStudents = SelectedExam.NumStudents;
        // State = SelectedExam.State;
    }

    private void UpdateExam()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        try
        {
            Exam exam = examService.UpdateExam(selectedExam!.Id, Language, LanguageLvl, selectedDate, ExamTime, MaxStudents);
            Exams[Exams.IndexOf(SelectedExam!)] = exam;
            ResetFields();
        }
        catch (ArgumentException e)
        {
            MessageBox.Show($"Error updating exam: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private bool CanUpdateExam()
    {
        return SelectedExam != null;
    }
    
    private void DeleteExam()
    {
        try
        {
            examService.DeleteExam(selectedExam!.Id);
            Exams.Remove(SelectedExam!);
        }
        catch (ArgumentException e)
        {
            MessageBox.Show($"Error deleting exam: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private bool CanDeleteExam()
    {
        return SelectedExam != null;
    }
    
    private void FilterExams()
    {
        DateOnly? filterDateOnly = FilterDate != null ? DateOnly.FromDateTime(FilterDate!.Value) : null;
        Exams = new ObservableCollection<Exam>(examService.FilterExams(LoadExams(), FilterLanguage, FilterLanguageLvl, filterDateOnly));
    }
    
    private List<Exam> LoadExams()
    {
        return examService.GetAllExams();
    }
}