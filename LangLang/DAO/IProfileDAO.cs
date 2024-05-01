using System;
using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.DAO;

public interface IProfileDAO
{
    public Dictionary<string, Profile> GetAllProfiles();
    public Profile? GetProfileById(string email);
    public Profile AddProfile(Profile profile);
    public Profile? UpdateProfile(string email, Profile profile);    
    public void DeleteProfile(string email);
}