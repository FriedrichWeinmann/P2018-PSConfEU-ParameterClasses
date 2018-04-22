Set-Content -Path $profile.CurrentUserCurrentHost -Value @'
$presentationRoot = "D:\Code\Github\P2018-PSConfEU-ParameterClasses"

$temp = Get-PSFConfigValue -FullName psutil.path.temp
$null = New-Item -Path $temp -Name demo -ItemType Directory -Force -ErrorAction Ignore
$demo = Get-Item "$temp\demo"
$null = New-PSDrive -PSProvider FileSystem -Name demo -Root $demo.FullName -ErrorAction Ignore
Set-Location demo:
function prompt 
{
    "" + $ExecutionContext.SessionState.Path.CurrentLocation + "> "
}
Add-Type -Path "$presentationRoot\library\ParameterClasses\bin\Debug\netstandard2.0\ParameterClasses.dll"
Set-PSFTypeAlias -AliasName "DateTimeSharpA" -TypeName "ParameterClasses.DateTimeSharpA"
Set-PSFTypeAlias -AliasName "DateTimeSharpB" -TypeName "ParameterClasses.DateTimeSharpB"
Set-PSFTypeAlias -AliasName "DateTimeSharpC" -TypeName "ParameterClasses.DateTimeSharpC"
Set-PSFTypeAlias -AliasName "DateTimeSharpD" -TypeName "ParameterClasses.DateTimeSharpD"
Set-PSFTypeAlias -AliasName "DateTimeSharpE" -TypeName "ParameterClasses.DateTimeSharpE"

. "$presentationRoot\powershell\Get-ParameterClassInfo.ps1"
'@

$presentationRoot = "D:\Code\Github\P2018-PSConfEU-ParameterClasses"

$temp = Get-PSFConfigValue -FullName psutil.path.temp
$null = New-Item -Path $temp -Name demo -ItemType Directory -Force -ErrorAction Ignore
$demo = Get-Item "$temp\demo"
$null = New-PSDrive -PSProvider FileSystem -Name demo -Root $demo.FullName -ErrorAction Ignore
Set-Location demo:
function prompt 
{
    "" + $ExecutionContext.SessionState.Path.CurrentLocation + "> "
}
Add-Type -Path "$presentationRoot\library\ParameterClasses\bin\Debug\netstandard2.0\ParameterClasses.dll"
Set-PSFTypeAlias -AliasName "DateTimeSharpA" -TypeName "ParameterClasses.DateTimeSharpA"
Set-PSFTypeAlias -AliasName "DateTimeSharpB" -TypeName "ParameterClasses.DateTimeSharpB"
Set-PSFTypeAlias -AliasName "DateTimeSharpC" -TypeName "ParameterClasses.DateTimeSharpC"
Set-PSFTypeAlias -AliasName "DateTimeSharpD" -TypeName "ParameterClasses.DateTimeSharpD"
Set-PSFTypeAlias -AliasName "DateTimeSharpE" -TypeName "ParameterClasses.DateTimeSharpE"

. "$presentationRoot\powershell\Get-ParameterClassInfo.ps1"