using System.Collections.Generic;
namespace FXA.DPSE.Service.ShadowPost.Facade.Core
{
    public class ShadowPostFacadeResponse
    {
        public ShadowPostFacadeResponse()
        {
            ProcessedChequeResponses = new List<ProcessedChequeResponse>();
        }
        public List<ProcessedChequeResponse> ProcessedChequeResponses { get; set; }

        public bool FacadeInternalProcessingFailure { get; set; }
        public bool IbInternalProcessingFailure { get; set; }
    }
}