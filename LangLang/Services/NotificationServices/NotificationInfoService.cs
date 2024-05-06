using System.Collections.Generic;
using LangLang.Model;
using LangLang.Services.AuthenticationServices;

namespace LangLang.Services.NotificationServices;

public class NotificationInfoService : INotificationInfoService
{
    private readonly IProfileService _profileService;
    private readonly IUserProfileMapper _userProfileMapper;

    public NotificationInfoService(IProfileService profileService, IUserProfileMapper userProfileMapper)
    {
        _profileService = profileService;
        _userProfileMapper = userProfileMapper;
    }

    public Dictionary<string, string> GetSenderNames(List<Notification> notifications)
    {
        Dictionary<string, string> senderNames = new();
        foreach (var notification in notifications)
        {
            if (notification.SenderId == null)
            {
                senderNames.Add(notification.Id, "");
                continue;
            }

            var profile = _profileService.GetProfile(notification.SenderId);
            if (profile == null)
            {
                senderNames.Add(notification.Id, "");
                continue;
            }
            
            var person = _userProfileMapper.GetPerson(profile).Person;
            if (person == null)
            {
                senderNames.Add(notification.Id, "");
                continue;
            }

            senderNames.Add(notification.Id, person.Name + " " + person.Surname + ":");
        }

        return senderNames;
    }
}