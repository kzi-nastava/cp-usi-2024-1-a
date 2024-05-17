namespace LangLang.Application.Utility.Email;

public class EmailCredentials
{
    public string EmailAddress { get; }
    public string Password { get; }
    public string Host { get; }
    public int Port { get; }
    
    public EmailCredentials(string email, string password, string host, int port)
    {
        EmailAddress = email;
        Password = password;
        Host = host;
        Port = port;
    }
}