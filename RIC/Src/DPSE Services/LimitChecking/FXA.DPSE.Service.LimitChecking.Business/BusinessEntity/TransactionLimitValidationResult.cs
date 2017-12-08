using System.Collections.Generic;
using FXA.DPSE.Framework.Service.WCF.Business;

namespace FXA.DPSE.Service.LimitChecking.Business.BusinessEntity
{
    public class TransactionLimitValidationResult : BusinessResult
    {
        public List<DepositChequeResult> Cheques { get; set; }
    }
}