namespace LangLang.Model;

public class Profile
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }

    public Profile()
    {
        Email = "";
        Password = "";
    }
    
    public Profile(string email, string password)
    {
        Email = email;
        Password = password;
        IsActive = true;
    }

    public Profile(string email, string password, bool isActive)
    {
        Email = email;
        Password = password;
        IsActive = isActive;
    }
}
