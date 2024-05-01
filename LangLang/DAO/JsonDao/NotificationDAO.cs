using System.Collections.Generic;
using System.Linq;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO.JsonDao;

public class NotificationDAO : INotificationDAO
{
    private ILastIdDAO _lastIdDao;

    public NotificationDAO(ILastIdDAO lastIdDao)
    {
        _lastIdDao = lastIdDao;
    }
    
    private Dictionary<string, Notification>? _notifications;

    private Dictionary<string, Notification> Notifications
    {
        get
        {
            _notifications ??= JsonUtil.ReadFromFile<Notification>(Constants.NotificationFilePath);
            return _notifications!;
        }
        set => _notifications = value;
    }
    
    public List<Notification> GetNotifications(Profile profile)
    {
        return Notifications.Values.Where(notification => notification.Receiver.Email == profile.Email).ToList();
    }

    public List<Notification> GetUnreadNotifications(Profile profile)
    {
        return Notifications.Values.Where(
            notification => notification.Receiver.Email == profile.Email &&
                            notification.ReadStatus == Notification.Status.Unread
        ).ToList();
    }

    public Notification AddNotification(Notification notification)
    {
        _lastIdDao.IncrementNotificationId();
        notification.Id = _lastIdDao.GetNotificationId();
        Notifications.Add(notification.Id, notification);
        Save();
        return notification;
    }

    public Notification? UpdateNotification(string id, Notification notification)
    {
        if (!Notifications.ContainsKey(id)) return null;
        Notifications[id] = notification;
        Save();
        return notification;
    }

    private void Save()
    {
        JsonUtil.WriteToFile(Notifications, Constants.NotificationFilePath);
    }
}