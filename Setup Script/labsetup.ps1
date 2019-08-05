if(
([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
)
{
write-host "Getting and installing chocolatey package manager" -ForegroundColor green
Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

write-host "Installing latest version of git" -ForegroundColor green
choco install git -params '"/GitAndUnixToolsOnPath"' -y -f 

Write-Host "Installing latest version of VScode" -ForegroundColor green
choco install vscode -y -f

Write-Host "pulling latest course files from github" -ForegroundColor green
Invoke-Expression 'cmd /c start powershell -Command {
    cd f:\ 
    "C:\Program Files\Git\cmd\git.exe" clone "https://github.com/microsoftlearning/AZ-203-DevelopingSolutionsForMicrosoftAzure"
    Write-Host "Linking the coursefile folder" -ForegroundColor green
    New-Item -ItemType SymbolicLink -target "f:\AZ-203-DevelopingSolutionsForMicrosoftAzure\Allfiles" -path "f:\allfiles"
    pause
}'


Write-Host "Completed setup" -ForegroundColor green
}
else{
Write-Host "Please run this in an admin shell" -ForegroundColor Yellow
}
