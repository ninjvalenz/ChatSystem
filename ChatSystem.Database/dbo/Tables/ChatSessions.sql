CREATE TABLE [dbo].[ChatSessions] (
    [SessionId]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [UserId]          NVARCHAR (50)    NOT NULL,
    [CreatedAt]       DATETIME         DEFAULT (getdate()) NULL,
    [IsActive]        BIT              DEFAULT ((1)) NULL,
    [AssignedAgentId] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([SessionId] ASC),
    FOREIGN KEY ([AssignedAgentId]) REFERENCES [dbo].[Agents] ([Id])
);

