[CmdletBinding()]
param()
Trace-VstsEnteringInvocation $MyInvocation

try
{
	Write-Host "... Powershell Starting ..."

	$parameter = Get-VstsInput -Name parameter -Require

	Write-Host "Parameter : $($parameter)"

	$exeFileName = "tool/MongoSync.exe"
	$exeFilePath  = Join-Path $PSScriptRoot $exeFileName 

	$pinfo = New-Object System.Diagnostics.ProcessStartInfo 
	$pinfo.FileName = $exeFilePath
	$pinfo.Arguments = $parameter
	$pinfo.UseShellExecute = $false 
	$pinfo.CreateNoWindow = $true 
	$pinfo.RedirectStandardOutput = $true 
	$pinfo.RedirectStandardError = $true

	$process= New-Object System.Diagnostics.Process 
	$process.StartInfo = $pinfo
	$process.Start() | Out-Null 
	$process.WaitForExit()


	$stdout = $process.StandardOutput.ReadToEnd();

	Write-Host $stdout 

	Write-Host "... Powershell Step finished ..."

}finally{
	 Trace-VstsLeavingInvocation $MyInvocation
}