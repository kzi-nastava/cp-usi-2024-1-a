using LangLang.Application.UseCases.Report;
using LangLang.WPF.MVVM;
using System.Windows;
using System.Windows.Input;

namespace LangLang.WPF.ViewModels.Director;

public class ReportViewModel : ViewModelBase
{
    private readonly IReportCoordinator _reportCoordinator;
    public ICommand SendCoursePenaltyReportCommand { get; }

    public ReportViewModel(IReportCoordinator reportCoordinator)
    {
        _reportCoordinator = reportCoordinator;
        SendCoursePenaltyReportCommand = new RelayCommand<string>(SendCongratulationsEmail);
    }

    private void SendCongratulationsEmail(string courseId)
    {
        try
        {
            _reportCoordinator.SendCoursePenaltyReport(courseId);   //RECIPIENT
            MessageBox.Show($"Congratulationary emails have been sent to the best students that attended !", "Success");
        }
        catch
        {
            MessageBox.Show($"There were no students attending this course. No emails were sent.", "Fail");
        }
    }
}
