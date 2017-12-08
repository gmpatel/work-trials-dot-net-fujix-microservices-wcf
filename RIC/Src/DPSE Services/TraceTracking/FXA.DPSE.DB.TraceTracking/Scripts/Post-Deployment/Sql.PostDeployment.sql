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

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE object_id = OBJECT_ID(N'[dbo].[fn_LeftPad]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[fn_LeftPad]
END
GO

CREATE FUNCTION [dbo].[fn_LeftPad]
(
	@String NVARCHAR(MAX), -- Initial string
	@TargetLength INT,     -- Size of final string
	@Pad CHAR              -- Pad character
) RETURNS NVARCHAR(MAX)
AS
BEGIN
	RETURN ISNULL(REPLICATE(@Pad, @TargetLength - LEN(@String)), '') + @String
END
GO

IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE Name = 'Trigger_GenerateChequeTraceTrackingCode' AND type = 'TR')
BEGIN
  DROP TRIGGER [dbo].[Trigger_GenerateChequeTraceTrackingCode]
END
GO

CREATE TRIGGER [dbo].[Trigger_GenerateChequeTraceTrackingCode] ON [dbo].[ElectronicTraceTrackingDetails] FOR INSERT AS
BEGIN
       declare @RowId bigint
       declare @ForeignRowId bigint
       declare @ChannelType varchar(50)
       declare @DateTimeCreated datetime2
       declare @ChequeTraceTrackingCode varchar(50)

       select @RowId = I.Id from Inserted I
       select @ForeignRowId = I.ElectronicTraceTrackingId from Inserted I
       select @ChannelType = ChannelType From ElectronicTraceTracking Where Id = @ForeignRowId  
       select @DateTimeCreated = DateTimeCreated From ElectronicTraceTracking Where Id = @ForeignRowId 
       
       set @ChequeTraceTrackingCode = 'NAB' + convert(NVARCHAR(8), @DateTimeCreated, 112) + CASE UPPER(@ChannelType) WHEN 'AFS-MIB' THEN '101' ELSE '100' END + [dbo].[fn_LeftPad](Convert(varchar(50), @RowId % 1000000), 6, 0)
       update ElectronicTraceTrackingDetails Set ChequeTraceTrackingCode = @ChequeTraceTrackingCode Where Id = @RowId
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ElectronicTraceTracking]') AND name = N'IX_ElectronicTraceTracking_Duplicate')
BEGIN
  DROP INDEX [IX_ElectronicTraceTracking_Duplicate] ON [dbo].[ElectronicTraceTracking]
END
GO

CREATE NONCLUSTERED INDEX [IX_ElectronicTraceTracking_Duplicate] ON [dbo].[ElectronicTraceTracking]
(
                [ClientIpAddressV4] ASC,
                [ClientDeviceId] ASC,
                [ChannelType] ASC,
                [TotalTransactionAmount] ASC,
                [DepositAccountBsbCode] ASC,
                [DepositAccountName] ASC,
                [DepositAccountNumber] ASC,
                [DepositAccountType] ASC ,
                [DateTimeCreated] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO