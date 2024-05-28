using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Email;
using LangLang.Domain;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Tutor.Exam;

namespace LangLang.WPF.ViewModels.Director;

public class FinishedExamOverviewViewModel : ViewModelBase
{
    private readonly IExamCoordinator _examCoordinator;
    private readonly IExamService _examService;
    private readonly IAccountService _accountService;
    private readonly IExamResultsService _examResultsService;

    public ICommand SendResultsEmailsCommand { get; }
    public ObservableCollection<ExamViewModel> Exams { get; set; }

    public FinishedExamOverviewViewModel(IExamCoordinator examCoordinator, IExamService examService, IAccountService accountService, IExamResultsService examResultsService)
    {
        _examCoordinator = examCoordinator;     
        _examService = examService;
        _accountService = accountService;
        _examResultsService = examResultsService;
        Exams = new ObservableCollection<ExamViewModel>();
        SendResultsEmailsCommand = new RelayCommand<ExamViewModel>(SendResultsEmails);
        LoadExams();
    }

    private void SendResultsEmails(ExamViewModel examViewModel)
    {
        if (_examService.GetExamById(examViewModel.Id)!.IsReported())
        {
            System.Windows.MessageBox.Show("Exam is already reported.", "Fail");
            return;
        }
        var sendingResult = _examResultsService.SendExamResult(examViewModel);
        string failed = "";
        if (sendingResult.FailedToSend.Count > 0)
        {
            failed = "Following students couldn't be reached:\n";
            failed += string.Join(",\n", sendingResult.FailedToSend.Select(student => Format(student)));
            failed += '.';
        }
        
        if (sendingResult.SuccessfullySent.Count == 0)
        {
            string message = "Unable to reach any students. Do you still want to mark exam as reported? ";
            message += failed;
            var messageBoxResult = System.Windows.MessageBox.Show(message, "Fail", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes)
                return;
        }
        else
        {
            string message = $"Successfully sent results to {sendingResult.SuccessfullySent.Count} students. ";
            message += failed;
            System.Windows.MessageBox.Show(message, "Results sent");
        }
        _examCoordinator.ReportedExam(_examService.GetExamById(examViewModel.Id)!, 
            sendingResult.SuccessfullySent.Select(person => (Domain.Model.Student)person).ToList());
    }

    private string Format(Person student)
    {
        string email = _accountService.GetEmailByUserId(((Domain.Model.Student)student).Id, UserType.Student);
        return $"{student.Name} {student.Surname} ({email})";
    }

    private void LoadExams()
    {
        foreach (Exam exam in _examCoordinator.GetGradedExams())
            Exams.Add(new ExamViewModel(exam));
    }
}