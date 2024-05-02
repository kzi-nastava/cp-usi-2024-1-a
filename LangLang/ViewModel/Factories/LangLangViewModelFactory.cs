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
    private readonly CreateViewModel<CourseInfoViewModel> _createCourseInfoViewModel;
    private readonly CreateViewModel<NotificationViewModel> _createNotificationViewModel;

    public LangLangViewModelFactory(CreateViewModel<LoginViewModel> createLoginViewModel,
        CreateViewModel<RegisterViewModel> createRegisterViewModel, 
        CreateViewModel<StudentViewModel> createStudentViewModel,
        CreateViewModel<TutorViewModel> createTutorViewModel,
        CreateViewModel<DirectorViewModel> createDirectorViewModel,
        CreateViewModel<CourseViewModel> createCourseViewModel,
        CreateViewModel<ExamViewModel> createExamViewModel,
        CreateViewModel<StudentAccountViewModel> createStudentAccountViewModel,
        CreateViewModel<CourseInfoViewModel> createCourseInfoViewModel, 
        CreateViewModel<NotificationViewModel> createNotificationViewModel)
    {
        _createLoginViewModel = createLoginViewModel;
        _createRegisterViewModel = createRegisterViewModel;
        _createStudentViewModel = createStudentViewModel;
        _createTutorViewModel = createTutorViewModel;
        _createDirectorViewModel = createDirectorViewModel;
        _createCourseViewModel = createCourseViewModel;
        _createExamViewModel = createExamViewModel;
        _createStudentAccountViewModel = createStudentAccountViewModel;
        _createCourseInfoViewModel = createCourseInfoViewModel;
        _createNotificationViewModel = createNotificationViewModel;
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
            ViewType.CourseInfo => _createCourseInfoViewModel(),
            ViewType.Notifications => _createNotificationViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, "No ViewModel exists for the given ViewType: " + viewType)
        };
    }
}