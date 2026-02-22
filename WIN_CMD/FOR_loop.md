```batch

set archive=My_Archive.zip
set params=one two three four ...

echo off

REM for %%p in (%params%) do (
for /f %%p in (params.txt) do (
    echo Setting parameter to %%p
    REM Extract with auto-rename if exists already.
	"C:\Program Files\7-Zip\7z.exe" x %archive% -aou -p%%p
)
```