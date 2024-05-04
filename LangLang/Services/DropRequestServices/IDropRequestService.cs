using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.Services.DropRequestServices;

public interface IDropRequestService
{
    public List<DropRequest> GetDropRequests(string courseId);
    public DropRequest AddDropRequest(string courseId, Profile sender, string message);
    public DropRequest Deny(DropRequest dropRequest);
    public DropRequest Accept(DropRequest dropRequest);

}
