#### Posh as Admin.

```powershell
wsl --install

wsl --update

wsl --list --online

wsl --install -d Debian

wsl --list --verbose

# RUN DEBIAN
wsl -d Debian

# REMOVE
wsl --unregister Debian
```

----

## Set Linux home directory

### Option 1: Change your Linux home directory (recommended)

 Check your current home:

```
echo $HOME
```

 It's usually:

```
/home/yourusername
```

 To change it permanently:

 1. Create the new directory:

```
sudo mkdir -p /data/home/myuser
sudo chown myuser:myuser /data/home/myuser
```

 2. Change your home directory in `/etc/passwd`:

```
sudo usermod -d /data/home/myuser myuser
```

 3. Move your existing files:

```
sudo mv /home/myuser/* /data/home/myuser/
sudo mv /home/myuser/.[!.]* /data/home/myuser/ 2>/dev/null
```

 4. Exit WSL and restart it:

```
wsl --shutdown
```

 Then reopen Debian.

---

 ### Option 2: Start WSL in a different directory

 If you just want WSL to open somewhere else:

 From PowerShell:

```
wsl -d Debian --cd /path/to/directory
```

 For example:

```
wsl -d Debian --cd /mnt/c/Projects
```

 Or set the Windows Terminal profile's `"startingDirectory"` if you launch WSL from Windows Terminal.

---

 ### Option 3: Make `/mnt/c/...` your home (not generally recommended)

 You can set your home to a Windows folder, for example:

```
sudo usermod -d /mnt/c/Users/YourName myuser
```

 However, this is **not recommended** because:

 - Linux file permissions don't map perfectly to NTFS.
- Many Linux tools expect a native Linux filesystem.
- Performance is generally better when your home is inside the WSL filesystem (`/home`, `/srv`, `/opt`, etc.).

 A common practice is to keep your Linux home in WSL and access Windows files under `/mnt/c` when needed.

---

 ### Option 4: Change the default WSL user

 If you're trying to change which user's home is used when Debian starts, you can set the default user from Windows:

```
debian config --default-user myuser
```

 Or, on newer WSL versions:

```
wsl -d Debian --user myuser
```

 or configure it in `/etc/wsl.conf`.

---
