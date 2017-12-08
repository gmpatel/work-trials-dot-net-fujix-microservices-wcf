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