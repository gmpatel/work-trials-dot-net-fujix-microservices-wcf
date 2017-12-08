﻿CREATE TABLE [dbo].[AccountName](
	[AccountId] [bigint] NOT NULL,
	[AccountName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_AccountName] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC,
	[AccountName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]