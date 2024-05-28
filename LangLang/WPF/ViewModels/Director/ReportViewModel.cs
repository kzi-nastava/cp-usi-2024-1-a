using LangLang.Application.Stores;
using LangLang.Application.UseCases.Report;
using LangLang.Application.UseCases.User;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using System;
using System.Windows;
using System.Windows.Input;

namespace LangLang.WPF.ViewModels.Director;

public class ReportViewModel : ViewModelBase
{
    private readonly IReportCoordinator _reportCoordinator;
    private readonly string loggedInDirectorEmail;

    public ICommand SendCoursePenaltyReportCommand { get; }
    public ICommand SendAverageCourseScoreCommand { get; }
    public ICommand SendPointsBySkillReportCommand { get; }

    public ReportViewModel(IReportCoordinator reportCoordinator, IAuthenticationStore authenticationStore, IAccountService accountService)
    {
        
        _reportCoordinator = reportCoordinator;
        SendCoursePenaltyReportCommand = new RelayCommand(execute => SendCongratulationsEmail());
        SendAverageCourseScoreCommand = new RelayCommand(execute => SendAverageCourseScoreEmail());
        SendPointsBySkillReportCommand = new RelayCommand(execute => SendPointsBySkillReportEmail());
        Domain.Model.Director _loggedInUser = (Domain.Model.Director?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create ReportViewModel without currently logged in director");
        loggedInDirectorEmail = accountService.GetEmailByUserId(_loggedInUser.Id, UserType.Director);
    }

    private void SendPointsBySkillReportEmail()
    {
        try
        {
            _reportCoordinator.SendPointsBySkillReport(loggedInDirectorEmail);
            MessageBox.Show($"The report has been sent to your email!", "Success");
        }
        catch
        {
            MessageBox.Show($"There was an error sending the email. Please examine the validity of the email you use for loggin.", "Fail");
        }
    }

    private void SendAverageCourseScoreEmail()
    {
        try
        {
            _reportCoordinator.SendAverageCoursePointsReport(loggedInDirectorEmail);
            MessageBox.Show($"The report has been sent to your email!", "Success");
        }
        catch
        {
            MessageBox.Show($"There was an error sending the email. Please examine the validity of the email you use for loggin.", "Fail");
        }
    }

    private void SendCongratulationsEmail()
    {
        try
        {
            _reportCoordinator.SendCoursePenaltyReport(loggedInDirectorEmail);
            MessageBox.Show($"The report has been sent to your email!", "Success");
        }
        catch
        {
            MessageBox.Show($"There was an error sending the email. Please examine the validity of the email you use for loggin.", "Fail");
        }
    }
}
