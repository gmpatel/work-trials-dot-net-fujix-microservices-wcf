CREATE TABLE [dbo].[Voucher](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentInstructionId] [bigint] NOT NULL,
	[TrackingId] [nvarchar](20) NOT NULL,
	[SequenceId] [int] NOT NULL,
	[VoucherType] [nvarchar](6) NOT NULL,
	[TransactionCode] [nvarchar](3) NOT NULL,
	[AuxDom] [nvarchar](20) NOT NULL,
	[ProcessingDateTime] [datetime] NOT NULL,
	[BSB] [nvarchar](6) NOT NULL,
	[AccountNumber] [nvarchar](20) NOT NULL,
	[AmountInCents] [int] NOT NULL,
	[IsNonPostingCheque] [bit] NOT NULL,
 CONSTRAINT [PK_Voucher] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]