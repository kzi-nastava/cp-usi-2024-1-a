using LangLang.Model;
using LangLang.Services.AuthenticationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services.DropRequestServices;

public class DropRequestInfoService : IDropRequestInfoService
{
    private readonly IProfileService _profileService;
    private readonly IUserProfileMapper _userProfileMapper;

    public DropRequestInfoService(IProfileService profileService, IUserProfileMapper userProfileMapper)
    {
        _profileService = profileService;
        _userProfileMapper = userProfileMapper;
    }
    public Dictionary<string, string> GetSenderNames(List<DropRequest> dropRequests)
    {
        Dictionary<string, string> senderNames = new();
        foreach (var dropRequest in dropRequests)
        {
            if (dropRequest.SenderId == null)
            {
                senderNames.Add(dropRequest.Id, "");
                continue;
            }

            var profile = _profileService.GetProfile(dropRequest.SenderId);
            if (profile == null)
            {
                senderNames.Add(dropRequest.Id, "");
                continue;
            }

            var person = _userProfileMapper.GetPerson(profile).Person;
            if (person == null)
            {
                senderNames.Add(dropRequest.Id, "");
                continue;
            }

            senderNames.Add(dropRequest.Id, person.Name + " " + person.Surname + ":");
        }

        return senderNames;
    }
}
