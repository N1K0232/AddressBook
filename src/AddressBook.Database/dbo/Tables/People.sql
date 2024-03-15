CREATE TABLE [dbo].[People]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(), 
    [FirstName] NVARCHAR(256) NOT NULL,
    [LastName] NVARCHAR(256) NOT NULL,
    [BirthDate] DATE NOT NULL,
    [Gender] VARCHAR(10) NOT NULL,
    [City] NVARCHAR(50) NOT NULL,
    [Province] VARCHAR(10) NOT NULL,
    [FiscalCode] NVARCHAR(50) NULL,
    [CellphoneNumber] VARCHAR(50) NULL,
    [EmailAddress] NVARCHAR(256) NULL,
    [CreationDate] DATETIME NOT NULL DEFAULT getutcdate(),
    [LastModifiedDate] DATETIME NULL,

    PRIMARY KEY([Id])
);

GO
    CREATE UNIQUE NONCLUSTERED INDEX [IX_FiscalCode]
    ON [dbo].[People]([FiscalCode]);

GO
    CREATE UNIQUE NONCLUSTERED INDEX [IX_CellphoneNumber]
    ON [dbo].[People]([CellphoneNumber]);

GO
    CREATE UNIQUE NONCLUSTERED INDEX [IX_EmailAddress]
    ON [dbo].[People]([EmailAddress]);