using FXA.DPSE.Service.DTO.NabInternetBanking;

namespace FXA.DPSE.Service.ShadowPost.Facade.Core
{
    public class ProcessedChequeResponse
    {
        public UpdateAccountBalanceResponse UpdateAccountBalanceResponse { get; set; }
        public bool Failed { get; set; }
    }
}