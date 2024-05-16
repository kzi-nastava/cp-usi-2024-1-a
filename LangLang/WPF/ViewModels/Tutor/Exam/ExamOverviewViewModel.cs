using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.Utility.Navigation;
using LangLang.Application.Utility.Timetable;
using LangLang.Domain;
using LangLang.Domain.Model;
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

    private ObservableCollection<ExamViewModel> _exams;
    private ObservableCollection<Language> _languages;
    private ObservableCollection<LanguageLevel> _languageLevels;
    private ObservableCollection<LanguageLevel> _filterLanguageLevels;
    private ObservableCollection<TimeOnly> _availableTimes;

    private ExamViewModel? _selectedExam;

    private Language? _language;
    private LanguageLevel? _languageLevel;
    private DateTime? _examDate;
    private TimeOnly? _examTime;
    private int _maxStudents;
    private int _numStudents;
    private Domain.Model.Exam.State _examState;

    private Language? _filterLanguage;
    private LanguageLevel? _filterLanguageLvl;
    private DateTime? _filterDate;

    private readonly Dictionary<Language, List<LanguageLevel>> _languageToLvl;

    public ObservableCollection<ExamViewModel> Exams
    {
        get => _exams;
        private set => SetField(ref _exams, value);
    }

    public ObservableCollection<Language> Languages
    {
        get => _languages;
        set => SetField(ref _languages, value);
    }

    public ObservableCollection<LanguageLevel> LanguageLevels
    {
        get => _languageLevels;
        private set => SetField(ref _languageLevels, value);
    }

    public ObservableCollection<LanguageLevel> FilterLanguageLevels
    {
        get => _filterLanguageLevels;
        set => SetField(ref _filterLanguageLevels, value);
    }

    public ObservableCollection<TimeOnly> AvailableTimes
    {
        get => _availableTimes;
        private set => SetField(ref _availableTimes, value);
    }

    public ExamViewModel? SelectedExam
    {
        get => _selectedExam;
        set => SetField(ref _selectedExam, value);
    }

    public Language? Language
    {
        get => _language;
        set
        {
            if (!SetField(ref _language, value)) return;
            LanguageLevel = null;
            LanguageLevels = _language == null
                ? new ObservableCollection<LanguageLevel>()
                : new ObservableCollection<LanguageLevel>(_languageToLvl[_language]);
        }
    }

    public LanguageLevel? LanguageLevel
    {
        get => _languageLevel;
        set => SetField(ref _languageLevel, value);
    }

    public DateTime? ExamDate
    {
        get => _examDate;
        set
        {
            if (!SetField(ref _examDate, value)) return;
            ExamTime = null;
            if (_examDate == null)
            {
                AvailableTimes = new ObservableCollection<TimeOnly>();
                return;
            }

            var times = _timetableService.GetAvailableExamTimes(DateOnly.FromDateTime(_examDate.Value), _tutor);
            AvailableTimes = new ObservableCollection<TimeOnly>(times);
        }
    }

    public TimeOnly? ExamTime
    {
        get => _examTime;
        set => SetField(ref _examTime, value);
    }

    public int MaxStudents
    {
        get => _maxStudents;
        set => SetField(ref _maxStudents, value);
    }

    public int NumStudents
    {
        get => _numStudents;
        set => SetField(ref _numStudents, value);
    }

    public Domain.Model.Exam.State ExamState
    {
        get => _examState;
        private set => SetField(ref _examState, value);
    }

    public Language? FilterLanguage
    {
        get => _filterLanguage;
        set
        {
            SetField(ref _filterLanguage, value);
            FilterExams();
        }
    }

    public LanguageLevel? FilterLanguageLvl
    {
        get => _filterLanguageLvl;
        set
        {
            SetField(ref _filterLanguageLvl, value);
            FilterExams();
        }
    }

    public DateTime? FilterDate
    {
        get => _filterDate;
        set
        {
            SetField(ref _filterDate, value);
            FilterExams();
        }
    }

    public ExamOverviewViewModel(IAuthenticationStore authenticationStore, IExamService examService,
        ITimetableService timetableService, CurrentExamStore currentExamStore,
        IPopupNavigationService popupNavigationService, NavigationStore navigationStore)
    {
        _tutor = (Domain.Model.Tutor?)authenticationStore.CurrentUser.Person ??
                 throw new InvalidOperationException(
                     "Cannot create ExamViewModel without currently logged in tutor");
        _examService = examService;
        _timetableService = timetableService;
        _currentExamStore = currentExamStore;
        _popupNavigationService = popupNavigationService;
        _navigationStore = navigationStore;
        _exams = new ObservableCollection<ExamViewModel>(LoadExams());
        _languages = new ObservableCollection<Language>(_tutor.KnownLanguages.Select(tuple => tuple.Item1));
        _languageLevels = new ObservableCollection<LanguageLevel>();
        _filterLanguageLevels = new ObservableCollection<LanguageLevel>(Enum.GetValues<LanguageLevel>());

        _languageToLvl = new Dictionary<Language, List<LanguageLevel>>();
        foreach (var knownLanguage in _tutor.KnownLanguages)
        {
            var levels = new List<LanguageLevel>();
            foreach (LanguageLevel lvl in Enum.GetValues(typeof(LanguageLevel)))
            {
                if (lvl > knownLanguage.Item2) break;
                levels.Add(lvl);
            }

            _languageToLvl.Add(knownLanguage.Item1, levels);
        }

        _availableTimes = new ObservableCollection<TimeOnly>();

        AddCommand = new RelayCommand(_ => AddExam(), _ => CanAddExam());
        SelectedExamChangedCommand = new RelayCommand(_ => SelectExam());
        UpdateCommand = new RelayCommand(_ => UpdateExam(), _ => CanUpdateExam());
        DeleteCommand = new RelayCommand(_ => DeleteExam(), _ => CanDeleteExam());
        ClearFiltersCommand = new RelayCommand(_ => ClearFilters(), _ => CanClearFilters());
        OpenExamInfoCommand = new RelayCommand(OpenExamInfo, _ => SelectedExam != null);
    }

    private int GetClassroomNumber()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        if (selectedDate != null && ExamTime != null)
        {
            var classrooms = _timetableService.GetAvailableClassrooms(selectedDate.Value, ExamTime.Value,
                Constants.ExamDuration, _tutor);
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
        var classroomNumber = GetClassroomNumber();
        try
        {
            var exam = _examService.AddExam(new ExamDto(null, Language, LanguageLevel, selectedDate, ExamTime,
                classroomNumber, MaxStudents, _tutor));
            Exams.Add(new ExamViewModel(exam));
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
        var exam = _examService.GetExamById(SelectedExam.Id);
        if (exam == null) return;

        Language = exam.Language;
        LanguageLevel = exam.LanguageLevel;
        ExamDate = exam.Time.Date;
        ExamTime = exam.TimeOfDay;
        MaxStudents = exam.MaxStudents;
        NumStudents = exam.NumStudents;
        ExamState = exam.ExamState;
    }

    private void UpdateExam()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        int classroomNumber = GetClassroomNumber();

        var exam = _examService.GetExamById(SelectedExam!.Id);
        if (exam == null) return;

        try
        {
            exam = _examService.UpdateExam(new ExamDto(_selectedExam!.Id, Language, LanguageLevel, selectedDate,
                ExamTime, classroomNumber, MaxStudents));
            Exams[Exams.IndexOf(SelectedExam!)] = new ExamViewModel(exam);
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
            _examService.DeleteExam(_selectedExam!.Id);
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
        Exams = new ObservableCollection<ExamViewModel>(ConvertToExamViewModel(_examService.FilterExams(FilterLanguage,
            FilterLanguageLvl, filterDateOnly)));
    }

    private List<ExamViewModel> LoadExams()
    {
        return ConvertToExamViewModel(_examService.GetExamsByTutor(_tutor.Id));
    }

    private List<ExamViewModel> ConvertToExamViewModel(List<Domain.Model.Exam> exams)
    {
        return exams.Select(exam => new ExamViewModel(exam)).ToList();
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
        var exam = SelectedExam == null ? null : _examService.GetExamById(SelectedExam.Id);
        _currentExamStore.CurrentExam = exam;
        switch (exam?.ExamState)
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