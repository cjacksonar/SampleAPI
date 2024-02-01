CREATE TABLE [dbo].[State] (
    [StateCode] NVARCHAR (2)  NOT NULL,
    [StateName] NVARCHAR (40) NULL,
    CONSTRAINT [PK_dbo.State] PRIMARY KEY CLUSTERED ([StateCode] ASC)
);

