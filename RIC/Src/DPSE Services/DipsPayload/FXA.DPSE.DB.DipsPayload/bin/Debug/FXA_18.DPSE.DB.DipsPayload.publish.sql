﻿/*
Deployment script for FXA.DPSE.DB.DipsPayload

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "FXA.DPSE.DB.DipsPayload"
:setvar DefaultFilePrefix "FXA.DPSE.DB.DipsPayload"
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
PRINT N'Dropping [dbo].[FK_Payload]...';


GO
ALTER TABLE [dbo].[PayloadDetails] DROP CONSTRAINT [FK_Payload];


GO
PRINT N'Starting rebuilding table [dbo].[Payloads]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Payloads] (
    [Id]                  BIGINT           IDENTITY (1, 1) NOT NULL,
    [TransactionId]       BIGINT           NOT NULL,
    [ChannelType]         NVARCHAR (50)    NOT NULL,
    [DateTimeCreated]     DATETIME2 (7)    NOT NULL,
    [RelativePath]        NVARCHAR (1000)  NOT NULL,
    [LockedBy]            UNIQUEIDENTIFIER NULL,
    [Locked]              BIT              NOT NULL,
    [DateTimeLocked]      DATETIME2 (7)    NULL,
    [Transported]         BIT              NOT NULL,
    [DateTimeTransported] DATETIME2 (7)    NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Payloads] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY];

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Payloads])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Payloads] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Payloads] ([Id], [TransactionId], [ChannelType], [DateTimeCreated], [RelativePath], [LockedBy], [Locked], [DateTimeLocked], [Transported], [DateTimeTransported])
        SELECT   [Id],
                 [TransactionId],
                 [ChannelType],
                 [DateTimeCreated],
                 [RelativePath],
                 [LockedBy],
                 [Locked],
                 [DateTimeLocked],
                 [Transported],
                 [DateTimeTransported]
        FROM     [dbo].[Payloads]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Payloads] OFF;
    END

DROP TABLE [dbo].[Payloads];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Payloads]', N'Payloads';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Payloads]', N'PK_Payloads', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[PayloadDetails]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_PayloadDetails] (
    [Id]                        BIGINT IDENTITY (1, 1) NOT NULL,
    [PayloadId]                 BIGINT NOT NULL,
    [ElectronicTraceTrackingId] BIGINT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_PayloadDetails] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY];

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[PayloadDetails])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PayloadDetails] ON;
        INSERT INTO [dbo].[tmp_ms_xx_PayloadDetails] ([Id], [PayloadId], [ElectronicTraceTrackingId])
        SELECT   [Id],
                 [PayloadId],
                 [ElectronicTraceTrackingId]
        FROM     [dbo].[PayloadDetails]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_PayloadDetails] OFF;
    END

DROP TABLE [dbo].[PayloadDetails];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_PayloadDetails]', N'PayloadDetails';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_PayloadDetails]', N'PK_PayloadDetails', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[FK_Payload]...';


GO
ALTER TABLE [dbo].[PayloadDetails] WITH NOCHECK
    ADD CONSTRAINT [FK_Payload] FOREIGN KEY ([PayloadId]) REFERENCES [dbo].[Payloads] ([Id]);


GO
----------------------------------------------------------------------------
--- Seed Table : Payloads & PayloadDetails (Temp, Only For Testing)
----------------------------------------------------------------------------

IF NOT EXISTS(Select * From Payloads Where Transported = 0)
BEGIN
  Declare @PayloadId bigint
  Insert Into Payloads Values ((Select isnull(max(TransactionId), 0) From Payloads) + 1, 'MMC', GETDATE(), '\Payloads\Payload1', NULL, 0, NULL, 0, NULL)
  Set @PayloadId = @@Identity
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  

  Insert Into Payloads Values ((Select isnull(max(TransactionId), 0) From Payloads) + 1, 'MMC', GETDATE(), '\Payloads\Payload2', NULL, 0, NULL, 0, NULL)
  Set @PayloadId = @@Identity
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
  Insert Into PayloadDetails Values (@PayloadId, (Select isnull(max(ElectronicTraceTrackingId), 0) From PayloadDetails) + 1)
END
GO

----------------------------------------------------------------------------
GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[PayloadDetails] WITH CHECK CHECK CONSTRAINT [FK_Payload];


GO
PRINT N'Update complete.';


GO
