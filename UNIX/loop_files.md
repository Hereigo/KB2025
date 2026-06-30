```sh
#!/usr/bin/env bash

# Make filename globbing safer:
# If '*' matches nothing, it expands to an empty list instead of the literal '*'.
shopt -s nullglob

# Iterate over every item in the current directory.
for file in *; do

    # Skip the item if it is not a regular file
    # (ignores directories, symlinks, devices, etc.).
    [[ -f "$file" ]] || continue

    # Get the file size in bytes.
    # stat -c '%s' prints only the size.
    # -- marks the end of command options so filenames beginning with '-'
    # are treated as filenames instead of options.
    size=$(stat -c '%s' -- "$file")

    ext=""
    if [[ "$file" == *.* && "$file" != .* ]]; then
        ext=".${file##*.}"
    fi

    # DO THE NECESSARY STAFF HERE ...

    echo "$file"
done
```