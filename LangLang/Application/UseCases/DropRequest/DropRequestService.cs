using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.DropRequest;

public class DropRequestService : IDropRequestService
{
    private readonly IDropRequestRepository _dropRequestRepository;

    public DropRequestService(IDropRequestRepository dropRequestRepository)
    {
        _dropRequestRepository = dropRequestRepository;
    }
    public Domain.Model.DropRequest Accept(Domain.Model.DropRequest dropRequest)
    {
        dropRequest.DropRequestStatus = Domain.Model.DropRequest.Status.Accepted;
        _dropRequestRepository.Update(dropRequest.Id, dropRequest);
        return dropRequest;
    }

    public Domain.Model.DropRequest AddDropRequest(string courseId, Profile sender, string message)
    {
        return _dropRequestRepository.Add(new Domain.Model.DropRequest(sender.Email, courseId, message));
    }

    public Domain.Model.DropRequest Deny(Domain.Model.DropRequest dropRequest)
    {
        dropRequest.DropRequestStatus = Domain.Model.DropRequest.Status.Denied;
        _dropRequestRepository.Update(dropRequest.Id, dropRequest);
        return dropRequest;
    }

    public List<Domain.Model.DropRequest> GetDropRequests(string courseId)
    {
        return _dropRequestRepository.GetDropRequests(courseId);
    }

    public List<Domain.Model.DropRequest> GetInReviewDropRequests(string courseId)
    {
        return _dropRequestRepository.GetInReviewDropRequests(courseId);
    }
}
