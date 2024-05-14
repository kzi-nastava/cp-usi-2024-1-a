using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface INotificationDAO
{
    public List<Notification> GetNotifications(Profile profile);
    public List<Notification> GetUnreadNotifications(Profile profile);
    public Notification AddNotification(Notification notification);
    public Notification? UpdateNotification(string id, Notification notification);
}