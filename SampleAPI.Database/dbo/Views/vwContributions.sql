
CREATE VIEW [dbo].[vwContributions]
AS
SELECT        dbo.Contributor.Id AS ContributorId, dbo.Contributor.Name, dbo.Contributor.Address, dbo.Contributor.City, dbo.Contributor.StateCode, dbo.Contributor.ZipCode, dbo.Contributor.Phone, dbo.Contributor.Email, 
                         dbo.Contributor.Comments, dbo.Contribution.Id AS ContributionId, dbo.Contribution.ContributionDate, dbo.Contribution.Amount, dbo.Contribution.CheckNumber, dbo.Contribution.Comments AS ContributionComments, 
                         dbo.Fund.Id AS FundId, dbo.Fund.FundName, dbo.Contributor.ProductId
FROM            dbo.Contributor INNER JOIN
                         dbo.Contribution ON dbo.Contributor.Id = dbo.Contribution.ContributorId INNER JOIN
                         dbo.Fund ON dbo.Contribution.FundId = dbo.Fund.Id