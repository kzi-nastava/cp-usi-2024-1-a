using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.Utility.Navigation;
using LangLang.Application.Utility.Timetable;
using LangLang.Domain.Model;
using LangLang.Domain;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Factories;

namespace LangLang.WPF.ViewModels.Tutor.Exam;

public class ExamOverviewViewModel : ViewModelBase
{
    private readonly Domain.Model.Tutor _tutor;
    
    private readonly IExamService _examService;
    private readonly ITimetableService _timetableService;
    private readonly NavigationStore _navigationStore;
    private readonly IPopupNavigationService _popupNavigationService;
    private readonly CurrentExamStore _currentExamStore;
    public RelayCommand OpenExamInfoCommand { get; }
    public RelayCommand AddCommand { get; set; }
    public RelayCommand SelectedExamChangedCommand { get; set; }
    public RelayCommand UpdateCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand ClearFiltersCommand { get; set; }

    private ObservableCollection<Domain.Model.Exam> exams;
    private ObservableCollection<Language> languages;
    private ObservableCollection<LanguageLevel> languageLevels;
    private ObservableCollection<LanguageLevel> filterLanguageLevels;
    private ObservableCollection<TimeOnly> availableTimes;
    
    private Domain.Model.Exam? selectedExam;

    private Language? language;
    private LanguageLevel? _languageLevel;
    private DateTime? examDate;
    private TimeOnly? examTime;
    private int maxStudents;
    private int numStudents;
    private Domain.Model.Exam.State examState;
    
    private Language? filterLanguage;
    private LanguageLevel? filterLanguageLvl;
    private DateTime? filterDate;
    
    private Dictionary<Language, List<LanguageLevel>> languageToLvl;

    public ObservableCollection<Domain.Model.Exam> Exams
    {
        get => exams;
        set => SetField(ref exams, value);
    }
    public ObservableCollection<Language> Languages
    {
        get => languages;
        set => SetField(ref languages, value);
    }
    public ObservableCollection<LanguageLevel> LanguageLevels
    {
        get => languageLevels;
        set => SetField(ref languageLevels, value);
    }
    public ObservableCollection<LanguageLevel> FilterLanguageLevels
    {
        get => filterLanguageLevels;
        set => SetField(ref filterLanguageLevels, value);
    }
    public ObservableCollection<TimeOnly> AvailableTimes
    {
        get => availableTimes;
        set => SetField(ref availableTimes, value);
    }
    
    public Domain.Model.Exam? SelectedExam
    {
        get => selectedExam;
        set => SetField(ref selectedExam, value);
    }
    
