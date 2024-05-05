using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.NotificationServices;

public interface INotificationService
{
    public List<Notification> GetNotifications(Profile profile);
    public List<Notification> GetUnreadNotifications(Profile profile);
    public Notification AddNotification(string message, Profile receiver, Profile? sender);
    public Notification MarkAsRead(Notification notification);
}