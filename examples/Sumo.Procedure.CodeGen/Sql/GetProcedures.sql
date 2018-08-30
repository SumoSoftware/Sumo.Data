SELECT [SPECIFIC_NAME] as [Name], [specific_schema] as [Schema]  FROM INFORMATION_SCHEMA.ROUTINES
where ROUTINE_TYPE != 'FUNCTION'
order by 2, 1