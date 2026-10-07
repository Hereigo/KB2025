You can run GUI apps on Linux without a full desktop environment—you just need a display server (X11 or Wayland) and, in most cases, a lightweight window manager. Below are the main approaches, from lightest to most practical.

## 1. X server + `.xinitrc` (no desktop, optional WM)

This is the classic “startx” method. You install Xorg, tell it what to launch via `~/.xinitrc`, and start X from the console.

- Install Xorg (examples):  
  - Debian/Ubuntu/Mint: `sudo apt install xorg`  
  - Fedora/RHEL: `sudo dnf install @base-x`  
  - Arch/Manjaro: `sudo pacman -S xorg` [linuxconfig](https://linuxconfig.org/how-to-run-x-applications-without-a-desktop-or-a-wm)
- Create/edit `~/.xinitrc` and put your app as the last `exec` line, e.g.:  
  ```bash
  # ~/.xinitrc
  exec firefox
  ```
  You can start a tiny window manager first if you need window controls:  
  ```bash
  exec openbox &
  exec firefox
  ```
  (Only the final `exec` should replace the shell; earlier ones run in the background.) [linuxconfig](https://linuxconfig.org/how-to-run-x-applications-without-a-desktop-or-a-wm)
- From a text console (TTY), run:  
  ```bash
  startx
  ```
  X starts, reads `.xinitrc`, and launches your GUI app without any desktop environment. [linuxconfig](https://linuxconfig.org/how-to-run-x-applications-without-a-desktop-or-a-wm)

Use this when you boot to console and want one or a few apps in a local graphical session.

## 2. Minimal window manager instead of a desktop

A full desktop (GNOME, KDE, etc.) is not required; a tiny window manager is often enough and makes multi-window use sane.

Common ultra‑light options:

- `openbox`, `fluxbox`, `icewm` – stacking WMs, very light  
- `dwm`, `i3`, `spectrwm` – tiling WMs, keyboard-driven  
- `twm` – often bundled with X, extremely minimal [unix.stackexchange](https://unix.stackexchange.com/questions/23059/do-i-need-a-desktop-to-run-a-gui)

Typical pattern:

```bash
sudo apt install openbox xorg   # or your distro’s equivalent
```

Then in `~/.xinitrc`:

```bash
exec openbox
```

and start with `startx`. You’ll get windows, menus, and keybindings, but no panels, notifications, or other DE baggage. [askubuntu](https://askubuntu.com/questions/59638/do-i-need-a-desktop-to-run-a-gui)

## 3. Remote GUI over SSH (X11 forwarding) – no local X needed on server

If you’re accessing the machine remotely, you often don’t need any X server or WM on the Linux box at all—just the app and its libraries.

- On the server: install the GUI app and dependencies (no Xorg needed if you never log in locally). [unix.stackexchange](https://unix.stackexchange.com/questions/23059/do-i-need-a-desktop-to-run-a-gui)
- On your local machine: ensure you have an X server (Linux/macOS usually do; on Windows use WSLg, Xming, VcXsrv, etc.).
- Connect with X11 forwarding:
  ```bash
  ssh -X user@host
  # or
  ssh -Y user@host   # trusted forwarding
  ```
- Then run:
  ```bash
  firefox &
  ```
  The window appears on your local desktop; the server runs only the app logic. [askubuntu](https://askubuntu.com/questions/59638/do-i-need-a-desktop-to-run-a-gui)

This is ideal for headless servers and lightweight VMs.

## 4. VNC / RDP “virtual display” without a desktop

For remote graphical access where you want a persistent session (not just single apps), use a virtual X server plus VNC/RDP:

- Install something like `xvfb` or `xvnc` to create a virtual display, plus a tiny WM (e.g., `openbox`, `matchbox`). [unix.stackexchange](https://unix.stackexchange.com/questions/23059/do-i-need-a-desktop-to-run-a-gui)
- Start a VNC server bound to that display, then connect with a VNC client.
- You can script this so that, for example, `openbox` + your app runs on `:1`, and you VNC into `:1`. [forums.raspberrypi](https://forums.raspberrypi.com/viewtopic.php?t=171140)

This gives you a “desktop-like” session without installing GNOME/KDE/etc.

## 5. Kiosk / single‑app modes

If you want exactly one GUI app full‑screen (e.g., a browser or media player), you can:

- Use X + a minimal WM configured to launch that app fullscreen on startup (via `.xinitrc` or WM config). [reddit](https://www.reddit.com/r/linux4noobs/comments/1exyh5x/run_gui_apps_without_de/)
- Use Wayland compositors with kiosk modes (e.g., Weston in kiosk mode) to run a single app without a traditional WM/DE. [reddit](https://www.reddit.com/r/linuxquestions/comments/1exyi9c/run_gui_apps_without_de/)

These setups are common for signage, appliances, and locked‑down systems.

## 6. Direct X tricks (advanced / hacky)

You can manually start X and point apps to it:

```bash
X &                 # starts X on :0 in the background
DISPLAY=:0 firefox &
```

This bypasses `startx` and `.xinitrc`, but you’ll likely still want at least a minimal WM for usable window behavior. [superuser](https://superuser.com/questions/550020/how-to-start-a-linux-gui-program-from-the-command-line-without-starting-a-deskto)

***

**Practical recommendations**

- Local console, one or few apps: Xorg + `.xinitrc` + tiny WM (e.g., `openbox`). [linuxconfig](https://linuxconfig.org/how-to-run-x-applications-without-a-desktop-or-a-wm)
- Headless server, remote use: SSH with X11 forwarding; no DE or even X server needed on the server. [askubuntu](https://askubuntu.com/questions/59638/do-i-need-a-desktop-to-run-a-gui)
- Persistent remote session: Xvfb/Xvnc + tiny WM + VNC/RDP. [unix.stackexchange](https://unix.stackexchange.com/questions/23059/do-i-need-a-desktop-to-run-a-gui)

If you tell me your distro and whether this is for local use, a VM, or a remote server, I can give exact commands and a ready‑to‑paste config.