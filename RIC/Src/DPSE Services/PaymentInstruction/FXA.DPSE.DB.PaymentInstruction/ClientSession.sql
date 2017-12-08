CREATE TABLE [dbo].[ClientSession](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SessionId] [nvarchar](60) NOT NULL,
	[UserId1] [nvarchar](50) NOT NULL,
	[UserId2] [nvarchar](50) NULL,
	[DeviceId] [nvarchar](100) NULL,
	[IpAddressV4] [nvarchar](50) NULL,
	[IpAddressV6] [nvarchar](50) NULL,
	[ClientName] [nvarchar](50) NULL,
	[ClientVersion] [nvarchar](50) NULL,
	[OS] [nvarchar](50) NULL,
	[OSVersion] [nvarchar](50) NULL,
	[CaptureDevice] [nvarchar](50) NULL,
 CONSTRAINT [PK_ClientSession] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]