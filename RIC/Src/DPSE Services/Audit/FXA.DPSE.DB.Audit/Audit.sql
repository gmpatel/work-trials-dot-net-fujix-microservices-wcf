﻿--CREATE SCHEMA Core Authorization dbo
--GO

--CREATE TABLE [Core].[Audit](
--	[Id] [bigint] IDENTITY(1,1) NOT NULL,
--	[TrackingIdentifier] [nvarchar](20) NULL,
--	[ExternalCorrelationIdentifier] [nvarchar](50) NULL,
--	[DocumentReferenceNumber] [nvarchar](12) NULL,
--	[AuditDateTime] [datetimeoffset](7) NOT NULL,
--	[Name] [nvarchar](50) NOT NULL,
--	[Description] [nvarchar](500) NOT NULL,
--	[Resource] [nvarchar](max) NULL,
--	[ChannelType] [nvarchar](20) NOT NULL,
--	[MachineName] [nvarchar](50) NOT NULL,
--	[ServiceName] [nvarchar](50) NOT NULL,
--	[OperationName] [nvarchar](50) NULL,
--	[OperatorName] [nvarchar](50) NULL,
--	[CreatedDateTime] [datetime] NOT NULL DEFAULT getdate(),
-- CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]