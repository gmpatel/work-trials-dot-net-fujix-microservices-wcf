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
    
    public partial class AccountName
    {
        public long AccountId { get; set; }
        public string AccountName1 { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
