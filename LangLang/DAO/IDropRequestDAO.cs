using LangLang.Model;
using System.Collections.Generic;

namespace LangLang.DAO;

public interface IDropRequestDAO
{
    public List<DropRequest> GetDropRequests(string courseId);
    public DropRequest AddDropRequest(DropRequest dropRequest);
    public DropRequest? UpdateDropRequest(string id, DropRequest dropRequest);

}
