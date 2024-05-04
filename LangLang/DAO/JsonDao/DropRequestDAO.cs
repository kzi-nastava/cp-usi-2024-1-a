﻿using Consts;
using LangLang.Model;
using LangLang.Util;
using System.Collections.Generic;

namespace LangLang.DAO.JsonDao;

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

    public List<DropRequest> GetDropRequests(Profile profile)
    {
        List<DropRequest> dropRequests = new();
        foreach(DropRequest dropRequest in DropRequests.Values)
        {
            if(dropRequest.ReceiverId == profile.Email)
            {
                dropRequests.Add(dropRequest);
            }
        }
        return dropRequests;
    }

    public List<DropRequest> GetDropRequestsCourse(Profile profile, string courseId)
    {
        List<DropRequest> dropRequests = new();
        foreach (DropRequest dropRequest in DropRequests.Values)
        {
            if (dropRequest.ReceiverId == profile.Email &&  dropRequest.CourseId == courseId)
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
}
