using System.Collections.ObjectModel;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;

namespace LangLang.ViewModel;

internal class ExamViewModel : ViewModelBase
{
    private ExamService examService;
    
    public RelayCommand AddCommand { get; set; }

    private ObservableCollection<Exam> exams;
    private Exam? selectedExam;

    public ObservableCollection<Exam> Exams
    {
        get => exams;
        set => SetField(ref exams, value);
    }

    public Exam? SelectedExam
    {
        get => selectedExam;
        set => SetField(ref selectedExam, value);
    }
    
    public ExamViewModel(ExamService examService)
    {
        this.examService = examService;
        exams = new ObservableCollection<Exam>(examService.GetAllExams());
        AddCommand = new RelayCommand(execute => AddExam(), execute => CanAddExam());
    }
    
    
    private void AddExam()
    {
        examService.AddExam(selectedExam!);
    }

    private bool CanAddExam()
    {
        return SelectedExam != null;
    }
}