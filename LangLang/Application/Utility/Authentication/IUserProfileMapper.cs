using LangLang.Application.DTO;
using LangLang.Domain.Model;

namespace LangLang.Application.Utility.Authentication;

public interface IUserProfileMapper
{
    public UserDto GetPerson(Profile profile);
    public Profile? GetProfile(UserDto user);
}