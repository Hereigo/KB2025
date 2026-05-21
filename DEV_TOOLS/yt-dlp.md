```bash

yt-dlp -o %(title)s.%(ext)s [VIDEO_URL_HERE]

yt-dlp -f bestaudio [VIDEO_URL_HERE]
yt-dlp -x -audio-format mp3 [VIDEO_URL_HERE]
yt-dlp -x –audio-format mp3 –audio-quality 0 [VIDEO_URL_HERE]
yt-dlp -x –audio-format mp3 –audio-quality 0 –embed-thumbnail –add-metadata -o %(title)s.%(ext) [VIDEO_URL_HERE]

# See Avalable formats:
yt-dlp -F [VIDEO_URL_HERE]

# See more details:
yt-dlp -F -v [VIDEO_URL_HERE] 

# Download Selected by Number:
yt-dlp -f 123 [VIDEO_URL_HERE]

```