using LangLang.DAO;
using LangLang.Model;
using LangLang.Services.UtilityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services.CourseServices;

public class DropRequestService : IDropRequestService
{
    private readonly IDropRequestDAO _dropRequestDao;

    public DropRequestService(IDropRequestDAO dropRequestDao)
    {
        _dropRequestDao = dropRequestDao;
    }
    public DropRequest Accept(DropRequest dropRequest)
    {
        dropRequest.DropRequestStatus = DropRequest.Status.Accepted;
        _dropRequestDao.UpdateDropRequest(dropRequest.Id, dropRequest);
        return dropRequest;
    }

    public DropRequest AddDropRequest(string courseId, Profile sender, Profile receiver)
    {
        return _dropRequestDao.AddDropRequest(new DropRequest(sender.Email, courseId, receiver.Email));
    }

    public DropRequest Deny(DropRequest dropRequest)
    {
        dropRequest.DropRequestStatus = DropRequest.Status.Denied;
        _dropRequestDao.UpdateDropRequest(dropRequest.Id, dropRequest);
        return dropRequest;
    }

    public List<DropRequest> GetDropRequests(Profile profile)
    {
        return _dropRequestDao.GetDropRequests(profile);
    }

    public List<DropRequest> GetDropRequestsCourse(Profile profile, string courseId)
    {
        return _dropRequestDao.GetDropRequestsCourse(profile, courseId);
    }
}
