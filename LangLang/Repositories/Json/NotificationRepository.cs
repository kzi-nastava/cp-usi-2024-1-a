using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class NotificationRepository : AutoIdRepository<Notification>, INotificationRepository
{
    public NotificationRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
    {
    }
    
    public List<Notification> Get(Profile profile)
    {
        return GetAll().Where(notification => notification.ReceiverId == profile.Email).ToList();
    }

    public List<Notification> GetUnread(Profile profile)
    {
        return GetAll().Where(
            notification => notification.ReceiverId == profile.Email &&
                            notification.ReadStatus == Notification.Status.Unread
        ).ToList();
    }
}