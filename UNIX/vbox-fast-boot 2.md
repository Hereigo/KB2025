If your priority is very fast Linux boot, low RAM usage, and minimal background services, I'd recommend a lightweight distribution with systemd services trimmed down and a lightweight desktop environment.

Here are my top choices:

## Best lightweight Linux desktop distros

File:Void Linux 20210930 Xfce Thunar 4.15.10 Adwaita dark theme - English.png - Wikimedia Commons

1. Void Linux + Xfce

My top pick- Uses runit instead of systemd.
- Minimal background services.
- Fast, lightweight package manager.
- glibc or musl variants.
- Good control over startup services.

Best balance of speed and a usable desktop.

Tux Machines — Systemd-Free antiX 26 Linux Distro Is Here Based on Debian 13 “Trixie”

2. antiX Linux

- No systemd.
- Very low RAM consumption.
- Uses lightweight window managers: IceWM, Fluxbox, JWM.
- Debian-based.
- Great for older hardware.

Best for minimal resource consumption.

Alpine Linux 3.20 arrives with support for 64-bit RISC-V

3. Alpine Linux + Xfce

- OpenRC init system.
- Very small base installation.
- Minimal services by default.
- Low memory footprint.

Best for building an extremely minimal system, but desktop setup takes more work.

The beginner’s handbook

4. Debian minimal + Xfce or Openbox

- Install only essential packages.
- Disable unnecessary services.
- Huge software ecosystem.
- Very stable.

Best for a lightweight but conventional desktop.

GitHub - sumithemmadi/ArchLinux-Installation-Guide: This is a guide for arch linux installation with Openbox Window Managers

5. Arch Linux + Openbox

- Install only what you need.
- Full control over services.
- Can be extremely lean.
- systemd by default, but configurable.

Best for customization.

iTechGuides

+2

## My recommendation

I'd choose Void Linux + Xfce for a fast everyday desktop, or antiX if minimum services and RAM usage are the absolute priorities.

---
---

# My recommendation: Void Linux + runit + Openbox

Best match for your prioritiesWith 8 GB RAM and an SSD, I'd build a minimal Void Linux x86\_64 glibc installation, using runit and a lightweight X11 window manager.

The goal is to minimize startup work, not merely minimize RAM consumption.

Voidcraft | Archcraft

Перешёл я на Void Linux — Скриншоты — Галерея

GitHub - addy-dclxvi/void-bspwm-dotfiles: My personal backup of my dotfiles (Void Linux). · GitHub

## 1. Recommended configuration

| Component | Choice |
| --- | --- |
| Distribution | Void Linux |
| Architecture | x86\_64 glibc |
| Init | runit |
| Desktop | Openbox |
| Display server | Xorg |
| Login manager | None initially; startx |
| Bootloader | GRUB |
| Filesystem | ext4 |
| Swap | 2–4 GB swapfile |
| Audio | PipeWire, if needed |
| Network | NetworkManager or dhcpcd |

Why this combination:

- runit is simple and has very little service-management overhead.
- Openbox avoids the overhead of a full desktop environment.
- Xorg plus `startx` avoids a graphical login manager.
- Void lets you explicitly choose which services start at boot.

Void's handbook documents its runit service management and how to enable or disable individual services.

Void Linux Handbook

+1

## 2. Minimal package selection

Start with these components:

bash

```bash
sudo xbps-install -S
sudo xbps-install -y \
  xorg-minimal \
  xinit \
  openbox \
  tint2 \
  rofi \
  pcmanfm \
  lxappearance \
  feh \
  firefox
```

Useful optional additions:

- `lightdm` — graphical login screen, if desired.
- `pipewire` — modern audio.
- `NetworkManager` — network management.
- `picom` — optional window compositor.
- `zramen` — optional compressed RAM swap.

Start the desktop with:

bash

```bash
echo "exec openbox-session" > ~/.xinitrc
startx
```

## 3. Keep startup services minimal

For runit, services are enabled by linking them into `/var/service`.

