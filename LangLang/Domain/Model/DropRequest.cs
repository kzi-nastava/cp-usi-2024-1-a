using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LangLang.Domain.Model;

public class DropRequest : INotifyPropertyChanged, IEntity
{
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string CourseId { get; set; }
    public string Message { get; set; }
    public enum Status
    {
        Accepted, Denied, InReview
    }
    private Status _dropRequestStatus;
    public Status DropRequestStatus
    {
        get => _dropRequestStatus;
        set
        {
            if (_dropRequestStatus == value) return;
            _dropRequestStatus = value;
            OnPropertyChanged();
        }
    }

    public DropRequest()
    {
        Id = "";
        SenderId = "";
        CourseId = "";
        DropRequestStatus = Status.InReview;
        Message = "";
    }
    public DropRequest(string studentId, string courseId, string message)
    {
        Id = "";
        SenderId = studentId;
        CourseId = courseId;
        DropRequestStatus = Status.InReview;
        Message = message;
    }
    public DropRequest(string id, string studentId, string courseId, string message)
    {
        Id = id;
        SenderId = studentId;
        CourseId = courseId;
        DropRequestStatus = Status.InReview;
        Message = message;
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
