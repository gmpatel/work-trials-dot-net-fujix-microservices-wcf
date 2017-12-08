//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FXA.DPSE.Service.PaymentInstruction.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentInstruction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentInstruction()
        {
            this.Vouchers = new HashSet<Voucher>();
        }
    
        public long Id { get; set; }
        public int TotalTransactionAmountInCents { get; set; }
        public string ChannelType { get; set; }
        public int ChequeCount { get; set; }
        public string TrackingId { get; set; }
        public long ClientSessionId { get; set; }
        public long AccountId { get; set; }
        public Nullable<System.DateTime> ProcessingDateTime { get; set; }
        public string BatchPath { get; set; }
        public Nullable<System.DateTime> BatchCreatedDateTime { get; set; }
        public Nullable<System.DateTime> TransportedDateTime { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public string Status { get; set; }
        public string BatchNumber { get; set; }
        public string TransactionNarrative { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual ClientSession ClientSession { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
