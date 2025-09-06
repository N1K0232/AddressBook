CREATE TABLE [dbo].[People] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_People_Id] DEFAULT (newsequentialid()) NOT NULL,
    [CityId]         UNIQUEIDENTIFIER NOT NULL,
    [FirstName]      NVARCHAR (255)   NOT NULL,
    [LastName]       NVARCHAR (255)   NOT NULL,
    [BirthDate]      DATE             NOT NULL,
    [CreatedAt]      DATETIME         CONSTRAINT [DF_People_CreatedAt] DEFAULT (getutcdate()) NOT NULL,
    [LastModifiedAt] DATETIME         NULL,
    CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_People_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id])
);