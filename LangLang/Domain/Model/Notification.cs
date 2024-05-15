using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LangLang.Domain.Model;

public class Notification : INotifyPropertyChanged, IEntity
{
    public string Id { get; set; }
    public string? SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Message { get; set; }
    public DateTime DateTime { get; set; }

    public enum Status
    {
        Unread, Read
    }

    private Status _readStatus;

    public Status ReadStatus
    {
        get => _readStatus;
        set
        {
            if (_readStatus == value) return;
            _readStatus = value;
            OnPropertyChanged();
        }
    }

    public Notification()
    {
        Id = "";
        SenderId = "";
        ReceiverId = "";
        Message = "";
    }

    public Notification(string? senderId, string receiverId, string message, DateTime dateTime)
    {
        Id = "";
        SenderId = senderId;
        ReceiverId = receiverId;
        Message = message;
        DateTime = dateTime;
    }

    public Notification(string id, string? senderId, string receiverId, string message, DateTime dateTime)
    {
        Id = id;
        SenderId = senderId;
        ReceiverId = receiverId;
        Message = message;
        DateTime = dateTime;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}