    public Language? Language
    {
        get => language;
        set
        {
            if (Equals(language, value)) return;
            language = value;
            OnPropertyChanged(nameof(language));
            LanguageLevel = null;
            LanguageLevels = language == null ? new ObservableCollection<LanguageLevel>() : new ObservableCollection<LanguageLevel>(languageToLvl[language]);
        }
    }
    public LanguageLevel? LanguageLevel
    {
        get => _languageLevel;
        set => SetField(ref _languageLevel, value);
    }
    public DateTime? ExamDate
    {
        get => examDate;
        set
        {
            if (examDate == value) return;
            examDate = value;
            OnPropertyChanged(nameof(examDate));
            ExamTime = null;
            if (examDate == null)
            {
                AvailableTimes = new ObservableCollection<TimeOnly>();
                return;
            }
            var times = _timetableService.GetAvailableExamTimes(DateOnly.FromDateTime(examDate.Value), _tutor);
            AvailableTimes = new ObservableCollection<TimeOnly>(times);
        }
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
    public Domain.Model.Exam.State ExamState
    {
        get => examState;
        set => SetField(ref examState, value);
    }
    
    public Language? FilterLanguage
    {
        get => filterLanguage;
        set {
            SetField(ref filterLanguage, value);
            FilterExams();
        }
    }
    
    public LanguageLevel? FilterLanguageLvl
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
    
    public ExamOverviewViewModel(IAuthenticationStore authenticationStore, IExamService examService, ITimetableService timetableService, CurrentExamStore currentExamStore, IPopupNavigationService popupNavigationService, NavigationStore navigationStore)
    {
        _tutor = (Domain.Model.Tutor?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create ExamViewModel without currently logged in tutor");
        _examService = examService;
        _timetableService = timetableService;
        _currentExamStore = currentExamStore;
        _popupNavigationService = popupNavigationService;
        _navigationStore = navigationStore;
        exams = new ObservableCollection<Domain.Model.Exam>(LoadExams());
        languages = new ObservableCollection<Language>(_tutor.KnownLanguages.Select(tuple => tuple.Item1));
        languageLevels = new ObservableCollection<LanguageLevel>();
        filterLanguageLevels = new ObservableCollection<LanguageLevel>(Enum.GetValues<LanguageLevel>());
        
        languageToLvl = new Dictionary<Language, List<LanguageLevel>>();
        foreach (var knownLanguage in _tutor.KnownLanguages)
        {
            var levels = new List<LanguageLevel>();
            foreach (LanguageLevel lvl in Enum.GetValues(typeof(LanguageLevel)))
            {
                if (lvl > knownLanguage.Item2) break;
                levels.Add(lvl);
            }
            languageToLvl.Add(knownLanguage.Item1, levels);
        }
        
        availableTimes = new ObservableCollection<TimeOnly>();
        
        AddCommand = new RelayCommand(execute => AddExam(), execute => CanAddExam());
        SelectedExamChangedCommand = new RelayCommand(execute => SelectExam());
        UpdateCommand = new RelayCommand(execute => UpdateExam(), execute => CanUpdateExam());
        DeleteCommand = new RelayCommand(execute => DeleteExam(), execute => CanDeleteExam());
        ClearFiltersCommand = new RelayCommand(execute => ClearFilters(), execute => CanClearFilters());
        OpenExamInfoCommand = new RelayCommand(OpenExamInfo, canExecute => SelectedExam != null);
    }

    private int GetClassroomNumber()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        if (selectedDate != null && ExamTime != null)
        {
            var classrooms = _timetableService.GetAvailableClassrooms(selectedDate.Value, ExamTime.Value, Constants.ExamDuration, _tutor);
            if (classrooms.Count > 0)
            {
                return classrooms[0];
            }
        }

        return -1;
    }
    
    private void AddExam()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        int classroomNumber = GetClassroomNumber();
        try
        {
            Domain.Model.Exam exam = _examService.AddExam(_tutor, Language, LanguageLevel, selectedDate, ExamTime, classroomNumber, MaxStudents);
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
        return Language != null && LanguageLevel != null && ExamDate != null && ExamTime != null && MaxStudents > 0;
    }
    
    private void ResetFields()
    {
        Language = null;
        LanguageLevel = null;
        ExamDate = null;
        ExamTime = null;
        MaxStudents = 0;
    }

    private void SelectExam()
    {
        if (SelectedExam == null) return;
        Language = SelectedExam.Language;
        LanguageLevel = SelectedExam.LanguageLevel;
        ExamDate = SelectedExam.Time.Date;
        ExamTime = SelectedExam.TimeOfDay;
        MaxStudents = SelectedExam.MaxStudents;
        NumStudents = SelectedExam.NumStudents;
        ExamState = SelectedExam.ExamState;
    }

    private void UpdateExam()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        try
        {
            Domain.Model.Exam exam = _examService.UpdateExam(selectedExam!.Id, Language, LanguageLevel, selectedDate, ExamTime, selectedExam!.ClassroomNumber, MaxStudents);
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
            _examService.DeleteExam(selectedExam!.Id);
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
        Exams = new ObservableCollection<Domain.Model.Exam>(_examService.FilterExams(LoadExams(), FilterLanguage, FilterLanguageLvl, filterDateOnly));
    }
    
    private List<Domain.Model.Exam> LoadExams()
    {
        return _examService.GetExamsByTutor(_tutor.Id);
    }
    
    private void ClearFilters()
    {
        FilterLanguage = null;
        FilterLanguageLvl = null;
        FilterDate = null;
    }
    
    private bool CanClearFilters()
    {
        return FilterLanguage != null || FilterLanguageLvl != null || FilterDate != null;
    }

    private void OpenExamInfo(object? obj)
    {
        _currentExamStore.CurrentExam = SelectedExam;
        switch (SelectedExam?.ExamState)
        {
            case Domain.Model.Exam.State.NotStarted:
            case Domain.Model.Exam.State.Locked:
                _popupNavigationService.Navigate(ViewType.UpcomingExamInfo);
                _navigationStore.PopupClosed += OnPopupClosed;
                break;
            case Domain.Model.Exam.State.InProgress:
                _popupNavigationService.Navigate(ViewType.ActiveExamInfo);
                _navigationStore.PopupClosed += OnPopupClosed;
                break;
            case Domain.Model.Exam.State.Finished:
                _popupNavigationService.Navigate(ViewType.FinishedExamInfo);
                _navigationStore.PopupClosed += OnPopupClosed;
                break;
        }
    }

    private void OnPopupClosed()
    {
        _navigationStore.PopupClosed -= OnPopupClosed;
    }
}