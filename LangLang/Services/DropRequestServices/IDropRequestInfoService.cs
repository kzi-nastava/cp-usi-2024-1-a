using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services.DropRequestServices;

public interface IDropRequestInfoService
{
    public Dictionary<string, string> GetSenderNames(List<DropRequest> dropRequests);
}
