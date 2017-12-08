using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FXA.DPSE.Framework.Service.WCF;
using FXA.DPSE.Service.DTO.Core.Response;
using FXA.DPSE.Service.DTO.NabInternetBanking;

namespace FXA.DPSE.NAB.InternetBankingDummy
{
    public class InternetBankingDummyService : DpseServiceBase, IInternetBankingDummyService
    {
        public UpdateAccountBalanceResponse UpdateAccountBalance(UpdateAccountBalanceRequest request)
        {
            var response = default(UpdateAccountBalanceResponse);

            if (string.IsNullOrEmpty(request.ResponseOverride) || string.IsNullOrWhiteSpace(request.ResponseOverride) || request.ResponseOverride.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
            {
                response = new UpdateAccountBalanceResponse
                {
                    CodeLine = request.CodeLine,
                    ConnexRef = "XXXXX-XXXX-XXX",
                    DailyLimit = "10000.00",
                    InternetBankingId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    DpseReference = request.TrackingId,
                    TransactionLimit = "10000.00",
                    SettlementDate = DateTime.Now.Date.ToString("yyyy-MM-dd")
                };

                return DpseResponse(response, HttpStatusCode.OK);
            }

            if (request.ResponseOverride.Equals("SuccessNextDay", StringComparison.CurrentCultureIgnoreCase))
            {
                response = new UpdateAccountBalanceResponse
                {
                    CodeLine = request.CodeLine,
                    ConnexRef = "XXXXX-XXXX-XXX",
                    DailyLimit = "10000.00",
                    InternetBankingId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    DpseReference = request.TrackingId,
                    TransactionLimit = "10000.00",
                    SettlementDate = DateTime.Now.AddDays(1).Date.ToString("yyyy-MM-dd")
                };

                return DpseResponse(response, HttpStatusCode.OK);
            }

            if (request.ResponseOverride.Equals("Blank", StringComparison.CurrentCultureIgnoreCase))
            {
                response = new UpdateAccountBalanceResponse
                {
                    CodeLine = request.CodeLine,
                    ConnexRef = "XXXXX-XXXX-XXX",
                    DailyLimit = "10000.00",
                    InternetBankingId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    DpseReference = request.TrackingId,
                    TransactionLimit = "10000.00",
                    SettlementDate = string.Empty
                };

                return DpseResponse(response, HttpStatusCode.OK);
            }

            response = new UpdateAccountBalanceResponse
            {
                CodeLine = request.CodeLine,
                ConnexRef = "XXXXX-XXXX-XXX",
                DailyLimit = "10000.00",
                InternetBankingId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                DpseReference = request.TrackingId,
                TransactionLimit = "10000.00",
                SettlementDate = string.Empty,
                Status = "Failed",
                Code = "500",
                Message = "Internet Banking Internal Error"
            };
            
            return DpseResponse(response, HttpStatusCode.InternalServerError);
        }
    }
}