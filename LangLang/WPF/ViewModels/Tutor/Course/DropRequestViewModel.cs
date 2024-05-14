using LangLang.Domain.Model;

namespace LangLang.WPF.ViewModels.Tutor.Course;

public class DropRequestViewModel
{
    public string Sender { get; }
    public DropRequest DropRequest { get; }

    public DropRequestViewModel(string sender, DropRequest dropRequest)
    {
        Sender = sender;
        DropRequest = dropRequest;
    }
}
