CREATE VIEW [dbo].[ViewContributionsList]
AS
SELECT        dbo.Contribution.Id AS ContributionId, dbo.Fund.FundName, dbo.Contributor.Name AS ContributorName, dbo.Contribution.ContributionDate, dbo.Contribution.Amount, ISNULL(dbo.Contribution.CheckNumber, '0') AS CheckNumber, 
                         ISNULL(dbo.Contribution.Comments, '') AS Comments
FROM            dbo.Contributor INNER JOIN
                         dbo.Contribution ON dbo.Contributor.Id = dbo.Contribution.ContributorId INNER JOIN
                         dbo.Fund ON dbo.Contribution.FundId = dbo.Fund.Id