Import-Module Database

$sqlPackageExe = Join-Path (get-item $PSScriptRoot).Parent.Parent.FullName "tools\sqlpackage\SqlPackage.exe" 
$dacpacPath = Join-Path $PSScriptRoot "FXA.DPSE.DB.PaymentInstruction.dacpac"

Publish-DacPac -Path $dacpacPath -TargetDatabase $DatabaseName -TargetServer $DatabaseServer -SqlPackageExe $sqlPackageExe
