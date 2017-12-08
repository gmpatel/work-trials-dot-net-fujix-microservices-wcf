CREATE TABLE [dbo].[Transmission](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreationDateTime] [datetime2](7) NOT NULL,
	[TransportedDateTime] [datetime2](7) NULL,
	[FilePath] [nvarchar](255) NULL,
	[FileName] [nvarchar](255) NULL,
	[FileSHAHash] [nvarchar](512) NULL,
	[TransactionCount] [bigint] NULL,
	[RetryCount] [int] NULL,
	[Status] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Transmission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]