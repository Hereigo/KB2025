# For 1080p TV:

- Raspberry Pi OS Lite 64-bit (current release).
- Wayland + labwc for a lightweight graphical session.
- Firefox ESR or the Firefox package available for your OS release.
- Automatic login and Firefox startup.
- Keyboard and mouse available whenever needed.
- No full desktop environment running in the background.

One important adjustment: don't use Cage for this setup. It's designed around a single application, whereas labwc gives you more flexibility for keyboard shortcuts, dialogs, and browser interaction.

The official Raspberry Pi kiosk guide uses the same general labwc autostart approach.

## 1. What you'll need

| Component | Recommendation |
| --- | --- |
| Raspberry Pi | Pi 4 Model B, 4 GB |
| OS | Raspberry Pi OS Lite 64-bit |
| Storage | 32 GB or larger, preferably high-endurance |
| Network | Gigabit Ethernet preferred |
| Display | 1080p HDMI |
| Power | Official 5.1V / 3A PSU |
| Browser | Firefox |

Ethernet is preferable for streaming, especially if your Wi-Fi signal near the TV is weak.

## 2. Install Raspberry Pi OS

Download [Raspberry Pi Imager](<https://www.raspberrypi.com/software/?utm_source=chatgpt.com>).

In Raspberry Pi Imager:

1. Select Raspberry Pi 4.
2. Choose Raspberry Pi OS Lite (64-bit).
3. Select your microSD card.
4. Configure:
   - Hostname: `pi-tv`
   - Username and password.
   - Wi-Fi credentials if needed.
   - Enable SSH.
5. Write the image.

Boot the Pi and connect over SSH or use a keyboard and monitor.

Update the system:

bash

```bash
sudo apt update
sudo apt full-upgrade -y
sudo reboot
```

## 3. Install the lightweight graphical environment

After reboot:

bash

```bash
sudo apt install --no-install-recommends -y \
  labwc \
  firefox-esr \
  dbus-user-session \
  fonts-liberation \
  wtype
```

Package availability can vary by Raspberry Pi OS release. If `firefox-esr` isn't available, check:

bash

```bash
apt-cache search firefox
```

Install the available native ARM64 Firefox package rather than an x86 package or an unofficial binary.

## 4. Configure automatic graphical login

For the Lite installation, install a lightweight login manager:

bash

```bash
sudo apt install --no-install-recommends -y lightdm
```

Check that the labwc session file exists:

bash

```bash
ls /usr/share/wayland-sessions/
```

You should see a `labwc.desktop` entry. If it isn't present, don't continue with the autologin configuration until the session is installed.

Create an autologin configuration:

bash

```bash
sudo mkdir -p /etc/lightdm/lightdm.conf.d
sudo nano /etc/lightdm/lightdm.conf.d/12-autologin.conf
```

Add the following, replacing `YOUR_USERNAME` with the account created in Imager:

ini

```ini
[Seat:*]
autologin-user=YOUR_USERNAME
autologin-session=labwc
user-session=labwc
```

Enable LightDM:

bash

```bash
sudo systemctl enable lightdm
```

## 5. Configure Firefox kiosk startup

Create the labwc configuration directory:

bash

```bash
mkdir -p ~/.config/labwc
nano ~/.config/labwc/autostart
```

Add:

bash

```bash
#!/bin/sh

# Prevent the TV display from sleeping
wlopm --off '*' 2>/dev/null || true

# Start Firefox
firefox --kiosk --new-instance \
  https://www.youtube.com \
  &
```

I recommend starting with YouTube as the home page. You can subsequently navigate to other movie websites using the mouse or keyboard.

For a more convenient experience, use Firefox's normal fullscreen mode rather than making the browser impossible to exit. Firefox kiosk mode hides the browser controls, but you can still use keyboard shortcuts.

For example:

- `Ctrl + L` — enter a website address.
- `Ctrl + T` — open another tab.
- `Ctrl + W` — close a tab.
- `F11` — toggle fullscreen.
- `Alt + F4` — close Firefox.

Note: `--kiosk` hides browser controls, so `Ctrl + L` may not be available in kiosk mode. If address-bar navigation is important, remove `--kiosk` and use `--start-fullscreen` instead, or use `F11` after startup.

## 6. Make Firefox convenient for TV use

For your particular use case, I'd actually start Firefox in normal browser mode and use fullscreen with F11.

Replace the autostart contents with:

bash

```bash
#!/bin/sh

# Start Firefox
firefox --new-window https://www.youtube.com &
```

This gives you:

- Full address-bar access.
- Normal browser tabs.
- Mouse navigation.
- Keyboard shortcuts.
- Fullscreen via F11.

You can save YouTube as your homepage and bookmark your favourite movie websites.

## 7. Disable screen blanking

You don't want the TV going black while watching a film.

Open:

bash

```bash
sudo raspi-config
```

Look for the display or screen blanking option and disable it if available.

Also check that your TV's own energy-saving settings aren't turning off the HDMI input.

## 8. Configure video playback

This is the most important part of the setup.

The Pi 4 has hardware decoding capabilities for:

- H.264 up to 1080p60.
- HEVC up to 4K60.

These are hardware capabilities, not guarantees that Firefox or every streaming website will use them.

pip-assets.raspberrypi.com

In Firefox:

1. Open `about:config`.
2. Leave hardware acceleration preferences at their defaults initially.
3. Open `about:support`.
4. Look at the Graphics section.
5. Check whether hardware acceleration is active.

Don't blindly force VA-API settings. Raspberry Pi's video acceleration support depends on the OS release, driver stack, Firefox build and codec.

For YouTube:

1. Play a 1080p video.
2. Right-click the video twice.
3. Select Stats for Nerds.
4. Watch the dropped frames counter.
5. Check the codec (`avc1` or `vp09`).

If 1080p60 VP9 playback struggles, try reducing playback quality to 1080p30 or 720p.

## 9. Reboot and test

bash

```bash
sudo reboot
```

Expected result:

1. Raspberry Pi powers on.
2. Linux boots automatically.
3. LightDM logs in to labwc.
4. Firefox opens YouTube.
5. Use keyboard and mouse to browse and watch movies.

## 10. Recommended performance settings

For your 4 GB Pi, I'd use these settings:

| Setting | Recommendation |
| --- | --- |
| Resolution | 1920 × 1080 at 60 Hz |
| Browser tabs | 2–4 |
| Swap | Default OS configuration |
| GPU memory | Default initially |
| Browser cache | Default |
| Wi-Fi | 5 GHz, if Ethernet unavailable |
| Overclocking | Not necessary |
| Cooling | Heatsink and fan recommended |

Don't disable swap or aggressively reduce GPU memory. Firefox can use a fair amount of memory with video streaming.

## 11\. Important streaming compatibility warning

YouTube should be your first test.

Other movie websites can be more complicated because of:

- Widevine DRM.
- VP9/AV1 video decoding.
- High-bitrate H.264 streams.
- Website-specific browser restrictions.

Netflix and some subscription streaming services may not work reliably on ARM Linux Firefox. Test the actual websites before treating the Pi as a finished media player.

## My final recommendation

One consideration: the Lite + LightDM + labwc combination above requires some manual integration. If LightDM doesn't offer labwc as a selectable session, use Raspberry Pi OS Desktop (64-bit) instead, configure desktop autologin, and launch Firefox from the existing labwc autostart. This is the simpler fallback and still works well with 4 GB RAM.

Useful reference: [Official Raspberry Pi kiosk tutorial](<https://www.raspberrypi.com/tutorials/how-to-use-a-raspberry-pi-in-kiosk-mode/?utm_source=chatgpt.com>).

One last suggestion: Before investing time in optimizing Firefox, test actual YouTube 1080p playback. The Pi 4's video decoding support is the biggest potential limitation, not the lightweight desktop environment.