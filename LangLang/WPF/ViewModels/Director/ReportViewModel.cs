using LangLang.Application.Stores;
using LangLang.Application.UseCases.Report;
using LangLang.Application.UseCases.User;
using LangLang.WPF.MVVM;
using System;
using System.Runtime.CompilerServices;
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
    public ICommand SendLanguageReportCommand { get; }

    public ReportViewModel(IReportCoordinator reportCoordinator, IAuthenticationStore authenticationStore, IAccountService accountService)
    {
        _reportCoordinator = reportCoordinator;
        SendCoursePenaltyReportCommand = new RelayCommand(execute => SendCongratulationsEmail());
        SendAverageCourseScoreCommand = new RelayCommand(execute => SendAverageCourseScoreEmail());
        SendPointsBySkillReportCommand = new RelayCommand(execute => SendPointsBySkillReportEmail());
        SendLanguageReportCommand = new RelayCommand(execute => SendLanguageReport());
        Domain.Model.Director _loggedInUser = (Domain.Model.Director?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create ReportViewModel without currently logged in director");
        loggedInDirectorEmail = accountService.GetEmailByUserId(_loggedInUser.Id);
    }

    private void SendPointsBySkillReportEmail()
        => SendReport(_reportCoordinator.SendPointsBySkillReport);
    private void SendCongratulationsEmail()
        => SendReport(_reportCoordinator.SendCoursePenaltyReport);
    private void SendAverageCourseScoreEmail()
        => SendReport(_reportCoordinator.SendAverageCoursePointsReport);
    private void SendLanguageReport()
        => SendReport(_reportCoordinator.SendLanguageReport);

    private delegate void SenderFunction(string recipient);
    private void SendReport(SenderFunction senderFunction)
    {
        try
        {
            senderFunction(loggedInDirectorEmail);
            MessageBox.Show($"The report has been sent to your email!", "Success");
        }
        catch
        {
            MessageBox.Show($"There was an error sending the email. Please examine the validity of the email you use for loggin.", "Fail");
        }
    }
}
