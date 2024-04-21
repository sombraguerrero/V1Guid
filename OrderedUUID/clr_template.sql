EXEC sp_configure 'clr enabled', 1;  
RECONFIGURE;  
GO

CREATE ASYMMETRIC KEY <keyname> FROM EXECUTABLE FILE = '<path to dll>';     
CREATE LOGIN <username> FROM ASYMMETRIC KEY <keyname> ;    
GRANT UNSAFE ASSEMBLY TO <username> ;  
GO

CREATE ASSEMBLY <assembly name> from '<path to dll>' WITH PERMISSION_SET = UNSAFE
GO

DROP FUNCTION IF EXISTS <schema>.<function name>;
GO

CREATE FUNCTION <schema>.functionName(<params>)  
RETURNS TABLE   
(<field> <datatype>)
as
EXTERNAL NAME <assembly name>.[<namespace>.<classname>].InitMethod -- name of entrypoint function
GO
