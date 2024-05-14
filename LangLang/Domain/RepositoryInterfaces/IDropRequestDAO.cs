using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IDropRequestDAO
{
    public List<DropRequest> GetDropRequests(string courseId);
    public List<DropRequest> GetInReviewDropRequests(string courseId);
    public DropRequest AddDropRequest(DropRequest dropRequest);
    public DropRequest? UpdateDropRequest(string id, DropRequest dropRequest);

}
