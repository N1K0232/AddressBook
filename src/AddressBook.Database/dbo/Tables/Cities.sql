CREATE TABLE [dbo].[Cities] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (100)   NOT NULL,
    [CreatedAt]      DATETIME         NOT NULL,
    [LastModifiedAt] DATETIME         NULL
);

GO
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [PK_Cities] PRIMARY KEY ([Id] ASC);

GO
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [DF_Cities_Id] DEFAULT (NEWSEQUENTIALID()) FOR [Id];

GO
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [DF_Cities_CreatedAt] DEFAULT (GETUTCDATE()) FOR [CreatedAt];

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Cities_Name]
    ON [dbo].[Cities]([Name] ASC);