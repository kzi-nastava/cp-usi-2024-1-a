using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services.NotificationServices;

public interface INotificationInfoService
{
    public Dictionary<string, string> GetSenderNames(List<Notification> notifications);
}