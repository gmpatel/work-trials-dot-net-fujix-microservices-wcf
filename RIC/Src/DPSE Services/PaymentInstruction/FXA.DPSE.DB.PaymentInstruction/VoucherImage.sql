CREATE TABLE [dbo].[VoucherImage](
	[VoucherId] [bigint] NOT NULL,
	[FrontImage] [nvarchar](max) NOT NULL,
	[RearImage] [nvarchar](max) NOT NULL,
	[FrontImageSHA] [nvarchar](max) NOT NULL,
	[RearImageSHA] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_VoucherImage] PRIMARY KEY CLUSTERED 
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]