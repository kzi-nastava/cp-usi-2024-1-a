namespace LangLang.Model.Display;

public class NotificationDisplay
{
    public string SenderName { get; }
    public Notification Notification { get; }

    public NotificationDisplay(string senderName, Notification notification)
    {
        SenderName = senderName;
        Notification = notification;
    }
}