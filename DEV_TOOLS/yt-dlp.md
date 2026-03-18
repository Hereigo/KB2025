```bash

yt-dlp -x --audio-format mp3 [VIDEO_URL_HERE]

yt-dlp -o %(title)s.%(ext)s [VIDEO_URL_HERE]

yt-dlp -x –audio-format mp3 –audio-quality 0 –embed-thumbnail –add-metadata -o %(title)s.%(ext) [VIDEO_URL_HERE]

```