The fastest option is not booting at all: **save the VM state** and resume it, which takes about 2–3 s. For a real cold boot, aim for **under 5 s to the app window** with a minimal distro, no display manager, autologin, and a kiosk Wayland compositor that launches the Flatpak app directly.

## 0. Skip booting: save the VM state
Set the VM up once with the app running, then close it with **"Save the machine state"**. You can do the same from the command line:
```bash
VBoxManage controlvm "MyVM" savestate
```
```bash
VBoxManage startvm "MyVM"
```
If you want the same clean state every time, take a snapshot while the VM is running and restore it before each start.

## 1. VirtualBox settings
- **System:** set Boot Order to Hard Disk only. Enable EFI if you'll use systemd-boot. Use 2–4 CPUs and Paravirtualization = KVM.
- **Display:** use VMSVGA with 128 MB video memory and turn 3D acceleration on. If apps glitch, try turning 3D off.
- **Storage:** use a SATA/AHCI or NVMe controller and tick **Solid-state Drive** and **Use Host I/O Cache**. Remove the empty optical drive.
- **Remove what you don't need:** floppy, audio, serial ports, and USB (or keep only USB 1.1).
- **Guest Additions:** recent kernels already include the `vmwgfx` driver, so you only need Guest Additions for the clipboard and shared folders.

## 2. Distro
Use **Debian 13 netinst** with no desktop selected (only "standard system utilities" unchecked too) or **Arch minimal**. Both are small and use systemd. Avoid full GNOME or KDE installs.

## 3. Bootloader and kernel
**GRUB** (`/etc/default/grub`):
```
GRUB_TIMEOUT=0
GRUB_TIMEOUT_STYLE=hidden
GRUB_CMDLINE_LINUX_DEFAULT="quiet loglevel=3 mitigations=off nowatchdog systemd.show_status=false"
```
```bash
sudo update-grub
```
Or replace GRUB with systemd-boot (EFI only), which is faster: `bootctl install`, then set `timeout 0` in its loader config.

Make the initramfs smaller with `MODULES=dep` in `/etc/initramfs-tools/initramfs.conf`, then:
```bash
sudo update-initramfs -u
```

## 4. Turn off slow services
```bash
sudo systemctl disable --now NetworkManager-wait-online.service systemd-networkd-wait-online.service apt-daily.timer apt-daily-upgrade.timer man-db.timer e2scrub_all.timer fstrim.timer ModemManager cups bluetooth avahi-daemon 2>/dev/null
```
```bash
sudo systemctl mask systemd-journal-flush.service
```
Find what's still slow:
```bash
systemd-analyze blame
```
```bash
systemd-analyze critical-chain
```

## 5. No display manager: autologin straight into the app
```bash
sudo apt install --no-install-recommends cage flatpak xdg-desktop-portal xdg-desktop-portal-gtk dbus-user-session fonts-dejavu-core
```
```bash
flatpak remote-add --if-not-exists flathub https://dl.flathub.org/repo/flathub.flatpakrepo
```
Autologin on tty1 (replace `youruser` with your username):
```bash
sudo mkdir -p /etc/systemd/system/getty@tty1.service.d
sudo tee /etc/systemd/system/getty@tty1.service.d/autologin.conf >/dev/null <<'EOF'
[Service]
ExecStart=
ExecStart=-/sbin/agetty --autologin youruser --noclear %I $TERM
EOF
```
Start the app from `~/.bash_profile`:
```bash
if [ -z "$WAYLAND_DISPLAY" ] && [ "$(tty)" = /dev/tty1 ]; then
  exec cage -- flatpak run org.example.App
fi
```
If you need several apps or windows, use `labwc` or `sway` instead of `cage`.

## 6. Flatpak details
- **Install a portal backend** (`xdg-desktop-portal-gtk` or `-wlr`). Without one, many apps hang for about **25 seconds** at startup waiting on D-Bus. This is the most common cause of a slow first launch.
- **Run each app once** after installing it, so the font and shader caches are built before you save the state or take a snapshot.
- To avoid repeating the first-launch setup, set `XDG_CURRENT_DESKTOP=sway` (or similar) so portal lookups resolve.

## Expected times
| Setup | Time to app window |
|---|---|
| Stock Debian with GNOME | 20–40 s |
| Tuned as above, cold boot | about 3–6 s |
| Resume from saved state | about 2 s |

----
----

**Arch Linux, minimal install.** It's the fastest to boot and the easiest to keep small.

**Why Arch:**
- **Nothing extra to remove.** A minimal install has no desktop and no background services you didn't add.
- **Fast boot chain.** You can use systemd-boot and keep the initramfs small easily (`mkinitcpio` with the `systemd` hook and only the drivers this VM needs). A cold boot to `cage` takes about 2–4 s.
- **Current packages.** You get a recent Mesa, the `vmwgfx` driver, and current Wayland, portals, and `cage`, which helps Flatpak GUI apps in VirtualBox.
- **Good Flatpak support.** Portals work properly and the Arch Wiki covers every setup step.
- **Quick to install.** `archinstall` sets it up in about 5 minutes. Choose the "minimal" profile, systemd-boot, and ext4.

**Second choice: Debian 13 netinst** with nothing selected at the "Software selection" step. Pick it if you'd rather not update often and want things to stay stable. It's about 1–2 s slower out of the box, mostly because of GRUB and a larger initramfs, and the tuning from my previous answer closes most of that gap.

**Avoid these:**
- **Ubuntu:** snapd and cloud-init make boot slower.
- **Fedora Silverblue:** it's built for Flatpak, but it boots slower and is heavier.
- **Alpine:** it boots very fast, but Flatpak and portals are awkward without systemd and you'll spend time fixing problems.

----
----

