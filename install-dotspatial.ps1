<powershell>
# DotSpatial Auto-Installation Script for AWS EC2

# Install Chocolatey
Set-ExecutionPolicy Bypass -Scope Process -Force
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# Install Git
choco install git -y

# Install .NET 8 SDK
choco install dotnet-8.0-sdk -y

# Refresh environment
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

# Clone DotSpatial
cd C:\
git clone https://github.com/DotSpatial/DotSpatial.git

# Create desktop shortcut
$WshShell = New-Object -comObject WScript.Shell
$Shortcut = $WshShell.CreateShortcut("$env:USERPROFILE\Desktop\DotSpatial.lnk")
$Shortcut.TargetPath = "C:\DotSpatial"
$Shortcut.Save()

Write-Host "✅ DotSpatial installed successfully at C:\DotSpatial"
</powershell>
