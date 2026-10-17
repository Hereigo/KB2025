```sh
#!/usr/bin/env bash

# It helps the script stop on errors, including a failed verification pipeline.
set -euo pipefail

SOURCE="backupDatabaseDir"
PASSFILE="$SOURCE/password.txt"
ARCHIVE="backupDatabaseDir.tar"
ENCRYPTED="$ARCHIVE.gpg"

# Verify required paths
[[ -d "$SOURCE" ]] || { echo "Source directory not found"; exit 1; }
[[ -f "$PASSFILE" ]] || { echo "Passphrase file not found"; exit 1; }

# Archive the directory, excluding the passphrase file
tar --exclude="$PASSFILE" -cvf "$ARCHIVE" "$SOURCE"
#   -cf => .tar | -czf => .tar.gz (compressed)

# gpg --yes - permits silently to overwrite an existing encrypted output file.
gpg --batch --yes --pinentry-mode loopback \
    --passphrase-file "$PASSFILE" \
    --symmetric --cipher-algo AES256 \
    --output "$ENCRYPTED" "$ARCHIVE"

# Verify that the encrypted file can be decrypted
gpg --batch --yes --pinentry-mode loopback \
    --passphrase-file "$PASSFILE" \
    --decrypt "$ENCRYPTED" | tar -tf - >/dev/null

echo "Verification successful"

# Delete originals only after successful verification
rm -rf -- "$SOURCE"
rm -r -- "$ARCHIVE"

echo "Backup created: $ENCRYPTED"

poweroff
```