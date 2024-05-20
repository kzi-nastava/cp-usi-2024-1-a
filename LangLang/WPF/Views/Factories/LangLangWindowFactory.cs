using System;
using System.Windows;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.ViewModels.Director;
using LangLang.WPF.ViewModels.Student;
using LangLang.WPF.ViewModels.Tutor;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.ViewModels.Tutor.Exam;
using LangLang.WPF.Views.Common;
using LangLang.WPF.Views.Director;
using LangLang.WPF.Views.Student;
using LangLang.WPF.Views.Tutor;
using LangLang.WPF.Views.Tutor.Course;
using LangLang.WPF.Views.Tutor.Exam;

namespace LangLang.WPF.Views.Factories;

public class LangLangWindowFactory : ILangLangWindowFactory
{
    public Window CreateWindow(ViewModelBase viewModel)
    {
        return viewModel switch
        {
            LoginViewModel loginViewModel => new LoginWindow(loginViewModel, this),
            RegisterViewModel registerViewModel => new RegisterWindow(registerViewModel, this),
            StudentMenuViewModel studentViewModel => new StudentMenuWindow(studentViewModel, this),
            TutorMenuViewModel tutorViewModel => new TutorMenuWindow(tutorViewModel, this),
            DirectorMenuViewModel directorMenuViewModel => new DirectorMenuWindow(directorMenuViewModel, this),
            StudentAccountViewModel studentAccountViewModel => new StudentAccountWindow(studentAccountViewModel, this),
            NotificationListViewModel notificationViewModel => new NotificationListWindow(notificationViewModel, this),
            ActiveCourseViewModel activeCourseInfoViewModel => new ActiveCourseWindow(activeCourseInfoViewModel, this),
            UpcomingCourseViewModel upcomingCourseInfoViewModel => new UpcomingCourseWindow(upcomingCourseInfoViewModel,this),
            FinishedCourseViewModel finishedCourseInfoViewModel => new FinishedCourseWindow(finishedCourseInfoViewModel, this),
            ActiveExamViewModel activeExamInfoViewModel => new ActiveExamWindow(activeExamInfoViewModel, this),
            UpcomingExamViewModel upcomingExamInfoViewModel => new UpcomingExamWindow(upcomingExamInfoViewModel, this),
            FinishedExamViewModel finishedExamInfoViewModel => new FinishedExamWindow(finishedExamInfoViewModel, this),
            //TutorOverviewViewModel tutorOverviewViewModel => new TutorOverviewView(tutorOverviewViewModel, this),
            RateTutorViewModel rateTutorViewModel => new RateTutorWindow(rateTutorViewModel, this),
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel), viewModel,
                "No Window exists for the given ViewModel: " + viewModel.GetType())
        };
    }
}