namespace LangLang.Model.Display;

public class DropRequestDisplay
{
    public string Sender { get; }
    public DropRequest DropRequest { get; }

    public DropRequestDisplay(string sender, DropRequest dropRequest)
    {
        Sender = sender;
        DropRequest = dropRequest;
    }
}
