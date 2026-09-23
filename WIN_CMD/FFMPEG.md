```bat
REM Cut single video
ffmpeg -ss 00:00:00 -to 01:59:59 -i input.mp4 -c copy output.mp4

REM Batch operations
for %%f in (*.mp4 *.mkv *.mov) do (

    REM Convert to MP3 	
    ffmpeg -ss 00:00:00 -i "%%f" -vn -ar 44100 -ac 2 -b:a 128k "%%~nf.%%~xf.mp3"

    REM Cut Video:
    ffmpeg -ss 00:00:00 -to 01:59:59 -i "%%f" -c copy "%%~nf_NEW_%%~xf"
    
    REM Convert VIdeo format with simplify and minify using codec x265  
    ffmpeg -i "%%f" -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac "%%~nf_NEW_%%~xf"
)

REM %%~nf - Name of File
REM %%~xf - Xtension of File

```