$Signature = @"
[DllImport("rpcrt4.dll", SetLastError = true)]public static extern int UuidCreateSequential(out Guid guid);
"@
$V1Guid = Add-Type -MemberDefinition $Signature -Name "UUidCreateSequential" -Namespace rpcrt4 -PassThru

for ($i=1; $i -le 30; $i++)
{
	$z = [System.Guid]::Empty
	if ($V1Guid::UuidCreateSequential([ref] $z) -eq 0) { Write-Host $z }
}
pause