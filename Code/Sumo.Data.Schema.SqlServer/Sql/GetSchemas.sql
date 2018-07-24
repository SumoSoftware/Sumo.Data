SELECT [schema_name] as [Name] FROM INFORMATION_SCHEMA.SCHEMATA
where 
	[schema_owner] = 'dbo'
	and [schema_name] not like 'db_%'
order by 1