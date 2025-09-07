CREATE TABLE [dbo].[People] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [CityId]         UNIQUEIDENTIFIER NOT NULL,
    [FirstName]      NVARCHAR (255)   NOT NULL,
    [LastName]       NVARCHAR (255)   NOT NULL,
    [BirthDate]      DATE             NOT NULL,
    [CreatedAt]      DATETIME         NOT NULL,
    [LastModifiedAt] DATETIME         NULL,
);

GO
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People] PRIMARY KEY ([Id] ASC);

GO
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [FK_People_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities]([Id]);

GO
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [DF_People_Id] DEFAULT (NEWSEQUENTIALID()) FOR [Id];

GO
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [DF_People_CreatedAt] DEFAULT (GETUTCDATE()) FOR [CreatedAt];