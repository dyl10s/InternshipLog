CREATE TABLE [dbo].[Entries] (
    [EntryId]   INT           IDENTITY (1, 1) NOT NULL,
    [StartTime] DATETIME      NOT NULL,
    [EndTime]   DATETIME      NULL,
    [Details]   VARCHAR (MAX) NOT NULL,
    [AccountId] INT           NOT NULL,
    CONSTRAINT [PK_Entry] PRIMARY KEY CLUSTERED ([EntryId] ASC)
);

