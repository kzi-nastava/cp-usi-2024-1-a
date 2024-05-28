using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.Utility.Notification;

public interface INotificationService
{
    public List<Domain.Model.Notification> GetNotifications(Profile profile);
    public List<Domain.Model.Notification> GetUnreadNotifications(Profile profile);
    public Domain.Model.Notification AddNotification(string message, Profile receiver, Profile? sender);
    public Domain.Model.Notification MarkAsRead(Domain.Model.Notification notification);
}