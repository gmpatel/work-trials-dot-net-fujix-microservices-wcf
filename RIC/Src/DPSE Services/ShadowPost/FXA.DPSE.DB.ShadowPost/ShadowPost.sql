CREATE TABLE [dbo].[ShadowPost]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TrackingId] [nvarchar](20) NOT NULL,
	[RequestGuid] UNIQUEIDENTIFIER NOT NULL, 
	[SessionId] nvarchar(60) NOT NULL,
	[DeviceId] nvarchar(100) NOT NULL, 
	[IpAddressV4] [nvarchar](50) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	CONSTRAINT [PK_PaymentInstruction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]