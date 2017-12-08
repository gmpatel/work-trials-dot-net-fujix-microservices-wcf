namespace FXA.DPSE.Service.ShadowPost.Facade.Core
{
    public interface IInternetBankingServiceFacade
    {
        ShadowPostFacadeResponse UpdateAccountBalance(ShadowPostRequestWrapper shadowPostRequestWrapper);
    }
}