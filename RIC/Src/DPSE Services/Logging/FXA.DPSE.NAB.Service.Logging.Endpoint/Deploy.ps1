Import-Module WebDeployment,Transformation

$appPoolName = "$ServiceName AppPool"

$installPath = Join-Path $InstallRoot $ServiceName
If(Test-Path $installPath)
{
    Write-Host "Directory $installPath already exists, removing..."
    Remove-Item $installPath -Recurse
}

Write-Host "Copying $PSScriptRoot to $installPath..."
Copy-Item $PSScriptRoot $installPath -Recurse

Template-Configuration -Template "$installPath\Web.csconfig" -Target "$installPath\Web.config" -Clean
Deploy-WebApplicationPool -ApplicationPoolName $appPoolName -Username $ServiceUsername -Password $ServicePassword
Deploy-WebSite -Name $ServiceName -ApplicationPoolName $appPoolName -PhysicalPath $installPath -Port $Port -EnabledProtocols "http"
