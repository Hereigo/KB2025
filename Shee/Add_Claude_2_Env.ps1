# if "claude" command not recognized:

$bin = "$env:USERPROFILE\.local\bin"
$current = [Environment]::GetEnvironmentVariable("Path", "User")
if (($current -split ";") -notcontains $bin) {
    [Environment]::SetEnvironmentVariable("Path", "$current;$bin", "User")
}

# Then close and reopen your terminal (or restart VS Code, if you use it).