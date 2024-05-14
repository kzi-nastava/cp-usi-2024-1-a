using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json;

public class ProfileDAO : IProfileDAO
{
    private Dictionary<string, Profile>? _profiles;
    
    private Dictionary<string, Profile> Profiles
    {
        get
        {
            _profiles ??= JsonUtil.ReadFromFile<Profile>(Constants.ProfileFilePath);
            return _profiles!;
        }
        set => _profiles = value;
    }
    
    public Dictionary<string, Profile> GetAllProfiles()
    {
        return Profiles;
    }

    public Profile? GetProfile(string email)
    {
        return Profiles.GetValueOrDefault(email);
    }

    public Profile AddProfile(Profile profile)
    {
        Profiles.Add(profile.Email, profile);
        SaveProfiles();
        return profile;
    }

    public Profile? UpdateProfile(string email, Profile profile)
    {
        if (!Profiles.ContainsKey(email)) return null;
        Profiles[email] = profile;
        SaveProfiles();
        return profile;
    }

    public void DeleteProfile(string email)
    {
        Profiles.Remove(email);
        SaveProfiles();
    }
    
    private void SaveProfiles()
    {
        JsonUtil.WriteToFile(Profiles, Constants.ProfileFilePath);
    }
}