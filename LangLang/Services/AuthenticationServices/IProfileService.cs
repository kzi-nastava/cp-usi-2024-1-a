using LangLang.Model;

namespace LangLang.Services.AuthenticationServices;

public interface IProfileService
{
    public Profile? GetProfile(string email);
    public bool IsEmailTaken(string email);
    public Profile AddProfile(Profile profile);
    public void DeactivateProfile(Profile profile);
}