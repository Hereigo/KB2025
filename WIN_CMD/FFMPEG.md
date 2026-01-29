```batch

REM Convert to MP3 - (%%~nf.mp3 = Name of File with new exten)
for %%f in (*.mp4) do ffmpeg -i %%f -vn -ar 44100 -ac 2 -b:a 128k %%~nf.mp3

REM Cut Video:
ffmpeg -ss 00:00:00 -to 01:59:59 -i input.mp4 -c copy output.mp4
REM or
ffmpeg -ss 00:00:00 -to 01:59:59 -i input.mp4 -acodec copy -vcodec copy output.mp4

REM Convert VIdeo format with simplify and minify with codec x265 
for %%f in (*.MOV) do ffmpeg -i %%f -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac %%~nf_New.MP4


```