using System;
using System.Collections.Generic;
using LangLang.DAO;
using LangLang.Model;
using ArgumentException = System.ArgumentException;

namespace LangLang.Services.NotificationServices;

public class NotificationService : INotificationService
{
    private readonly INotificationDAO _notificationDao;

    public NotificationService(INotificationDAO notificationDao)
    {
        _notificationDao = notificationDao;
    }

    public List<Notification> GetNotifications(Profile profile) => _notificationDao.GetNotifications(profile);

    public List<Notification> GetUnreadNotifications(Profile profile) => _notificationDao.GetUnreadNotifications(profile);

    public Notification AddNotification(string message, Profile receiver, Profile? sender = null)
    {
        message = message.Trim();
        if (message == "") throw new ArgumentException("Message cannot be empty.");
        return _notificationDao.AddNotification(new Notification(sender?.Email, receiver.Email, message, DateTime.Now));
    }

    public Notification MarkAsRead(Notification notification)
    {
        notification.ReadStatus = Notification.Status.Read;
        return _notificationDao.UpdateNotification(notification.Id, notification)
            ?? throw new ArgumentException("No notification found with given id.");
    }
}