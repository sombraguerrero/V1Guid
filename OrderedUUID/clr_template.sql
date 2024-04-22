EXEC sp_configure 'clr enabled', 1;  
RECONFIGURE;  
GO

CREATE ASYMMETRIC KEY <keyname> FROM EXECUTABLE FILE = '<path to dll>';     
CREATE LOGIN <username> FROM ASYMMETRIC KEY <keyname> ;    
GRANT UNSAFE ASSEMBLY TO <username> ;  
GO

CREATE ASSEMBLY UUID from '<path to dll>' WITH PERMISSION_SET = UNSAFE
GO

DROP FUNCTION IF EXISTS dbo.orderedUUIDTVF();
GO

CREATE FUNCTION dbo.orderedUUIDTVF(@num int = 1)  
RETURNS TABLE   
(ordered_uuid varbinary(32))
as
EXTERNAL NAME UUID.[OrderedUUID.Class1].InitMethod -- name of entrypoint function
GO

CREATE FUNCTION ordered_uuid() RETURNS VARBINARY(32)   
AS EXTERNAL NAME UUID.[OrderedUUID.Class1].ReturnOrderedGuid