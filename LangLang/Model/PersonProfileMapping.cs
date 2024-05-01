namespace LangLang.Model;

public class PersonProfileMapping
{
    public string Email { get; set; }
    public UserType UserType { get; set; }
    public string UserId { get; set; }

    public PersonProfileMapping()
    {
        Email = "";
        UserType = UserType.Student;
        UserId = "";
    }

    public PersonProfileMapping(string email, UserType userType, string userId)
    {
        Email = email;
        UserType = userType;
        UserId = userId;
    }
}