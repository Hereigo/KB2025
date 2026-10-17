There are several ways to display progress when creating or extracting a Linux `.tar` archive.

## 1. Use `pv` (recommended)

`pv` (Pipe Viewer) displays a progress bar, data transferred, speed, and ETA.

Install it:

bash
```
# Debian / Ubuntu
sudo apt install pv

# Fedora
sudo dnf install pv

# Arch Linux
sudo pacman -S pv
```

Create an archive with progress:

bash
```
tar -cf - /path/to/directory | pv -s "$(du -sb /path/to/directory | cut -f1)" > archive.tar
```

Create a compressed `.tar.gz` archive with progress:

bash
```
tar -cf - /path/to/directory \
  | pv -s "$(du -sb /path/to/directory | cut -f1)" \
  | gzip > archive.tar.gz
```

Example output:

text
```
 450MiB 0:00:12 [37.5MiB/s] [=======>       ] 45% ETA 0:00:15
```

Note that `du -sb` estimates the input size; progress may not exactly match the final compressed archive size.

## 2. Use `tar`'s built-in verbose output

If you only need to see which files are being processed:

bash
```
tar -cvf archive.tar /path/to/directory
```

For gzip compression:

bash
```
tar -czvf archive.tar.gz /path/to/directory
```

The `-v` flag prints each filename as it is archived, but it does not show a percentage or ETA.

## 3. Display progress when extracting

With `pv`:

bash
```
pv archive.tar | tar -xvf -
```

For a gzip-compressed archive:

bash
```
pv archive.tar.gz | tar -xzvf -
```

These commands show bytes read from the archive, rather than the percentage of files extracted.

## 4. Use GNU tar's checkpoint feature

GNU `tar` can periodically print progress-like messages without installing anything:

bash
```
tar -cf archive.tar --checkpoint=1000 \
  --checkpoint-action=dot /path/to/directory
```

This prints a dot for every 1,000 archive records processed. It is useful for large archives, but it doesn't provide an accurate percentage or ETA.

## Which method should you use?

| Method | Progress bar | Percentage / ETA | Extra tool |
| --- | --- | --- | --- |
| `tar -v` | No | No | No |
| `tar --checkpoint` | No, dots only | No | No |
| `pv` | Yes | Yes, with a size estimate | Yes |

My recommendation: use `pv` for a real progress bar. If you want to avoid installing anything, use `tar -v` or `--checkpoint`.