Example:

bash

```bash
sudo ln -s /etc/sv/dbus /var/service/
sudo ln -s /etc/sv/NetworkManager /var/service/
```

Only enable services you actually need.

| Service | Recommendation |
| --- | --- |
| dbus | Enable if required |
| NetworkManager | Enable for Wi-Fi |
| chronyd | Optional |
| sshd | Disable unless needed |
| cups | Disable unless printing |
| Bluetooth | Disable unless needed |
| Avahi | Disable unless needed |

Don't disable hardware, graphics, or networking services just to reduce the service count. Void's service documentation explains the runit management commands.

Void Linux Handbook

## 4. Optimize boot time

I'd prioritize these changes:

1. Disable unnecessary UEFI/BIOS hardware initialization.
2. Reduce GRUB timeout to 1 second.
3. Avoid unnecessary initramfs modules.
4. Avoid unnecessary filesystem checks and mount delays.
5. Start only essential services.
6. Avoid graphical display managers if you don't need them.
7. Use the SSD's native TRIM support.

For periodic SSD maintenance:

bash

```bash
sudo xbps-install -S fstrim
```

Check boot performance using:

bash

```bash
systemd-analyze
```

That command won't work on Void because it doesn't use systemd. Instead, use kernel boot logs and timestamps to investigate startup delays.

## 5. Alternative: antiX 26.1

Tux Machines — Systemd-Free antiX 26 Linux Distro Is Here Based on Debian 13 “Trixie”

antiX 26.1

A particularly interesting alternative if you want less installation and configuration work.

- Systemd-free.
- runit by default.
- IceWM desktop.
- Multiple init choices.
- Debian-based.

