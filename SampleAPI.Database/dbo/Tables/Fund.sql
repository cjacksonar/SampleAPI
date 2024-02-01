CREATE TABLE [dbo].[Fund] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [FundName]  NVARCHAR (24)   NOT NULL,
    [Comments]  NVARCHAR (1024) NULL,
    [ProductId] NVARCHAR (15)   NOT NULL,
    CONSTRAINT [PK_dbo.Fund] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Fund_Registration] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Registration] ([ProductId]) ON DELETE CASCADE
);

