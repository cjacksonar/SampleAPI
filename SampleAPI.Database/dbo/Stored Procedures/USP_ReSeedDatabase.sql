CREATE PROCEDURE [dbo].[USP_ReSeedDatabase] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE FROM [Fund]
	DELETE FROM [RegisteredUser]
	DELETE FROM [Registration]
	DELETE FROM [Contribution]
	DELETE FROM [Contributor]
--------------------------------------------------------------------------------------------------------------------------------------------------------------
	DBCC CHECKIDENT ( 'Fund', RESEED, 0)
	DBCC CHECKIDENT ( 'RegisteredUser', RESEED, 0)
	DBCC CHECKIDENT ( 'Contribution', RESEED, 0)
	DBCC CHECKIDENT ( 'Contributor', RESEED, 0)				 
END