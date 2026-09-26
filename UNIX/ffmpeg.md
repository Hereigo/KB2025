```bash
#!/bin/bash

# for file in /home/user/Downloads/TEMP/*.mp4
# do
#  #ffmpeg -ss 00:00:00 -to 00:59:59 -i "$file" -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac "${file/OLD_PATH/NEW_PATH}"
#   ffmpeg -i "$file" -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac "${file/.mp4/_B.mp4}"
# done

dir="/home/user/Downloads"

# Do if any .mp3 files exist in the directory
if ls "$dir"/*.mp3 1> /dev/null 2>&1; then 
  for file in "$dir"/*.mp3
  do
	lame --mp3input -b 64 "$file" "${file/.mp3/_out.mp3}"  
  done
else
  echo "No .mp3 files found in the directory."
fi

# Do if any .mp4 files exist in the directory
if ls "$dir"/*.mp4 1> /dev/null 2>&1; then 
  for file in "$dir"/*.mp4
  do
    # ffmpeg -ss 00:00:00 -to 00:04:40 -i "$file" -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac "${file/.mp4/_A.mp4}"
    # ffmpeg -ss 00:06:38 -to 00:08:03 -i "$file" -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac "${file/.mp4/_B.mp4}"
    ffmpeg -i "$file" -c:v libx265 -pix_fmt yuv420p -crf 25 -preset fast -tune animation -c:a aac "${file/.mp4/_B.mp4}"
  done
else
  echo "No .mp4 files found in the directory."
fi

# Explanation:
# 
# ls "$dir"/*.mp3: Lists all .mp3 files in the specified directory.
# 1> /dev/null 2>&1: Redirects both standard output (stdout) and error output (stderr) to /dev/null to suppress unnecessary output.
# If there are .mp3 files, the script prints a message saying they exist. Otherwise, it prints a message saying no .mp3 files are found.

##############################

dir="/path/to/directory"

# Check if any files exist
# if [ "$(ls -A $dir)" ]; then
#   # Loop through all files in the directory
#   for file in "$dir"/*; do
#     if [ -f "$file" ]; then
#       # Specify the new name pattern, for example, prefix with 'new_'
#       new_name="$dir/new_$(basename "$file")"
#       
#       # Rename the file
#       mv "$file" "$new_name"
#     fi
#   done
# else
#   echo "No files found in the directory."
# fi

# Explanation:
# 
#     if [ "$(ls -A $dir)" ]: Checks if the directory contains any files.
#     for file in "$dir"/*: Loops through all files in the directory.
#     basename "$file": Gets the name of the file without the directory part.
#     mv "$file" "$new_name": Renames the file.
# 
# You can adjust the new_name variable to modify the renaming logic based on your needs.

```