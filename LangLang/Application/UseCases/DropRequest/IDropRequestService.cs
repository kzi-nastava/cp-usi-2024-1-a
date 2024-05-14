using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.DropRequest;

public interface IDropRequestService
{
    public List<Domain.Model.DropRequest> GetDropRequests(string courseId);
    public List<Domain.Model.DropRequest> GetInReviewDropRequests(string courseId);
    public Domain.Model.DropRequest AddDropRequest(string courseId, Profile sender, string message);
    public Domain.Model.DropRequest Deny(Domain.Model.DropRequest dropRequest);
    public Domain.Model.DropRequest Accept(Domain.Model.DropRequest dropRequest);

}
