

CREATE PROCEDURE [dbo].[USP_CreateTestData] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;			
	EXEC USP_CreateUserRoles
	EXEC USP_CreateStates

	DECLARE @ProductId varchar(15) 
	SELECT @ProductId = '5000A-400-3000B'

	DECLARE @FundId int
	DECLARE @ContributorId int	
	SELECT @FundId = 1
	SELECT @ContributorId = 1

	INSERT INTO dbo.Registration (ProductId,OrganizationName,PrimaryContactName,PrimaryContactEmail,AdminUserId,AdminUserPassword) 
	VALUES (@ProductId,'Demo organization','Amy Gordon','amy.gordon@gmail.com','admin','admin')	
	
	--INSERT INTO dbo.Fund(ProductId,FundName, Comments) 	VALUES (@ProductId,'General','General purpose fund')
	INSERT INTO dbo.Fund(ProductId,FundName, Comments) 	VALUES (@ProductId,'Savings','Savings fund')

	INSERT INTO dbo.Contributor([Name],[Address],[City],[StateCode],[ZipCode],[Phone],[Email],[Comments],[ProductId])
	VALUES ('Anne Dodsworth','1000 Broadway Ave','Council Bluffs','IA','51501',NULL,NULL,NULL,@ProductId)

	INSERT INTO dbo.Contributor([Name],[Address],[City],[StateCode],[ZipCode],[Phone],[Email],[Comments],[ProductId])
	VALUES ('Laura Callahan','2500 Avenue I','Council Bluffs','IA','51501',NULL,NULL,NULL,@ProductId)

	INSERT INTO dbo.Contributor([Name],[Address],[City],[StateCode],[ZipCode],[Phone],[Email],[Comments],[ProductId])
	VALUES ('Robert King','100 Tunnel Road','Council Bluffs','IA','51501',NULL,NULL,NULL,@ProductId)

	-- General fund contributions for all contributors	
	WHILE(@ContributorId <= 3)
	BEGIN
		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('11/13/2023',100.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('12/13/2023',200.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('01/10/2022',300.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('02/23/2022',400.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('03/05/2022',500.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('04/12/2022',600.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('05/03/2022',700.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('06/13/2022',800.00,0,null,@FundId,@ContributorId,@ProductId)

		SET @ContributorId += 1
	END

	-- Savings fund contributions for all contributors	
	SELECT @FundId = 2, @ContributorId = 1
	WHILE(@ContributorId <= 3)
	BEGIN
		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('11/01/2023',1000.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('12/01/2023',2000.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('01/01/2022',3000.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('02/01/2022',4000.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('03/01/2022',5000.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('04/01/2022',6000.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('05/01/2022',7000.00,0,null,@FundId,@ContributorId,@ProductId)

		INSERT INTO dbo.Contribution([ContributionDate],[Amount],[CheckNumber],[Comments],[FundId],[ContributorId],[ProductId])
		VALUES ('06/01/2022',8000.00,0,null,@FundId,@ContributorId,@ProductId)

		SET @ContributorId += 1
	END
END