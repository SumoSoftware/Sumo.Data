CREATE PROCEDURE [Test].[ParamContextTest]
	@InputDate datetime,
	@OutputDate datetime = null output
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	set @OutputDate = @InputDate;

    -- Insert statements for procedure here
    return -1;
END
