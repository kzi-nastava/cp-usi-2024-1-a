using System;
using LangLang.MVVM;

namespace LangLang.ViewModel.Factories;

public class LangLangViewModelFactory : ILangLangViewModelFactory
{
    private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
    private readonly CreateViewModel<RegisterViewModel> _createRegisterViewModel;
    private readonly CreateViewModel<StudentViewModel> _createStudentViewModel;
    private readonly CreateViewModel<TutorViewModel> _createTutorViewModel;
    private readonly CreateViewModel<DirectorViewModel> _createDirectorViewModel;
    private readonly CreateViewModel<CourseViewModel> _createCourseViewModel;
    private readonly CreateViewModel<ExamViewModel> _createExamViewModel;
    private readonly CreateViewModel<StudentAccountViewModel> _createStudentAccountViewModel;
    private readonly CreateViewModel<NotificationViewModel> _createNotificationViewModel;
    private readonly CreateViewModel<ActiveCourseInfoViewModel> _createActiveCourseInfoViewModel;
    private readonly CreateViewModel<UpcomingCourseInfoViewModel> _createUpcomingCourseInfoViewModel;
    private readonly CreateViewModel<RateTutorViewModel> _createRateTutorViewModel;
    private readonly CreateViewModel<FinishedCourseInfoViewModel> _createFinishedCourseInfoViewModel;
    private readonly CreateViewModel<TutorTableViewModel> _createTutorTableViewModel;
    private readonly CreateViewModel<ActiveExamInfoViewModel> _createActiveExamInfoViewModel;
    private readonly CreateViewModel<UpcomingExamInfoViewModel> _createUpcomingExamInfoViewModel;
    private readonly CreateViewModel<FinishedExamInfoViewModel> _createFinishedExamInfoViewModel;

    public LangLangViewModelFactory(CreateViewModel<LoginViewModel> createLoginViewModel,
        CreateViewModel<RegisterViewModel> createRegisterViewModel,
        CreateViewModel<StudentViewModel> createStudentViewModel,
        CreateViewModel<TutorViewModel> createTutorViewModel,
        CreateViewModel<DirectorViewModel> createDirectorViewModel,
        CreateViewModel<CourseViewModel> createCourseViewModel,
        CreateViewModel<ExamViewModel> createExamViewModel,
        CreateViewModel<StudentAccountViewModel> createStudentAccountViewModel,
        CreateViewModel<NotificationViewModel> createNotificationViewModel,
        CreateViewModel<ActiveCourseInfoViewModel> createActiveCourseInfoViewModel,
        CreateViewModel<UpcomingCourseInfoViewModel> createUpcomingCourseInfoViewModel,
        CreateViewModel<RateTutorViewModel> createRateTutorViewModel,
        CreateViewModel<FinishedCourseInfoViewModel> createfinishedCourseInfoViewModel,
        CreateViewModel<ActiveExamInfoViewModel> createActiveExamInfoViewModel,
        CreateViewModel<UpcomingExamInfoViewModel> createUpcomingExamInfoViewModel,
        CreateViewModel<FinishedExamInfoViewModel> createfinishedExamInfoViewModel,
        CreateViewModel<TutorTableViewModel> createTutorTableViewModel
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
        _createFinishedCourseInfoViewModel = createfinishedCourseInfoViewModel;
        _createActiveExamInfoViewModel = createActiveExamInfoViewModel;
        _createUpcomingExamInfoViewModel = createUpcomingExamInfoViewModel;
        _createFinishedExamInfoViewModel = createfinishedExamInfoViewModel;

        _createTutorTableViewModel = createTutorTableViewModel;
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
            ViewType.TutorTable => _createTutorTableViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, "No ViewModel exists for the given ViewType: " + viewType)
        };
    }
}