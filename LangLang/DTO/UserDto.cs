using LangLang.Model;

namespace LangLang.DTO;

public class UserDto
{
    public Person? Person { get; }
    public UserType? UserType { get; }

    public UserDto(Person? person, UserType? userType)
    {
        Person = person;
        UserType = userType;
    }
}