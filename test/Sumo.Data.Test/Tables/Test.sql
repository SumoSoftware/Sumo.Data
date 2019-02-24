CREATE TABLE [Test].[Test]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
	[Status] int not null,
	CONSTRAINT [TestPk] PRIMARY KEY CLUSTERED ([Id] ASC),
)