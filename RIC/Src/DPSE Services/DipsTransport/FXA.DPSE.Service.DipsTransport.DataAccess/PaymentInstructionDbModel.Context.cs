﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FXA.DPSE.Service.DipsTransport.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PaymentInstructionDb : DbContext
    {
        public PaymentInstructionDb()
            : base("name=PaymentInstructionDb")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountName> AccountNames { get; set; }
        public virtual DbSet<ClientSession> ClientSessions { get; set; }
        public virtual DbSet<PaymentInstruction> PaymentInstructions { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<VoucherImage> VoucherImages { get; set; }
    }
}
