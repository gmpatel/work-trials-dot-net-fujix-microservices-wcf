CREATE TABLE [dbo].[TransmissionBatch](
	[TransmissionId] [bigint] NOT NULL,
	[PaymentInstructionId] [bigint] NOT NULL,
	[BatchNumber] [nvarchar](8) NULL,
	[FilePath] [nvarchar](255) NULL,
	[FileName] [nvarchar](255) NULL,
	[TransactionCount] [int] NULL,
 CONSTRAINT [PK_TransmissionBatch] PRIMARY KEY CLUSTERED 
(
	[TransmissionId] ASC,
	[PaymentInstructionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]