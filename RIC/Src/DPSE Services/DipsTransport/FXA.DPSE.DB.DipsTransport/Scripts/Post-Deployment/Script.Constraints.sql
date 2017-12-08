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

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Transmission_CreationDateTime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Transmission] ADD  CONSTRAINT [DF_Transmission_CreationDateTime]  DEFAULT (getdate()) FOR [CreationDateTime]
END
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Transmission_TransactionCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Transmission] ADD  CONSTRAINT [DF_Transmission_TransactionCount]  DEFAULT ((0)) FOR [TransactionCount]
END
GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Transmission_RetryCount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Transmission] ADD  CONSTRAINT [DF_Transmission_RetryCount]  DEFAULT ((0)) FOR [RetryCount]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TransmissionBatch_Transmission]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransmissionBatch]'))
ALTER TABLE [dbo].[TransmissionBatch]  WITH CHECK ADD  CONSTRAINT [FK_TransmissionBatch_Transmission] FOREIGN KEY([TransmissionId])
REFERENCES [dbo].[Transmission] ([Id])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TransmissionBatch_Transmission]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransmissionBatch]'))
ALTER TABLE [dbo].[TransmissionBatch] CHECK CONSTRAINT [FK_TransmissionBatch_Transmission]
GO