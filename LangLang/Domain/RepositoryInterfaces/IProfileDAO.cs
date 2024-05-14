using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IProfileDAO
{
    public Dictionary<string, Profile> GetAllProfiles();
    public Profile? GetProfile(string email);
    public Profile AddProfile(Profile profile);
    public Profile? UpdateProfile(string email, Profile profile);    
    public void DeleteProfile(string email);
}