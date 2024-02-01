CREATE TABLE [dbo].[Contributor] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50)   NOT NULL,
    [Address]   NVARCHAR (50)   NOT NULL,
    [City]      NVARCHAR (50)   NOT NULL,
    [StateCode] NVARCHAR (2)    NOT NULL,
    [ZipCode]   NVARCHAR (10)   NOT NULL,
    [Phone]     NVARCHAR (14)   NULL,
    [Email]     NVARCHAR (50)   NULL,
    [Comments]  NVARCHAR (1024) NULL,
    [ProductId] NVARCHAR (15)   NOT NULL,
    CONSTRAINT [PK_dbo.Contributor] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Contributor_Registration] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Registration] ([ProductId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Contributor_State] FOREIGN KEY ([StateCode]) REFERENCES [dbo].[State] ([StateCode])
);

