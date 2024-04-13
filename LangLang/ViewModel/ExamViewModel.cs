﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;

namespace LangLang.ViewModel;

internal class ExamViewModel : ViewModelBase
{
    private Tutor tutor;
    
    private ExamService examService;
    private TimetableService timetableService;
    
    public RelayCommand AddCommand { get; set; }
    public RelayCommand SelectedExamChangedCommand { get; set; }
    public RelayCommand UpdateCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand ClearFiltersCommand { get; set; }

    private ObservableCollection<Exam> exams;
    private ObservableCollection<Language> languages;
    private ObservableCollection<LanguageLvl> languageLevels;
    private ObservableCollection<LanguageLvl> filterLanguageLevels;
    private ObservableCollection<TimeOnly> availableTimes;
    
    private Exam? selectedExam;

    private Language? language;
    private LanguageLvl? languageLvl;
    private DateTime? examDate;
    private TimeOnly? examTime;
    private int maxStudents;
    private int numStudents;
    private Exam.State examState;
    
    private Language? filterLanguage;
    private LanguageLvl? filterLanguageLvl;
    private DateTime? filterDate;
    
    private Dictionary<Language, List<LanguageLvl>> languageToLvl;

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
    public ObservableCollection<LanguageLvl> FilterLanguageLevels
    {
        get => filterLanguageLevels;
        set => SetField(ref filterLanguageLevels, value);
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
        set
        {
            if (Equals(language, value)) return;
            language = value;
            OnPropertyChanged(nameof(language));
            LanguageLvl = null;
            LanguageLevels = language == null ? new ObservableCollection<LanguageLvl>() : new ObservableCollection<LanguageLvl>(languageToLvl[language]);
        }
    }
    public LanguageLvl? LanguageLvl
    {
        get => languageLvl;
        set => SetField(ref languageLvl, value);
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
            var times = timetableService.GetAvailableExamTimes(DateOnly.FromDateTime(examDate.Value), tutor);
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
    public Exam.State ExamState
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
    
    public ExamViewModel(Tutor tutor, ExamService examService, TimetableService timetableService)
    {
        this.tutor = tutor;
        this.examService = examService;
        this.timetableService = timetableService;
        
        exams = new ObservableCollection<Exam>(LoadExams());
        languages = new ObservableCollection<Language>(tutor.KnownLanguages.Select(tuple => tuple.Item1));
        languageLevels = new ObservableCollection<LanguageLvl>();
        filterLanguageLevels = new ObservableCollection<LanguageLvl>(Enum.GetValues<LanguageLvl>());
        
        languageToLvl = new Dictionary<Language, List<LanguageLvl>>();
        foreach (var knownLanguage in tutor.KnownLanguages)
        {
            var levels = new List<LanguageLvl>();
            foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
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
    }

    private int GetClassroomNumber()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        if (selectedDate != null && ExamTime != null)
        {
            var classrooms = timetableService.GetAvailableClassrooms(selectedDate.Value, ExamTime.Value, Constants.ExamDuration, tutor);
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
            Exam exam = examService.AddExam(tutor, Language, LanguageLvl, selectedDate, ExamTime, classroomNumber, MaxStudents);
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
        ExamState = SelectedExam.ExamState;
    }

    private void UpdateExam()
    {
        DateOnly? selectedDate = ExamDate != null ? DateOnly.FromDateTime(ExamDate!.Value) : null;
        try
        {
            Exam exam = examService.UpdateExam(selectedExam!.Id, Language, LanguageLvl, selectedDate, ExamTime, selectedExam!.ClassroomNumber, MaxStudents);
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
        return examService.GetExamsByTutor(tutor.Email);
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
}