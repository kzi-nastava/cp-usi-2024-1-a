using System;
using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using ArgumentException = System.ArgumentException;

namespace LangLang.Application.Utility.Notification;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public List<Domain.Model.Notification> GetNotifications(Profile profile) => _notificationRepository.Get(profile);

    public List<Domain.Model.Notification> GetUnreadNotifications(Profile profile) => _notificationRepository.GetUnread(profile);

    public Domain.Model.Notification AddNotification(string message, Profile receiver, Profile? sender = null)
    {
        message = message.Trim();
        if (message == "") throw new ArgumentException("Message cannot be empty.");
        return _notificationRepository.Add(new Domain.Model.Notification(sender?.Email, receiver.Email, message, DateTime.Now));
    }

    public Domain.Model.Notification MarkAsRead(Domain.Model.Notification notification)
    {
        notification.ReadStatus = Domain.Model.Notification.Status.Read;
        return _notificationRepository.Update(notification.Id, notification)
            ?? throw new ArgumentException("No notification found with given id.");
    }
}