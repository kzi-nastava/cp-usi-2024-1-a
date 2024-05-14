using LangLang.Domain.Model;

namespace LangLang.WPF.ViewModels.Common;

public class NotificationViewModel
{
    public string SenderName { get; }
    public Notification Notification { get; }

    public NotificationViewModel(string senderName, Notification notification)
    {
        SenderName = senderName;
        Notification = notification;
    }
}