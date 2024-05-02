using LangLang.DTO;
using LangLang.Model;

namespace LangLang.Services.AuthenticationServices;

public interface IUserProfileMapper
{
    public UserDto GetPerson(Profile profile);
    public Profile? GetProfile(UserDto user);
}