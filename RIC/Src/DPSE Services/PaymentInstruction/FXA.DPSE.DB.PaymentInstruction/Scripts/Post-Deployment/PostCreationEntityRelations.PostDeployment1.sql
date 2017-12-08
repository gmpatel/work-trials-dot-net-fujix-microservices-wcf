/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountName]') AND name = N'IX_AccountId')
CREATE NONCLUSTERED INDEX [IX_AccountId] ON [dbo].[AccountName]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PaymentInstruction_AccountId]    Script Date: 13/10/2015 6:14:01 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PaymentInstruction]') AND name = N'IX_PaymentInstruction_AccountId')
CREATE NONCLUSTERED INDEX [IX_PaymentInstruction_AccountId] ON [dbo].[PaymentInstruction]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PaymentInstruction_ClientSessionId]    Script Date: 13/10/2015 6:14:01 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PaymentInstruction]') AND name = N'IX_PaymentInstruction_ClientSessionId')
CREATE NONCLUSTERED INDEX [IX_PaymentInstruction_ClientSessionId] ON [dbo].[PaymentInstruction]
(
	[ClientSessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Voucher_PaymentInstructionId]    Script Date: 13/10/2015 6:14:01 PM ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Voucher]') AND name = N'IX_Voucher_PaymentInstructionId')
CREATE NONCLUSTERED INDEX [IX_Voucher_PaymentInstructionId] ON [dbo].[Voucher]
(
	[PaymentInstructionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_PaymentInstruction_CreateDateTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PaymentInstruction] ADD  CONSTRAINT [DF_PaymentInstruction_CreateDateTime]  DEFAULT (getdate()) FOR [CreatedDateTime]
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_PaymentInstruction_Status]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PaymentInstruction] ADD  CONSTRAINT [DF_PaymentInstruction_Status]  DEFAULT (N'PENDING') FOR [Status]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountName_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountName]'))
ALTER TABLE [dbo].[AccountName]  WITH CHECK ADD  CONSTRAINT [FK_AccountName_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountName_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountName]'))
ALTER TABLE [dbo].[AccountName] CHECK CONSTRAINT [FK_AccountName_Account]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PaymentInstruction_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[PaymentInstruction]'))
ALTER TABLE [dbo].[PaymentInstruction]  WITH CHECK ADD  CONSTRAINT [FK_PaymentInstruction_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PaymentInstruction_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[PaymentInstruction]'))
ALTER TABLE [dbo].[PaymentInstruction] CHECK CONSTRAINT [FK_PaymentInstruction_Account]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PaymentInstruction_ClientSession]') AND parent_object_id = OBJECT_ID(N'[dbo].[PaymentInstruction]'))
ALTER TABLE [dbo].[PaymentInstruction]  WITH CHECK ADD  CONSTRAINT [FK_PaymentInstruction_ClientSession] FOREIGN KEY([ClientSessionId])
REFERENCES [dbo].[ClientSession] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PaymentInstruction_ClientSession]') AND parent_object_id = OBJECT_ID(N'[dbo].[PaymentInstruction]'))
ALTER TABLE [dbo].[PaymentInstruction] CHECK CONSTRAINT [FK_PaymentInstruction_ClientSession]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Voucher_PaymentInstruction]') AND parent_object_id = OBJECT_ID(N'[dbo].[Voucher]'))
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [FK_Voucher_PaymentInstruction] FOREIGN KEY([PaymentInstructionId])
REFERENCES [dbo].[PaymentInstruction] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Voucher_PaymentInstruction]') AND parent_object_id = OBJECT_ID(N'[dbo].[Voucher]'))
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [FK_Voucher_PaymentInstruction]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoucherImage_Voucher]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoucherImage]'))
ALTER TABLE [dbo].[VoucherImage]  WITH CHECK ADD  CONSTRAINT [FK_VoucherImage_Voucher] FOREIGN KEY([VoucherId])
REFERENCES [dbo].[Voucher] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoucherImage_Voucher]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoucherImage]'))
ALTER TABLE [dbo].[VoucherImage] CHECK CONSTRAINT [FK_VoucherImage_Voucher]
GO
--ALTER DATABASE [DPSE_PaymentInstruction] SET  READ_WRITE 
--GO