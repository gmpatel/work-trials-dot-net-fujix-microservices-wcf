CREATE TABLE [dbo].[ElectronicTraceTrackingDetails]
(
	[Id] BIGINT IDENTITY(1,1) NOT NULL,
	[ElectronicTraceTrackingId] BIGINT NOT NULL,
	[ChequeTraceTrackingCode] NVARCHAR (50) NULL,
    CONSTRAINT FK_ElectronicTraceTracking FOREIGN KEY (ElectronicTraceTrackingId) REFERENCES ElectronicTraceTracking(Id),
	CONSTRAINT [PK_ElectronicTraceTrackingDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]