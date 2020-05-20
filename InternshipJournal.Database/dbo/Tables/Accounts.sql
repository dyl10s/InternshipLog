CREATE TABLE [dbo].[Accounts] (
    [AccountId] INT             IDENTITY (1, 1) NOT NULL,
    [Username]  VARCHAR (MAX)   NOT NULL,
    [Password]  VARBINARY (MAX) NOT NULL,
    [Company]   VARCHAR (MAX)   NULL,
    [Name]      VARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([AccountId] ASC)
);

