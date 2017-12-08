CREATE TABLE [dbo].[PaymentInstruction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalTransactionAmountInCents] [int] NOT NULL,
	[ChannelType] [nvarchar](20) NOT NULL,
	[ChequeCount] [int] NOT NULL,
	[TransactionNarrative] [nvarchar](100) NOT NULL,
	[TrackingId] [nvarchar](20) NOT NULL,
	[ClientSessionId] [bigint] NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[ProcessingDateTime] [datetime] NULL,
	[BatchNumber] [nvarchar](8) NULL,
	[BatchPath] [nvarchar](255) NULL,
	[BatchCreatedDateTime] [datetime] NULL,
	[TransportedDateTime] [datetime] NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[Status] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_PaymentInstruction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
