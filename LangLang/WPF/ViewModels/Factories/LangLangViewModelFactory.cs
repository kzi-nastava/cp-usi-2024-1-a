using System;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.ViewModels.Director;
using LangLang.WPF.ViewModels.Student;
using LangLang.WPF.ViewModels.Tutor;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.ViewModels.Tutor.Exam;

namespace LangLang.WPF.ViewModels.Factories;

public class LangLangViewModelFactory : ILangLangViewModelFactory
{
    private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
    private readonly CreateViewModel<RegisterViewModel> _createRegisterViewModel;
    private readonly CreateViewModel<StudentMenuViewModel> _createStudentViewModel;
    private readonly CreateViewModel<TutorMenuViewModel> _createTutorViewModel;
    private readonly CreateViewModel<DirectorMenuViewModel> _createDirectorViewModel;
    private readonly CreateViewModel<CourseOverviewViewModel> _createCourseViewModel;
    private readonly CreateViewModel<ExamOverviewViewModel> _createExamViewModel;
    private readonly CreateViewModel<StudentAccountViewModel> _createStudentAccountViewModel;
    private readonly CreateViewModel<NotificationListViewModel> _createNotificationViewModel;
    private readonly CreateViewModel<ActiveCourseViewModel> _createActiveCourseInfoViewModel;
    private readonly CreateViewModel<UpcomingCourseViewModel> _createUpcomingCourseInfoViewModel;
    private readonly CreateViewModel<RateTutorViewModel> _createRateTutorViewModel;
    private readonly CreateViewModel<FinishedCourseViewModel> _createFinishedCourseInfoViewModel;
    private readonly CreateViewModel<TutorOverviewViewModel> _createTutorOverviewViewModel;
    private readonly CreateViewModel<ActiveExamViewModel> _createActiveExamInfoViewModel;
    private readonly CreateViewModel<UpcomingExamViewModel> _createUpcomingExamInfoViewModel;
    private readonly CreateViewModel<FinishedExamViewModel> _createFinishedExamInfoViewModel;

    public LangLangViewModelFactory(CreateViewModel<LoginViewModel> createLoginViewModel,
        CreateViewModel<RegisterViewModel> createRegisterViewModel,
        CreateViewModel<StudentMenuViewModel> createStudentViewModel,
        CreateViewModel<TutorMenuViewModel> createTutorViewModel,
        CreateViewModel<DirectorMenuViewModel> createDirectorViewModel,
        CreateViewModel<CourseOverviewViewModel> createCourseViewModel,
        CreateViewModel<ExamOverviewViewModel> createExamViewModel,
        CreateViewModel<StudentAccountViewModel> createStudentAccountViewModel,
        CreateViewModel<NotificationListViewModel> createNotificationViewModel,
        CreateViewModel<ActiveCourseViewModel> createActiveCourseInfoViewModel,
        CreateViewModel<UpcomingCourseViewModel> createUpcomingCourseInfoViewModel,
        CreateViewModel<RateTutorViewModel> createRateTutorViewModel,
        CreateViewModel<FinishedCourseViewModel> createFinishedCourseInfoViewModel,
        CreateViewModel<ActiveExamViewModel> createActiveExamInfoViewModel,
        CreateViewModel<UpcomingExamViewModel> createUpcomingExamInfoViewModel,
        CreateViewModel<TutorOverviewViewModel> createTutorOverviewViewModel
        CreateViewModel<FinishedExamViewModel> createFinishedExamInfoViewModel,
        )
    {
        _createLoginViewModel = createLoginViewModel;
        _createRegisterViewModel = createRegisterViewModel;
        _createStudentViewModel = createStudentViewModel;
        _createTutorViewModel = createTutorViewModel;
        _createDirectorViewModel = createDirectorViewModel;
        _createCourseViewModel = createCourseViewModel;
        _createExamViewModel = createExamViewModel;
        _createStudentAccountViewModel = createStudentAccountViewModel;
        _createNotificationViewModel = createNotificationViewModel;
        _createActiveCourseInfoViewModel = createActiveCourseInfoViewModel;
        _createUpcomingCourseInfoViewModel = createUpcomingCourseInfoViewModel;
        _createRateTutorViewModel = createRateTutorViewModel;
        _createFinishedCourseInfoViewModel = createFinishedCourseInfoViewModel;
        _createActiveExamInfoViewModel = createActiveExamInfoViewModel;
        _createUpcomingExamInfoViewModel = createUpcomingExamInfoViewModel;
        _createTutorOverviewViewModel = createTutorOverviewViewModel;
        _createFinishedExamInfoViewModel = createFinishedExamInfoViewModel;
    }

    public ViewModelBase CreateViewModel(ViewType viewType)
    {
        return viewType switch
        {
            ViewType.Login => _createLoginViewModel(),
            ViewType.Register => _createRegisterViewModel(),
            ViewType.Student => _createStudentViewModel(),
            ViewType.Tutor => _createTutorViewModel(),
            ViewType.Director => _createDirectorViewModel(),
            ViewType.Course => _createCourseViewModel(),
            ViewType.Exam => _createExamViewModel(),
            ViewType.StudentAccount => _createStudentAccountViewModel(),
            ViewType.Notifications => _createNotificationViewModel(),
            ViewType.ActiveCourseInfo => _createActiveCourseInfoViewModel(),
            ViewType.UpcomingCourseInfo => _createUpcomingCourseInfoViewModel(),
            ViewType.RateTutor => _createRateTutorViewModel(),
            ViewType.FinishedCourseInfo => _createFinishedCourseInfoViewModel(),
            ViewType.ActiveExamInfo => _createActiveExamInfoViewModel(),
            ViewType.UpcomingExamInfo => _createUpcomingExamInfoViewModel(),
            ViewType.FinishedExamInfo => _createFinishedExamInfoViewModel(),
            ViewType.TutorTable => _createTutorOverviewViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, "No ViewModel exists for the given ViewType: " + viewType)
        };
    }
}