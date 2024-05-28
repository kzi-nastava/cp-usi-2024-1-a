using System.Collections.Generic;

namespace LangLang.Application.Utility.Notification;

public interface INotificationInfoService
{
    public Dictionary<string, string> GetSenderNames(List<Domain.Model.Notification> notifications);
}