CREATE TABLE [dbo].[Cities] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_Cities_Id] DEFAULT (newsequentialid()) NOT NULL,
    [Name]           NVARCHAR (100)   NOT NULL,
    [CreatedAt]      DATETIME         CONSTRAINT [DF_Cities_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [LastModifiedAt] DATETIME         NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Cities_Name]
    ON [dbo].[Cities]([Id] ASC);