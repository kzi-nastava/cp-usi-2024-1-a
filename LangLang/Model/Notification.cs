using System;

namespace LangLang.Model;

public class Notification
{
    public string Id { get; set; }
    public Profile? Sender { get; set; }
    public Profile Receiver { get; set; }
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
        Receiver = new Profile();
        Message = "";
    }

    public Notification(Profile? sender, Profile receiver, string message, DateTime dateTime)
    {
        Id = "";
        Sender = sender;
        Receiver = receiver;
        Message = message;
        DateTime = dateTime;
    }

    public Notification(string id, Profile? sender, Profile receiver, string message, DateTime dateTime)
    {
        Id = id;
        Sender = sender;
        Receiver = receiver;
        Message = message;
        DateTime = dateTime;
    }
}