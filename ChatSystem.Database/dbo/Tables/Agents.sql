CREATE TABLE [dbo].[Agents] (
    [Id]           UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Name]         NVARCHAR (100)   NOT NULL,
    [Seniority]    NVARCHAR (20)    NOT NULL,
    [CurrentChats] INT              DEFAULT ((0)) NULL,
    [MaxChats]     INT              NOT NULL,
    [ShiftStart]   TIME (7)         NULL,
    [ShiftEnd]     TIME (7)         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

