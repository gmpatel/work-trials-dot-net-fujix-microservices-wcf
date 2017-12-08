using System.Linq;
using System.Net;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Framework.Service.WCF.Attributes.Error;
using FXA.DPSE.Framework.Service.WCF.Attributes.Logging;
using FXA.DPSE.Framework.Service.WCF.Attributes.Validation;
using FXA.DPSE.Service.CodelineRules.Common;
using FXA.DPSE.Service.DTO.CodelineRules;

namespace FXA.DPSE.Service.CodelineRules
{
    [LoggingBehavior]
    [ErrorBehavior("DPSE-8804")]
    [ValidationBehavior("DPSE-8801")]
    public class CodelineRulesService : DpseServiceBase, ICodelineRulesService
    {
        public CodelineRulesResponse CodelineRules(CodelineRulesRequest codelineRulesRequest)
        {
            var codelineCheckingResponse = new CodelineRulesResponse()
            {
                Code = StatusCode.CodelineRulesSuccessful,
                Message = "Codeline Rules Successfull",
                TrackingId = codelineRulesRequest.TrackingId,
            };
            var codelineRulesCheques = codelineRulesRequest.Cheques.Select(cheque => new ChequeResponse()
            {
                SequenceId = cheque.SequenceId, 
                ChequeResponseCode = StatusCode.CodelineRulesSuccessful, 
                ChequeResponseDescription = "Codeline Rules Successfull"
            }).ToList();
            codelineCheckingResponse.Cheques = codelineRulesCheques.ToArray();
            return DpseResponse(codelineCheckingResponse, HttpStatusCode.OK);
        }
    }
}
