using System.Collections.Generic;

namespace LangLang.Application.UseCases.DropRequest;

public interface IDropRequestInfoService
{
    public Dictionary<string, string> GetSenderNames(List<Domain.Model.DropRequest> dropRequests);
}
