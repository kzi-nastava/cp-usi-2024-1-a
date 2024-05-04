using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.Services.CourseServices;

public interface IDropRequestService
{
    public List<DropRequest> GetDropRequests(Profile profile);
    public List<DropRequest> GetDropRequestsCourse(Profile profile, string courseId);
    public DropRequest AddDropRequest(string courseId, Profile sender, Profile receiver);
    public DropRequest Deny(DropRequest dropRequest);
    public DropRequest Accept(DropRequest dropRequest);

}
