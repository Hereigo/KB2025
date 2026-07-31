**Watch it update continuously**

   ```bash
   watch -n 2 mega-sync
   ```

   or

   ```bash
   while true; do
       clear
       mega-sync
       sleep 2
   done
   ```

**View active transfers**

   ```bash
   mega-transfers
   ```

This lists uploads/downloads currently in progress and is useful for seeing whether data is still being transferred. The command supports additional options for filtering and managing transfers. ([GitHub][1])

MEGAcmd does **not** provide an overall sync completion percentage for `mega-sync` or `mega-backup`.

However, there are a few workarounds:

**Compare local vs. remote size**

```bash
du -sb /path/to/local
mega-du /remote/path
```

If the remote size is close to the local size, you're nearly finished. This is only an approximation because metadata and skipped files can affect the totals.

**Count files**

```bash
find /path/to/local -type f | wc -l
mega-find /remote/path | wc -l
```

Comparing file counts can give a rough estimate of progress.

**Monitor active transfers**

```bash
mega-transfers
```

When this becomes empty and:

```bash
mega-sync
```

### If you want a live percentage

a small script that periodically compares:

* local bytes vs. remote bytes, or
* local file count vs. remote file count,

and computes:

```text
progress = remote_size / local_size × 100%
```

(or the equivalent using file counts).
