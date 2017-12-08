using System.ServiceModel;
using System.ServiceModel.Web;
using FXA.DPSE.Service.DTO.CodelineRules;

namespace FXA.DPSE.Service.CodelineRules
{
    [ServiceContract]
    public interface ICodelineRulesService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CodelineRules",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CodelineRulesResponse CodelineRules(CodelineRulesRequest codelineRulesRequest);
    }
}
