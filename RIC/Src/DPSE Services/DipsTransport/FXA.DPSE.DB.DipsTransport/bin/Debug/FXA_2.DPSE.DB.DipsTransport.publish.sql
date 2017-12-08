﻿/*
Deployment script for FXA.DPSE.DB.DipsTransport

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "FXA.DPSE.DB.DipsTransport"
:setvar DefaultFilePrefix "FXA.DPSE.DB.DipsTransport"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Dropping [dbo].[DF_Transmission_CreationDateTime]...';


GO
ALTER TABLE [dbo].[Transmission] DROP CONSTRAINT [DF_Transmission_CreationDateTime];


GO
PRINT N'Dropping [dbo].[DF_Transmission_TransactionCount]...';


GO
ALTER TABLE [dbo].[Transmission] DROP CONSTRAINT [DF_Transmission_TransactionCount];


GO
PRINT N'Dropping [dbo].[DF_Transmission_RetryCount]...';


GO
ALTER TABLE [dbo].[Transmission] DROP CONSTRAINT [DF_Transmission_RetryCount];


GO
PRINT N'Dropping [dbo].[FK_TransmissionBatch_Transmission]...';


GO
ALTER TABLE [dbo].[TransmissionBatch] DROP CONSTRAINT [FK_TransmissionBatch_Transmission];


GO
PRINT N'Starting rebuilding table [dbo].[Transmission]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Transmission] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [CreationDateTime]    DATETIME2 (7)  NOT NULL,
    [TransportedDateTime] DATETIME2 (7)  NULL,
    [FilePath]            NVARCHAR (255) NULL,
    [FileName]            NVARCHAR (255) NULL,
    [FileSHAHash]         NVARCHAR (512) NULL,
    [TransactionCount]    BIGINT         NULL,
    [RetryCount]          INT            NULL,
    [Status]              NVARCHAR (10)  NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Transmission] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY];

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Transmission])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Transmission] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Transmission] ([Id], [CreationDateTime], [TransportedDateTime], [FilePath], [FileName], [FileSHAHash], [TransactionCount], [RetryCount], [Status])
        SELECT   [Id],
                 [CreationDateTime],
                 [TransportedDateTime],
                 [FilePath],
                 [FileName],
                 [FileSHAHash],
                 [TransactionCount],
                 [RetryCount],
                 [Status]
        FROM     [dbo].[Transmission]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Transmission] OFF;
    END

DROP TABLE [dbo].[Transmission];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Transmission]', N'Transmission';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Transmission]', N'PK_Transmission', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
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

GO
PRINT N'Update complete.';


GO