[Official antiX website](<https://antixlinux.com/?utm_source=chatgpt.com>)

robin-antix.codeberg.page

+1

## Final verdict

| Priority | My choice |
| --- | --- |
| Fast, customizable boot | Void + runit |
| Minimal RAM usage | Void + Openbox |
| Minimal background services | Void + runit |
| Easiest systemd-free desktop | antiX |
| Maximum desktop minimalism | Void + Openbox + startx |

I'd personally install Void Linux with Openbox. You have enough RAM that there's no need to sacrifice desktop usability just to save a few hundred megabytes.

One important caveat: actual boot time depends heavily on firmware initialization, hardware, kernel, drivers and storage—not just the init system.

---
---

# Void Linux: optimizing for a 5-second desktop boot

Your target is particularly aggressive: power button → usable graphical desktop in under 5 seconds, including login.

My recommendation is to use Void Linux, runit, Openbox, and automatic graphical login through a minimal X startup script.

One important reality check: under 5 seconds is possible on some hardware, but cannot be guaranteed. UEFI firmware initialization alone can consume several seconds. The configuration below minimizes Linux-side startup time.

## 1. Final architecture

## Minimal Void desktop

UEFI firmware

Fast boot enabled

GRUB / EFI boot

Minimal bootloader delay

Linux kernel + initramfs

Lean hardware-specific initramfs

runit

Only essential services

Xorg + Openbox

Automatic login, no display manager

Usable desktop

Target: under 5 seconds

I'd use:

- Void Linux x86\_64 glibc
- runit
- Linux kernel, initially the standard Void kernel
- ext4
- Openbox
- Xorg
- `startx` from a dedicated autologin console
- No LightDM, SDDM, GDM, KDE or GNOME

## 2. Installation packages

Install a minimal base system, then:

bash

```bash
sudo xbps-install -S
sudo xbps-install -y \
  xorg-minimal \
  xinit \
  openbox \
  tint2 \
  rofi \
  feh \
  dbus \
  elogind
```

For networking, install just one manager appropriate to your hardware:

bash

```bash
sudo xbps-install -y NetworkManager
```

For Wi-Fi, you may also need firmware appropriate to your wireless adapter.

For audio, add PipeWire later rather than putting it on the critical boot path.

## 3. Remove unnecessary startup services

First inspect what's enabled:

bash

```bash
sudo sv status /var/service/*
```

For a minimal desktop, keep only services that are actually needed.

| Service | Recommendation |
| --- | --- |
| dbus | Keep |
| NetworkManager | Keep if using managed networking |
| elogind | Keep if using desktop power/session management |
| dhcpcd | Don't run alongside NetworkManager |
| Bluetooth | Disable if unused |
| cups | Disable if no printer |
| avahi | Disable if not needed |
| sshd | Disable if not needed |
| cron | Optional |

Disable a service with:

bash

```bash
sudo rm /var/service/bluetoothd
```

Replace `bluetoothd` with the actual service directory name.

Important: Don't remove runit core services or essential device-management services. Also, don't disable all getty services indiscriminately; keep a recovery console.

Void's runit service documentation: [Void service management handbook](<https://docs.voidlinux.org/config/services/index.html?utm_source=chatgpt.com>).

## 4. Eliminate the graphical login manager

This is one of the most useful optimizations.

Instead of LightDM or another display manager, configure automatic login on a dedicated local console and start X directly.

For example, your user's `~/.bash_profile` can contain:

bash

```bash
if [ -z "$DISPLAY" ] && \
   [ "$(tty)" = "/dev/tty1" ]; then
    exec startx
fi
```

And `~/.xinitrc`:

bash

```bash
#!/bin/sh

exec openbox-session
```

Make it executable:

bash

```bash
chmod +x ~/.xinitrc
```

You still need to configure console autologin through the appropriate Void agetty service configuration. This makes the graphical session start without a password prompt.

Security warning: Automatic login gives anyone with physical access to the machine access to that user account. Use it only if that trade-off is acceptable, and retain a recovery console.

## 5. Optimize the initramfs

Void uses dracut by default.

For a simple installation with:

- One SSD
- ext4 root filesystem
- No LUKS encryption
- No LVM
- No RAID

You can use a host-specific initramfs to avoid unnecessary hardware and storage discovery.

Start by inspecting your existing configuration:

bash

```bash
ls -lh /boot/initramfs*
lsblk -f
```

Create:

`/etc/dracut.conf.d/10-fastboot.conf`

ini

```ini
hostonly="yes"
```

This can reduce initramfs size and unnecessary discovery.

Do not disable LUKS, LVM, RAID or device-mapper support if your installation depends on them. Void documents dracut configuration and alternative initramfs generators here: [Void kernel handbook](<https://docs.voidlinux.org/config/kernel.html?utm_source=chatgpt.com>).

## 6. Bootloader optimization

If using GRUB, edit:

`/etc/default/grub`

Set:

ini

```ini
GRUB_TIMEOUT=1
GRUB_TIMEOUT_STYLE=hidden
```

Then regenerate the configuration:

bash

```bash
sudo update-grub
```

If you want to experiment further, consider direct UEFI kernel boot or systemd-boot (the bootloader, not the systemd init system).

UEFI firmware settings:

- Enable Fast Boot if hardware initialization remains reliable.
- Disable unused boot devices.
- Put your SSD first in the boot order.
- Avoid unnecessary network/PXE boot.
- Disable unused onboard hardware where appropriate.

## 7. Kernel and filesystem tuning

For the first installation, keep the standard Void kernel. Don't jump immediately to custom kernels.

Recommended filesystem mount options for ext4:

fstab

```fstab
UUID=YOUR_UUID / ext4 defaults,noatime 0 1
```

`noatime` avoids unnecessary access-time updates.

For SSD maintenance, use periodic TRIM rather than continuous discard:

bash

```bash
sudo fstrim -av
```

Run it periodically, not on every boot.

Avoid:

- Aggressive kernel module blacklisting.
- Disabling filesystem checks.
- Disabling security mitigations blindly.
- Removing hardware drivers to save milliseconds.

## 8. How to measure boot time

There are three different measurements:

| Measurement | What it includes |
| --- | --- |
| Firmware time | Power-on to bootloader |
| Linux boot time | Kernel through init and services |
| Desktop-ready time | Power-on until Openbox is usable |

Check kernel startup timestamps:

bash

```bash
dmesg | less
```

Check system uptime immediately after the desktop appears:

bash

```bash
cat /proc/uptime
```

For accurate testing, use a stopwatch or video recording from power-on until the desktop responds to keyboard input.

Test several cold boots, not just warm reboots.

## 9. Realistic performance expectations

These are engineering targets, not measured benchmarks:

| Boot stage | Optimization goal |
| --- | --- |
| UEFI | 1–3 seconds |
| Bootloader | 0–0.5 seconds |
| Kernel and initramfs | 1–2 seconds |
| runit and essential services | 0.3–1 second |
| Xorg and Openbox | 0.5–2 seconds |

These stages can overlap or vary, so don't interpret the numbers as guaranteed additive timings.

My honest assessment: Under five seconds from power-on to a usable desktop is an extreme target. Even with this configuration, firmware and graphics initialization may make it impossible on some machines.

---
---

That changes the recommendation significantly. Inside VirtualBox, I'd optimize Void Linux for fast virtual hardware initialization, minimal services, and quick graphical startup rather than physical hardware.

I'd keep Void Linux, but simplify the configuration further.

# Recommended VirtualBox configuration

🖥️ Share Your Desktop \[2026\] - Page 5 - Linux lounge - EndeavourOS

[forum.endeavouros.com](<https://forum.endeavouros.com/t/share-your-desktop-2026/77330?page=5&utm_source=chatgpt.com>)

## 1. Virtual machine settings

For your 8 GB RAM host:

| VirtualBox setting | Recommendation |
| --- | --- |
| RAM | 2048 MB |
| CPU | 2 vCPUs |
| Chipset | PIIX3 |
| Firmware | BIOS initially |
| Graphics controller | VMSVGA |
| Video memory | 128 MB |
| 3D acceleration | Test enabled and disabled |
| Disk | 12–20 GB dynamically allocated VDI |
| Network | NAT |
| Audio | Disabled if unnecessary |
| USB | Disabled if unnecessary |

Use 2 vCPUs initially. More virtual CPUs do not automatically mean faster boot.

## 2. Guest operating system

Void Linux x86\_64 glibc

Recommended

Os 12 Melhores Temas para seu Tint2

[terminalroot.com.br](<https://terminalroot.com.br/2021/12/os-12-melhores-temas-para-seu-tint2.html?utm_source=chatgpt.com>)

- runit
- Openbox
- Xorg
- startx
- ext4
- No display manager
- No systemd

For VirtualBox, I'd avoid a heavy desktop such as KDE or GNOME.

## 3. Minimize guest services

Keep:

- runit core services
- dbus
- elogind if needed
- NetworkManager only if needed
- VirtualBox guest integration if needed

Disable unused:

- Bluetooth
- CUPS
- Avahi
- SSH
- Printing services

## 4. VirtualBox Guest Additions

Install Guest Additions if you need:

- Dynamic screen resolution
- Mouse integration
- Shared clipboard
- Shared folders

These features can introduce additional services, so don't install them if you don't need them.

## 5. Boot optimization

For a virtual machine, I'd prioritize:

1. No graphical display manager.
2. Automatic console login.
3. Start Xorg directly.
4. Openbox instead of a full desktop environment.
5. Minimal initramfs.
6. No unnecessary network or hardware services.
7. Short GRUB timeout.

Avoid trying to optimize physical hardware drivers inside the guest. VirtualBox presents virtual hardware, so the guest should use the appropriate virtual graphics and storage drivers.

## 6. Boot-time target

For VirtualBox, I'd use these as experimental targets, not guarantees:

| Stage | Target |
| --- | --- |
| GRUB | 0–0.5 sec |
| Kernel and initramfs | 1–2 sec |
| runit | 0.3–0.7 sec |
| Xorg + Openbox | 0.5–1.5 sec |

A sub-5-second guest startup is a reasonable optimization goal, but the host's CPU scheduling, VirtualBox overhead and firmware affect the result.

---
---

# Optimized Void Linux VirtualBox VM — Windows 11 host

For your setup, I'd use Void Linux + runit + Openbox + Xorg + VirtualBox Guest Additions.

The important adjustment is that clipboard and shared-folder integration need Guest Additions, so we should retain their essential components rather than stripping the guest down indiscriminately.

Recommended configuration 2 GB RAM · 2 vCPUs · VMSVGA · Openbox · runit

## 1. Exact VirtualBox 7.x settings

Shut down the VM completely before changing settings.

## System → Motherboard

Base memory

2048 MB

Boot order

Hard Disk, Optical

Chipset

PIIX3

Pointing device

USB Tablet

EFI

Disabled initially

## System → Processor

Processors

2 CPUs

Execution cap

100%

PAE/NX

Enabled

## Display → Screen

Graphics controller

VMSVGA

Video memory

128 MB

3D acceleration

Initially disabled

## Storage

Controller

AHCI SATA

Disk

20 GB VDI

Allocation

Dynamically allocated

## Network

Adapter 1

Intel PRO/1000 MT Desktop

Attached to

NAT

## Audio / USB

- Audio: Disabled if unused.
- USB: Disabled if unused.
- Enable USB only if you need USB passthrough.

Why 2 GB RAM? Openbox doesn't need much memory. This leaves 6 GB for Windows 11. If the host has plenty of free RAM, 3 GB is also reasonable.

## 2. VirtualBox integration settings

Under Settings → General → Advanced:

| Setting | Value |
| --- | --- |
| Shared Clipboard | Bidirectional |
| Drag and Drop | Disabled |

Under Devices → Shared Folders, add a permanent folder:

- Folder name: `HostShare`
- Folder path: `C:\VMShare`
- Auto-mount: Enabled
- Make Permanent: Enabled
- Read-only: Disabled, if guest write access is wanted

Create `C:\VMShare` in Windows first.

Shared folders and bidirectional clipboard require Guest Additions.

Oracle Documentation

+1

## 3. Install Void Linux

Use the official x86\_64 glibc installation image:

[Download Void Linux](<https://voidlinux.org/download/?utm_source=chatgpt.com>)

During installation, choose:

- Filesystem: ext4
- Partitioning: automatic
- Bootloader: GRUB
- Swap: 2 GB
- Network: DHCP
- Init: runit (Void default)

For a simple VirtualBox installation, avoid encryption, LVM and RAID. They add complexity and can increase startup time.

## 4. Install the minimal desktop

After booting into Void:

bash

```bash
sudo xbps-install -Syu

sudo xbps-install -y \
  xorg-minimal \
  xinit \
  openbox \
  tint2 \
  rofi \
  dbus \
  elogind \
  bash
```

Install NetworkManager if you need networking:

bash

```bash
sudo xbps-install -y NetworkManager
sudo ln -s /etc/sv/NetworkManager /var/service/
```

Enable D-Bus:

bash

```bash
sudo ln -s /etc/sv/dbus /var/service/
```

Check whether a service is already enabled before creating its link.

## 5. Configure automatic graphical startup

Create `~/.xinitrc`:

bash

```bash
#!/bin/sh

exec openbox-session
```

Then:

bash

```bash
chmod +x ~/.xinitrc
```

Configure automatic console login on `tty1` using Void's agetty service, and add this to the user's `~/.bash_profile`:

bash

```bash
if [ -z "$DISPLAY" ] && \
   [ "$(tty)" = "/dev/tty1" ]; then
    exec startx
fi
```

This eliminates a separate display manager.

For a VM dedicated to this purpose, automatic login is convenient. Remember that it removes the login password barrier.

## 6. Install VirtualBox Guest Additions

Required for your integration featuresI recommend using the Guest Additions ISO matching your installed VirtualBox 7.x version.

First install build dependencies:

bash

```bash
sudo xbps-install -S
sudo xbps-install -y \
  base-devel \
  perl \
  dkms \
  linux-headers
```

Check that the headers match your running kernel. If the generic `linux-headers` package is unavailable or doesn't match, install the headers package corresponding to your installed Void kernel.

Then:

1. Start the VM.
2. In VirtualBox, select Devices → Insert Guest Additions CD Image.
3. Mount the CD if it doesn't mount automatically.
4. Run the installer as root.

For example:

bash

```bash
sudo sh /run/media/$USER/VBox_GAs_*/VBoxLinuxAdditions.run
```

The mount location may differ. Check `/media` or `/run/media` if necessary.

Oracle documents the Linux Guest Additions installation and kernel module setup here: [VirtualBox 7 Guest Additions documentation](<https://docs.oracle.com/en/virtualization/virtualbox/7.0/user/guestadditions.html?utm_source=chatgpt.com>).

Oracle Documentation

+1

After installation, reboot and verify:

bash

```bash
lsmod | grep vbox
```

You should see the relevant VirtualBox modules.

## 7. Enable clipboard and shared folders

### Clipboard

Create the Openbox autostart directory:

bash

```bash
mkdir -p ~/.config/openbox
nano ~/.config/openbox/autostart
```

Add:

bash

```bash
VBoxClient --clipboard &
```

This starts clipboard integration with the graphical session, rather than running unnecessary integration processes before the desktop starts.

Select Devices → Shared Clipboard → Bidirectional in VirtualBox.

### Shared folders

Add your user to the VirtualBox shared-folder group:

bash

```bash
sudo usermod -aG vboxsf "$USER"
```

Log out and back in, or reboot.

Your Windows folder should appear at:

text

```text
/media/sf_HostShare
```

If automatic mounting doesn't work, test manually:

bash

```bash
sudo mkdir -p /mnt/HostShare

sudo mount -t vboxsf \
  HostShare /mnt/HostShare
```

VirtualBox's shared-folder documentation: [Shared folders and mounting](<https://docs.oracle.com/en/virtualization/virtualbox/7.0/user/guestadditions.html?utm_source=chatgpt.com>).

Oracle Documentation

## 8. Final boot optimizations

Set GRUB to skip the waiting period:

Edit `/etc/default/grub`:

ini

```ini
GRUB_TIMEOUT=0
GRUB_TIMEOUT_STYLE=hidden
```

Regenerate:

bash

```bash
sudo update-grub
```

Keep the standard Void kernel and host-specific initramfs. Avoid unnecessary kernel command-line tweaks until you have a working baseline.

Keep only essential services enabled:

bash

```bash
sudo sv status /var/service/*
```

Void's runit service management is particularly useful here because you can explicitly control which services run.

The Void Linux Handbook

+1

## 9. Final configuration summary

# Your target VM

# 2 GB

Guest RAM

# 2

Virtual CPUs

# Openbox

Desktop

# runit

Init system

# VMSVGA

Graphics

# ext4

Filesystem

Enabled features

- Enabled Bidirectional clipboard
- Enabled Shared folders
- Enabled Automatic graphical login
- Enabled Minimal service set
- Disabled Display manager
- Disabled Unnecessary daemons

Important: Under five seconds from clicking Start on Windows 11 to a responsive desktop is an ambitious target. VirtualBox process startup, Windows scheduling, GRUB, guest kernel startup and Xorg all contribute. Measure cold boots before and after changes; don't assume a smaller service count automatically means a faster boot.

## 10. One final optimization opportunity

I would like to know your Windows host CPU, because VirtualBox CPU scheduling can make a substantial difference.

You can check it in Windows PowerShell:

powershell

```powershell
Get-CimInstance Win32_Processor |
  Select-Object Name, NumberOfCores, NumberOfLogicalProcessors
```

Paste the output and I can recommend the best vCPU count, execution cap and VirtualBox processor configuration for your host.

