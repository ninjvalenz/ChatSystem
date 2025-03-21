CREATE TABLE [dbo].[ChatQueue] (
    [Id]         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [SessionId]  UNIQUEIDENTIFIER NOT NULL,
    [UserId]     NVARCHAR (50)    NOT NULL,
    [CreatedAt]  DATETIME         DEFAULT (getdate()) NULL,
    [Processed]  BIT              DEFAULT ((0)) NULL,
    [IsOverflow] BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

