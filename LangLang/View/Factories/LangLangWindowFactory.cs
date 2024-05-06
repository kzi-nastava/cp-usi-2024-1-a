using System;
using System.Windows;
using LangLang.MVVM;
using LangLang.ViewModel;


namespace LangLang.View.Factories;

public class LangLangWindowFactory : ILangLangWindowFactory
{
    public Window CreateWindow(ViewModelBase viewModel)
    {
        return viewModel switch
        {
            LoginViewModel loginViewModel => new LoginWindow(loginViewModel, this),
            RegisterViewModel registerViewModel => new RegisterWindow(registerViewModel, this),
            StudentViewModel studentViewModel => new StudentWindow(studentViewModel, this),
            TutorViewModel tutorViewModel => new TutorWindow(tutorViewModel, this),
            DirectorViewModel directorViewModel => new DirectorWindow(directorViewModel, this),
            StudentAccountViewModel studentAccountViewModel => new StudentAccountWindow(studentAccountViewModel, this),
            NotificationViewModel notificationViewModel => new NotificationWindow(notificationViewModel, this),
            ActiveCourseInfoViewModel activeCourseInfoViewModel => new ActiveCourseInfoWindow(activeCourseInfoViewModel, this),
            UpcomingCourseInfoViewModel upcomingCourseInfoViewModel => new UpcomingCourseInfoWindow(upcomingCourseInfoViewModel,this),
            FinishedCourseInfoViewModel finishedCourseInfoViewModel => new FinishedCourseInfoWindow(finishedCourseInfoViewModel, this),
            TutorTableViewModel tutorTableViewModel => new TutorTableWindow(tutorTableViewModel, this),
            RateTutorViewModel rateTutorViewModel => new RateTutorWindow(rateTutorViewModel, this),
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel), viewModel,
                "No Window exists for the given ViewModel: " + viewModel.GetType())
        };
    }
}