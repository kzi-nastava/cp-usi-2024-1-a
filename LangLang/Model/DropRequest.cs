using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LangLang.Model;

public class DropRequest : INotifyPropertyChanged
{
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string CourseId { get; set; }
    public string ReceiverId { get; set; }
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
        ReceiverId = "";
        DropRequestStatus = Status.InReview;
    }
    public DropRequest(string studentId, string courseId, string tutorId)
    {
        Id = "";
        SenderId = studentId;
        CourseId = courseId;
        ReceiverId = tutorId;
        DropRequestStatus = Status.InReview;
    }
    public DropRequest(string id, string studentId, string courseId, string tutorId)
    {
        Id = id;
        SenderId = studentId;
        CourseId = courseId;
        ReceiverId = tutorId;
        DropRequestStatus = Status.InReview;
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
