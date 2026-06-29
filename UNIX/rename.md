```sh
#!/bin/bash

VIDEO_DIR="/home/user/VIDEOS"

find "$VIDEO_DIR" -maxdepth 1 -type f | while IFS= read -r file; do
    duration=$(ffprobe \
        -v error \
        -show_entries format=duration \
        -of default=noprint_wrappers=1:nokey=1 \
        "$file")

    # Format as 10 digits: 4 decimal before then point and 5 after
    prefix=$(printf "%10.5f" "$duration")

    dir=$(dirname "$file")
    base=$(basename "$file")

    # Get the file size in bytes.
    # stat -c '%s' prints only the size.
    # -- marks the end of command options so filenames beginning with '-'
    # are treated as filenames instead of options.
    size=$(stat -c '%s' -- "$file")
    
    ext=""
    if [[ "$file" == *.* && "$file" != .* ]]; then
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
```