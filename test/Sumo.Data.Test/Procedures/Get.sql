CREATE PROCEDURE [Test].[Get]
	@Id bigint,
	@Status int out
AS

	select @Status = [Status]
	from Test.Test
	where Id = @Id;

	select *
	from Test.Test
	where Id = @Id;

	return -1;
RETURN 0
