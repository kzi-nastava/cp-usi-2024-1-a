using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using LangLang.Application.UseCases.Course;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Course;

namespace LangLang.WPF.ViewModels.Director;

public class FinishedCourseOverviewForDirectorViewModel : ViewModelBase
{
    private readonly IStudentCourseCoordinator _courseCoordinator;
    private readonly ICourseService _courseService;
    private readonly IBestStudentsByCourseService _bestStudentsByCourseService;
    public ObservableCollection<string?> Priorities { get; set; }
    private string priorityPicker = "";
    public string PriorityPicker
    {
        get => priorityPicker;
        set
        {
            priorityPicker = value;
            OnPropertyChanged();
        }
    }

    public ICommand SendCongratulationsEmailCommand { get; }
    public ObservableCollection<CourseViewModel> Courses { get; set; }

    public FinishedCourseOverviewForDirectorViewModel(IBestStudentsByCourseService bestStudentsByCourseService, IStudentCourseCoordinator studentCourseCoordinator, ICourseService courseService)
    {
        _courseCoordinator = studentCourseCoordinator;     
        _courseService = courseService;
        _bestStudentsByCourseService = bestStudentsByCourseService;
        Courses = new ObservableCollection<CourseViewModel>();
        Priorities = new();
        SendCongratulationsEmailCommand = new RelayCommand<string>(SendCongratulationsEmail);
        LoadCourses();
        LoadPriorities();
    }

    private void SendCongratulationsEmail(string courseId)
    {
        try
        {
            if(PriorityPicker == "Activity grade")
            {
                _bestStudentsByCourseService.SendEmailToBestStudents(courseId, Application.Utility.BestStudentsConstants.GradingPriority.ActivityGrade);
            }else if(PriorityPicker == "Knowledge grade")
            {
                _bestStudentsByCourseService.SendEmailToBestStudents(courseId, Application.Utility.BestStudentsConstants.GradingPriority.KnowledgeGrade);
            }
            else
            {
                _bestStudentsByCourseService.SendEmailToBestStudents(courseId, Application.Utility.BestStudentsConstants.GradingPriority.EqualPriority);
            }

            string courseName = _courseService.GetCourseById(courseId)!.Name;
            MessageBox.Show($"Congratulationary emails have been sent to the {Application.Utility.BestStudentsConstants.NumOfBestStudents} best students that attended {courseName}!", "Success");
        }
        catch
        {
            MessageBox.Show($"There were no students attending this course. No emails were sent.", "Fail");
        }
    }
    public void LoadPriorities()
    {
        Priorities.Add("Equal priority");
        Priorities.Add("Activity grade");
        Priorities.Add("Knowledge grade");
    }

    public void LoadCourses()
    {
        var finishedCourses = _courseCoordinator.GetFinishedAndGradedCourses();
        foreach (Course course in finishedCourses)
        {
            Courses.Add(new CourseViewModel(course));
        }
    }
}