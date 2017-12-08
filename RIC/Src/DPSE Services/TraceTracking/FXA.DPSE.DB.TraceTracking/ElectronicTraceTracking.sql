CREATE TABLE [dbo].[ElectronicTraceTracking](
	[Id] BIGINT IDENTITY(1,1) NOT NULL,
	[RequestId] NVARCHAR(50) NULL,
	[ChannelType] NVARCHAR(20) NOT NULL,
	[ClientSessionId] NVARCHAR(50) NULL,
	[ClientDeviceId] NVARCHAR(60) NOT NULL,
	[ClientIpAddressV4] NVARCHAR(50) NOT NULL,
	[ChequeCount] INT NOT NULL,	
	[TotalTransactionAmount] INT NOT NULL,
	[DepositAccountName] NVARCHAR(255) NOT NULL,
	[DepositAccountNumber] NVARCHAR(20) NOT NULL,
	[DepositAccountBsbCode] NVARCHAR(6) NOT NULL,
	[DepositAccountType] NVARCHAR(20) NOT NULL,
	[DateTimeCreated] DATETIME2(7) NOT NULL,
    CONSTRAINT [PK_ElectronicTraceTracking] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]