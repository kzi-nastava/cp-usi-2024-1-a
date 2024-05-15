using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class DropRequestRepository : AutoIdRepository<DropRequest>, IDropRequestRepository
{
    public DropRequestRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
    {
    }

    public List<DropRequest> GetDropRequests(string courseId)
    {
        List<DropRequest> dropRequests = new();
        foreach(DropRequest dropRequest in GetAll().Values)
        {
            if(dropRequest.CourseId == courseId)
            {
                dropRequests.Add(dropRequest);
            }
        }
        return dropRequests;
    }
    
    public List<DropRequest> GetInReviewDropRequests(string courseId)
    {
        List<DropRequest> dropRequests = new();
        foreach (DropRequest dropRequest in GetAll().Values)
        {
            if (dropRequest.CourseId == courseId && dropRequest.DropRequestStatus == DropRequest.Status.InReview)
            {
                dropRequests.Add(dropRequest);
            }
        }
        return dropRequests;
    }
}
