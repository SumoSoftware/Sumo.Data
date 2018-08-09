SELECT [table_name] as [Name], [table_schema] as [Schema]  FROM INFORMATION_SCHEMA.TABLES
where 
	[table_schema] != 'sys'
	and [table_type] = 'BASE TABLE'
order by 2, 1