CREATE PROCEDURE [Test].[Get]
	@Id bigint,
	@Out nvarchar(256) out
AS
	select @Out = [Name] 
	from Test.Test
	where Id = @Id;
RETURN 0
