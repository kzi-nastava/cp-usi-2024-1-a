using System;

namespace LangLang.Model;

public class Notification
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
    public Status ReadStatus { get; set; }

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
}