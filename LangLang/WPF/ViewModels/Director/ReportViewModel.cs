using LangLang.Application.Stores;
using LangLang.Application.UseCases.Report;
using LangLang.Application.UseCases.User;
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

    public ReportViewModel(IReportCoordinator reportCoordinator, IAuthenticationStore authenticationStore, IAccountService accountService)
    {
        _reportCoordinator = reportCoordinator;
        SendCoursePenaltyReportCommand = new RelayCommand(execute => SendCongratulationsEmail());
        Domain.Model.Director _loggedInUser = (Domain.Model.Director?)authenticationStore.CurrentUser.Person ??
                                throw new InvalidOperationException(
                                    "Cannot create ReportViewModel without currently logged in director");
        loggedInDirectorEmail = accountService.GetEmailByUserId(_loggedInUser.Id);
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
