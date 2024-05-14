using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json;

public class DropRequestDAO : IDropRequestDAO
{
    private ILastIdDAO _lastIdDao;
    public DropRequestDAO(ILastIdDAO lastIdDAO)
    {
        _lastIdDao = lastIdDAO;
    }
    private Dictionary<string, DropRequest>? _dropRequests;
    public Dictionary<string, DropRequest> DropRequests
    {
        get
        {
            _dropRequests ??= JsonUtil.ReadFromFile<DropRequest>(Constants.DropRequestFilePath);
            return _dropRequests;
        }
        set => _dropRequests = value;
    }
    public DropRequest AddDropRequest(DropRequest dropRequest)
    {
        _lastIdDao.IncrementDropRequstId();
        dropRequest.Id = _lastIdDao.GetDropRequestId();
        DropRequests.Add(dropRequest.Id, dropRequest);
        Save();
        return dropRequest;
    }

    public List<DropRequest> GetDropRequests(string courseId)
    {
        List<DropRequest> dropRequests = new();
        foreach(DropRequest dropRequest in DropRequests.Values)
        {
            if(dropRequest.CourseId == courseId)
            {
                dropRequests.Add(dropRequest);
            }
        }
        return dropRequests;
    }
    public DropRequest? UpdateDropRequest(string id, DropRequest dropRequest)
    {
        if (!DropRequests.ContainsKey(id)) return null;
        DropRequests[id] = dropRequest;
        Save();
        return dropRequest;
    }
    private void Save()
    {
        JsonUtil.WriteToFile(DropRequests, Constants.DropRequestFilePath);
    }

    public List<DropRequest> GetInReviewDropRequests(string courseId)
    {
        List<DropRequest> dropRequests = new();
        foreach (DropRequest dropRequest in DropRequests.Values)
        {
            if (dropRequest.CourseId == courseId && dropRequest.DropRequestStatus == DropRequest.Status.InReview)
            {
                dropRequests.Add(dropRequest);
            }
        }
        return dropRequests;
    }
}
