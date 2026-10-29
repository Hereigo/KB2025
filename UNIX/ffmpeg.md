```sh

#!/bin/bash

# Define directories
dir="/home/user/Videos"

for source_file in "$dir"/*.{mp4,mov,mkv,MP4,MOV,MKV}; do

    # Skip if no .mp4 files are found
    [ -e "$source_file" ] || continue

    filename=$(basename -- "$source_file")
    filename_noext="${filename%.*}"
    converted_file="${dir}/${filename_noext}_AAA.mp4"

    # to CUT:
    # ffmpeg -ss 00:00:00 -to 23:59:59 -i "$file" -c:v ....

    ffmpeg -i "$source_file" -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac "$converted_file"
    wait
    echo "Created - $converted_file"

    source_size=$(stat -c %s "$source_file")
    converted_size=$(stat -c %s "$converted_file")

    if [ "$source_size" -gt "$converted_size" ]; then
        rm "$source_file"
    else
        rm "$converted_file"
    fi
done

echo "Convertation done."

find "$dir" -maxdepth 1 -type f | while IFS= read -r file; do
    duration=$(ffprobe \
        -v error \
        -show_entries format=duration \
        -of default=noprint_wrappers=1:nokey=1 \
        "$file")

    # Format as 10 digits: (4 decimal before) + . + (5 after)
    # (with leading zeros if necessary)
    prefix=$(printf "%010.5f" "$duration")

    dir=$(dirname "$file")
    base=$(basename "$file")

    # Get the file size in bytes.
    # stat -c '%s' prints only the size.
    # -- marks the end of command options so filenames beginning with '-'
    # are treated as filenames instead of options.
    size=$(stat -c '%s' -- "$file")
    
    ext=""
    # Must be fixed for filename only with no path!!!
    # if [[ "$file" == *.* && "$file" != .* ]]; then
    if [[ "$file" == *.* ]]; then
        ext=".${file##*.}"
    fi

    newname="${dir}/${prefix}_${size}${ext}"
    
    i=1
    # Repeat while:
    #   1. A file with the desired name already exists, and
    #   2. That existing file is not the file we're currently processing.
    #
    # The second condition prevents treating the current file as a collision
    # if it already has the desired name.
    while [[ -e "$newname" && "$newname" != "$file" ]]; do
        newname="${size}_$i${ext}"
        ((i++))
    done

    echo "Renaming:"
    echo "  $base"
    echo "  -> $(basename "$newname")"

    # Rename the file.
    # -- prevents filenames beginning with '-' from being treated as options.
    mv -- "$file" "$newname"
done

# poweroff
```