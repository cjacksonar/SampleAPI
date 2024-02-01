CREATE TABLE [dbo].[Registration] (
    [ProductId]           NVARCHAR (15)  NOT NULL,
    [OrganizationName]    NVARCHAR (100) NOT NULL,
    [PrimaryContactName]  NVARCHAR (50)  NOT NULL,
    [PrimaryContactEmail] NVARCHAR (255) NOT NULL,
    [AdminUserId]         NVARCHAR (24)  NOT NULL,
    [AdminUserPassword]   NVARCHAR (12)  NOT NULL,
    CONSTRAINT [PK_Registration] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);


GO
-- ================================================
-- Author:		Curtis Jackson
-- Create date: 05.19.2022
-- Description:	Insert Seed data after registration 
-- ================================================
CREATE TRIGGER [dbo].[RegistrationInsertTrigger] on [dbo].[Registration]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ProductId VARCHAR(15)
	DECLARE  @AdminUserId VARCHAR(24)
	DECLARE  @AdminUserPassword VARCHAR(12)
	DECLARE  @AdminUserName VARCHAR(50)
	DECLARE  @AdminUserEmail VARCHAR(255)

    SELECT @ProductId = ins.ProductId FROM INSERTED ins;
	SELECT @AdminUserId = ins.AdminUserId FROM INSERTED ins;
	SELECT @AdminUserPassword = ins.AdminUserPassword FROM INSERTED ins;
	SELECT @AdminUserName = ins.PrimaryContactName FROM INSERTED ins;
	SELECT @AdminUserEmail = ins.PrimaryContactEmail FROM INSERTED ins;
	
	-- insert admin login 
	INSERT INTO 
		dbo.RegisteredUser (ProductId,UserId,UserPassword,UserName,UserEmail,UserRoleId, AllowEditing) 
	VALUES
	  (@ProductId,@AdminUserId,@AdminUserPassword,@AdminUserName, @AdminUserEmail,1, 0)

	-- insert fund seed data 
	INSERT INTO 
		dbo.Fund(ProductId,FundName, Comments) 
	VALUES
	  (@ProductId,'General','General purpose fund')
	
	--PRINT 'Successfully inserted registration seed data for new Product Id.'
END