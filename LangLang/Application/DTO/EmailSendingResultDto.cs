using LangLang.Domain.Model;
using System.Collections.Generic;

namespace LangLang.Application.DTO;

public class EmailSendingResultDto
{
    public List<Person> SuccessfullySent { get; set; } = new();
    public List<Person> FailedToSend { get; set; } = new();

    public EmailSendingResultDto() { }

    public EmailSendingResultDto(List<Person> successfullySent, List<Person> failedToSend) 
    {
        SuccessfullySent = successfullySent;
        FailedToSend = failedToSend;
    }
}

