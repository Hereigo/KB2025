> Replacing Xfce with a bare X11 window manager can reduce RAM use and make the desktop feel snappier, but it usually gives only a small boot-time improvement—because most boot time is spent starting system services, not the desktop. Measure first with `systemd-analyze`; if boot is slow, optimize services and VM settings rather than replacing the DE. [man.archlinux](https://man.archlinux.org/man/systemd-analyze.1.en.txt)

## When a WM is worth it

Use a standalone WM if your VM is RAM-constrained or you want a deliberately minimal “app launcher” machine.

| Approach | Boot-time gain | Usability | Adjustment needed |
|---|---:|---|---|
| **Keep Xfce, trim it** | Small | High | Minimal |
| **LXQt** | Small to moderate | High | Low |
| **Openbox** | Small to moderate | Medium | Moderate |
| **i3 / Sway** | Small | Low initially | High |

A window manager such as Openbox handles windows but does not provide the panel, settings center, file manager integration, network applet, and session management that a DE supplies; you add those components individually if needed. [wiki.archlinux](https://wiki.archlinux.org/title/Openbox)

## Best practical choice

For your goal—fast VirtualBox boot and Flatpak apps with little setup—**keep Xfce, but disable its compositor and autostart extras**. Xfce’s own `xfwm4` compositor can be disabled, and it is a meaningful performance tweak for a VM; it may also help with visual glitches or tearing. [wiki.archlinux](https://wiki.archlinux.org/title/Xfwm)

Do this:

- In **Settings → Window Manager Tweaks → Compositor**, disable compositing.
- Remove unneeded startup apps: **Settings → Session and Startup → Application Autostart**.
- Disable desktop icons, wallpaper effects, animations, and unnecessary panel plugins.
- Keep Xfce’s file manager and panel; they cost little and preserve usability.
- Install only Flatpak, Flathub, and the apps you need.

If you want a lighter desktop but still want normal usability, choose **LXQt** rather than a bare WM. It is lighter than Xfce in many setups while retaining a conventional desktop workflow.

## If you do use a WM

Choose **Openbox**, not i3 or Hyprland, for the least adjustment.

- Install `openbox`, a lightweight terminal, file manager, panel/taskbar, and network manager applet.
- Select **Openbox** at the login screen; it can run standalone without a desktop environment or session manager. [openbox](https://openbox.org/help/Getting_started)
- Configure keyboard shortcuts, application menus, autostart, and themes through Openbox’s `rc.xml`, `menu.xml`, `autostart`, and `environment` files. [wiki.archlinux](https://wiki.archlinux.org/title/Openbox)
- Add `picom` only if you need transparency/shadows; Openbox has no native compositor. [wiki.archlinux](https://wiki.archlinux.org/title/Openbox)

Example minimal setup:

```bash
sudo apt install openbox pcmanfm lxterminal network-manager-gnome picom
```

Then log out and select **Openbox** in the display manager.

## What actually improves boot

Use these first—they usually matter more than switching from Xfce to Openbox:

```bash
systemd-analyze
systemd-analyze blame
systemd-analyze critical-chain
```

`systemd-analyze` reports kernel, initrd, and userspace timing, while `blame` identifies services that took longest to start. [man.archlinux](https://man.archlinux.org/man/systemd-analyze.1.en.txt)

- Disable services you do not use, but do **not** blindly disable networking, D-Bus, or display-manager-related units.
- Use a lightweight display manager such as **LightDM**.
- Avoid a full desktop autostarting Bluetooth, printer services, indexing, or update checkers if you do not need them.
- In VirtualBox, allocate enough RAM, use an SSD-backed virtual disk, and avoid an oversized virtual disk image.

**Bottom line:** stay with **Xfce + X11**, disable its compositor and unnecessary autostarts. Move to **Openbox** only if you are comfortable editing a few config files and do not mind adding basic desktop components yourself.

---

**Yes—but not with VirtualBox’s normal Shared Folders feature.** Shared Folders require Guest Additions, because they use a special guest filesystem driver. [docs.oracle](https://docs.oracle.com/en/virtualization/virtualbox/6.0/user/sharedfolders.html)

## Without Guest Additions, the easiest reliable method is **network transfer**: use SSH/SFTP, or switch the VM’s network mode to **Bridged Adapter** and share files over the local network.

## Easiest option: SFTP

Install and enable an SSH server in the Linux guest:

```bash
sudo apt update
sudo apt install openssh-server
sudo systemctl enable --now ssh
```

Then, from the host, connect with an SFTP-capable file manager or client:

- **Windows:** WinSCP, FileZilla, or Windows File Explorer: `sftp://user@127.0.0.1:2222`
- **macOS/Linux:** FileZilla, or a terminal command:
  ```bash
  sftp -P 2222 user@127.0.0.1
  ```

With the default **NAT** network mode, create a port-forwarding rule from host port `2222` to guest port `22` in **VM Settings → Network → Advanced → Port Forwarding**. VirtualBox explicitly supports forwarding host TCP traffic to the guest’s SSH port this way. [download.virtualbox](https://download.virtualbox.org/virtualbox/7.2.18/UserManual.pdf)

## Bridged adapter option

In **VM Settings → Network**, change **Attached to** from **NAT** to **Bridged Adapter**. The guest then behaves more like a device on your real LAN, so you can connect from the host directly to the guest’s IP address:

```bash
sftp user@GUEST_IP
```

Find the guest IP with:

```bash
ip addr
```

This avoids port forwarding, but it exposes the guest to your local network, so use a strong password or SSH keys.

## Other no-Guest-Additions methods

| Method | Best for | Notes |
|---|---|---|
| **SFTP/SSH** | Regular, secure transfers | Recommended; works with NAT plus port forwarding or Bridged networking.  [download.virtualbox](https://download.virtualbox.org/virtualbox/7.2.18/UserManual.pdf) |
| **HTTP file server** | One-off downloads from guest to host | Run `python3 -m http.server` in a guest folder, then open `http://GUEST_IP:8000` in the host browser. |
| **Cloud storage / web upload** | Small or occasional files | Simple, but slower and less private. |
| **USB drive passed through** | Larger files | Can work, but USB passthrough can be less convenient than networking. |
| **VirtualBox Guest Control File Manager** | GUI drag-and-drop-style transfer | Requires Guest Additions, so it does not meet your constraint.  [download.virtualbox](https://download.virtualbox.org/virtualbox/7.2.18/UserManual.pdf) |

## Recommendation

For your minimal Xfce/Openbox VM, use **OpenSSH + SFTP**:

1. Keep the VM on **NAT**.
2. Add host port `2222` → guest port `22`.
3. Use FileZilla/WinSCP from the host, or `sftp -P 2222 user@127.0.0.1`.
4. Optionally install `sshfs` on the host to mount the guest folder like a local directory.

It is secure, scriptable, and does not require Guest Additions. Shared Folders remain the most seamless option, but only if you decide to install Guest Additions later. [docs.oracle](https://docs.oracle.com/en/virtualization/virtualbox/6.0/user/sharedfolders.html)