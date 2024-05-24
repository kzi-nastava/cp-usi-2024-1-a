using System;
using System.Collections.ObjectModel;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Navigation;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Course;

namespace LangLang.WPF.ViewModels.Director;

public class FinishedCourseOverviewForDirectorViewModel : ViewModelBase
{
    private readonly ICourseService _courseService;
    private readonly IPopupNavigationService _popupNavigationService;
    private readonly IStudentCourseCoordinator _courseCoordinator;
    public ObservableCollection<CourseViewModel> Courses { get; set; }

    public FinishedCourseOverviewForDirectorViewModel(ICourseService courseService, IPopupNavigationService popupNavigationService, IStudentCourseCoordinator studentCourseCoordinator)
    {
        _courseService = courseService;
        _popupNavigationService = popupNavigationService;
        _courseCoordinator = studentCourseCoordinator;       
        Courses = new ObservableCollection<CourseViewModel>();
        
        LoadCourses();
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