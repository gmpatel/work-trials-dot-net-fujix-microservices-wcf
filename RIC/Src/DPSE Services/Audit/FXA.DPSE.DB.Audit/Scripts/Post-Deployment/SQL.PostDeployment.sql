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

--IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Audit_Timestamp]') AND type = 'D')
--BEGIN
--ALTER TABLE [dbo].[Audit] ADD  CONSTRAINT [DF_Audit_Timestamp]  DEFAULT (getdate()) FOR [CreatedDateTime]
--END
--GO