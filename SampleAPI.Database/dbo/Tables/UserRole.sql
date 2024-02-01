CREATE TABLE [dbo].[UserRole] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [RoleDescription] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_dbo.UserRole] PRIMARY KEY CLUSTERED ([Id] ASC)
);

