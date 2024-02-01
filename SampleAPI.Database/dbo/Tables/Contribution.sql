CREATE TABLE [dbo].[Contribution] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [ContributionDate] DATETIME        NOT NULL,
    [Amount]           DECIMAL (18, 2) NOT NULL,
    [CheckNumber]      INT             NOT NULL DEFAULT 0,
    [Comments]         NVARCHAR (1024) NULL,
    [FundId]           INT             NOT NULL,
    [ContributorId]    INT             NULL,
    [ProductId]        NVARCHAR (15)   NOT NULL,
    CONSTRAINT [PK_dbo.Contribution] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Contribution_dbo.Contributor_ContributorId] FOREIGN KEY ([ContributorId]) REFERENCES [dbo].[Contributor] ([Id]),
    CONSTRAINT [FK_dbo.Contribution_dbo.Fund_FundId] FOREIGN KEY ([FundId]) REFERENCES [dbo].[Fund] ([Id]) ON DELETE CASCADE
);

