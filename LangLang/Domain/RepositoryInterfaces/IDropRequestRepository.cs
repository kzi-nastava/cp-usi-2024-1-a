using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IDropRequestRepository : IRepository<DropRequest>
{
    public List<DropRequest> GetDropRequests(string courseId);
    public List<DropRequest> GetInReviewDropRequests(string courseId);
}
