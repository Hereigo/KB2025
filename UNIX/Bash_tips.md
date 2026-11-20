```sh
# Check syntax in a script:

bash -n your_script_name
```
-------------------------

```sh
#!/bin/bash
#=====================================
# Change many files Encoding.
#=====================================
# iconv -f ENCODING_FROM -t ENCODING_INTO > NEW_RECODED_FILE
# CONFIG:
TYPES="*.txt";
FROM='cp1251';
TO='utf8';
FILEPrefix='.utf8';

# START:
if [ -z $1 ]
then
        echo "Recoding files: $TYPES from the Dir $FROM into $TO";
        echo "Using path: $0 "; echo;
        exit 1;
else
        echo "Recoding files: $TYPES from $1 path $FROM into $TO";
        echo;
        find $1 -name "$TYPES" -type f -print0|while read -d '' SOURCE; do
                echo "========== Recoding: $SOURCE From: $FROM into: $TO ==========";
                cat "$SOURCE"|iconv -f $FROM -t $TO > "$SOURCE$FILEPrefix";
                wait;
        done;
        exit 0;
fi
# end
```
---
---

## Bash prompt sets by the `PS1` variable.

```bash
nano ~/.bashrc
```
Add this at the end:

```bash
# --- Custom prompt: installed packages + free space on /dev/sda1 ---
__prompt_info() {
    # Package count: recount only when the dpkg database changes (keeps the prompt fast)
    local mtime
    mtime=$(stat -c %Y /var/lib/dpkg/status 2>/dev/null)
    if [[ "$mtime" != "$__PKG_MTIME" ]]; then
        __PKG_COUNT=$(dpkg-query -f '${db:Status-Abbrev}\n' -W 2>/dev/null | grep -c '^ii')
        __PKG_MTIME=$mtime
    fi

    # Free space on /dev/sda1 in MB
    __FREE_MB=$(df -BM --output=avail /dev/sda1 2>/dev/null | tail -n1 | tr -dc '0-9')
    # Free space on /dev/sda1 in GB
    __FREE_GB=$(df -B1 --output=avail /dev/sda1 2>/dev/null | awk 'NR==2 {printf "%.1f", $1/1024/1024/1024}')
}

PROMPT_COMMAND="__prompt_info${PROMPT_COMMAND:+; $PROMPT_COMMAND}"

PS1='\[\e[1;33m\][pkgs: ${__PKG_COUNT} | sda1: ${__FREE_MB} MB]\[\e[0m\] \[\e[1;32m\]\u@\h\[\e[0m\]:\[\e[1;34m\]\w\[\e[0m\]\$ '
```

### To Apply:

```bash
source ~/.bashrc
```
### How it works

- `dpkg-query ... | grep -c '^ii'` counts the packages that are actually installed. It skips packages that were removed but still have config files left (`rc`), which a plain `dpkg -l | wc -l` would include.
- The count only runs again when `/var/lib/dpkg/status` changes, so the prompt doesn't slow down. It updates on its own after `apt install` or `apt remove`.
- `df -BM --output=avail /dev/sda1` gives the free space in MB. This is the space your user can actually use, so it doesn't count the blocks reserved for root.
- `PS1` is in single quotes on purpose. That makes `${__PKG_COUNT}` and `${__FREE_MB}` update every time the prompt is drawn, not just once at login.
- The `\[...\]` parts are colours. If you want a plain prompt, use this instead: `PS1='[pkgs: ${__PKG_COUNT} | sda1: ${__FREE_MB} MB] \u@\h:\w\$ '`

### Notes

- Run `lsblk` or `df -h` first to check that `/dev/sda1` is the right partition. On many systems the root partition is somewhere else, like `/dev/nvme0n1p2` or `/dev/sda2`. `df` only reports a partition that is mounted. If you mean the root filesystem, use `df -BM --output=avail /` instead.
- This only changes the prompt for your user. For root, add the same block to `/root/.bashrc`. For every user on the system, add it to `/etc/bash.bashrc`.
