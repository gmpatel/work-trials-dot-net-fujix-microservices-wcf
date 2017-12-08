using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSE.Service.PaymentInstruction.Business.Entities
{
    public class PaymentInstructionBusinessData
    {
        public string MessageVersion { get; set; }
        public string RequestDateTimeUtc { get; set; }
        public string Id { get; set; }
        public int TotalTransactionAmount { get; set; }
        public string ChannelType { get; set; }
        public int ChequeCount { get; set; }
        public string TransactionNarrative { get; set; }
        
        public PaymentInstructionSession ClientSession { get; set; }

        public PaymentInstructionAccountDetails DepositingAccountDetails { get; set; }

        public IList<PaymentInstructionNotification> Notifications { get; set; }

        public IList<PaymentInstructionPostingCheque> PostingCheques { get; set; }

        public IList<PaymentInstructionNonPostingCheque> NonPostingCheques { get; set; }

        public IDictionary<string, string> Headers { get; set; } 
    }

    public class PaymentInstructionSession
    {
        public string SessionId { get; set; }
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
        public string DeviceId { get; set; }
        public string IpAddressV4 { get; set; }
        public string IpAddressV6 { get; set; }
        public string ClientName { get; set; }
        public string ClientVersion { get; set; }
        public string Os { get; set; }
        public string OsVersion { get; set; }
        public string CaptureDevice { get; set; }
    }

    public class PaymentInstructionAccountDetails
    {
        public IList<PaymentInstructionAccountName> Names { get; set; }
        public string AccountNumber { get; set; }
        public string BsbCode { get; set; }
        public string AccountType { get; set; }
    }

    public class PaymentInstructionAccountName
    {
        public string Name { get; set; }
    }

    public class PaymentInstructionNotification
    {
        public string NotificationType { get; set; }
        public string NotificationInfo { get; set; }
    }

    public class PaymentInstructionPostingCheque
    {
        public int SequenceId { get; set; }
        public string Codeline { get; set; }
        public int ChequeAmount { get; set; }
        public string FrontImage { get; set; }
        public string RearImage { get; set; }
        public string FrontImageSha { get; set; }
        public string RearImageSha { get; set; }
    }

    public class PaymentInstructionNonPostingCheque
    {
        public int SequenceId { get; set; }
        public string Codeline { get; set; }
        public int ChequeAmount { get; set; }
        public string FrontImage { get; set; }
        public string RearImage { get; set; }
    }
}
