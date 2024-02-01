CREATE TABLE [dbo].[RegisteredUser] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ProductId]      NVARCHAR (15)  NOT NULL,
    [UserId]         NVARCHAR (24)  NOT NULL,
    [UserPassword]   NVARCHAR (12)  NOT NULL,
    [UserName]       NVARCHAR (50)  NOT NULL,
    [UserEmail]      NVARCHAR (255) NULL,
    [UserRoleId]     INT            CONSTRAINT [DF_RegisteredUser_UserRoleId] DEFAULT ((2)) NOT NULL,
    [AllowEditing]   BIT            CONSTRAINT [DF__Registere__Allow__5441852A] DEFAULT ((2)) NOT NULL,
    [NumberOfLogins] INT            CONSTRAINT [DF__Registere__Numbe__5535A963] DEFAULT ((1)) NOT NULL,
    [LastLogin]      DATE           DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_dbo.RegisteredUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.RegisteredUser_dbo.UserRole_UserRoleId] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RegisteredUser_Registration] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Registration] ([ProductId]) ON DELETE CASCADE,
    CONSTRAINT [UQ_UserId_ProductId] UNIQUE NONCLUSTERED ([ProductId] ASC, [UserId] ASC)
);

