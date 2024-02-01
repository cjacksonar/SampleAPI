
CREATE PROCEDURE [dbo].[USP_CreateUserRoles] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM dbo.UserRole
	DBCC CHECKIDENT ('UserRole', RESEED,1)

	INSERT INTO dbo.UserRole (RoleDescription) VALUES ('Admin')
	INSERT INTO dbo.UserRole (RoleDescription) VALUES ('User')
					 
END