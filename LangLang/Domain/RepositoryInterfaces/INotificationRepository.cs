using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface INotificationRepository : IRepository<Notification>
{
    public List<Notification> Get(Profile profile);
    public List<Notification> GetUnread(Profile profile);
}