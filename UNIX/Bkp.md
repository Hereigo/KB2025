```sh
tar -czf Backup.tar WORKDIR && \
gpg --pinentry-mode loopback --passphrase-file password.txt -c Backup.tar && \
rm Backup.tar && \
mv -f Backup.tar.gpg /run/media/_some_path_/Backup.tar.gpg && \
poweroff
```