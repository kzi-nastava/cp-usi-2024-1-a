using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model;

public class DropRequest : INotifyPropertyChanged
{
    public string Id { get; set; }
    public string StudentId { get; set; }
    public string CourseId { get; set; }
    public string TutorId { get; set; }
    public enum Status
    {
        Accepted, Denied
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
        StudentId = "";
        CourseId = "";
        TutorId = "";
    }
    public DropRequest(string studentId, string courseId, string tutorId)
    {
        Id = "";
        StudentId = studentId;
        CourseId = courseId;
        TutorId = tutorId;
    }
    public DropRequest(string id, string studentId, string courseId, string tutorId)
    {
        Id = id;
        StudentId = studentId;
        CourseId = courseId;
        TutorId = tutorId;
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
