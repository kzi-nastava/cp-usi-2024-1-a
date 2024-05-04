using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.DAO;

public interface IDropRequestDAO
{
    public List<DropRequest> GetDropRequestsCourse(Profile profile, string courseId);
    public List<DropRequest> GetDropRequests(Profile profile);
    public DropRequest AddDropRequest(DropRequest dropRequest);
    public DropRequest? UpdateDropRequest(string id, DropRequest dropRequest);

}
