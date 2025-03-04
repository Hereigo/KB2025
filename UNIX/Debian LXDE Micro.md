```sh

# 1. install Debian netinstall (do not choose a DE)

# 2. login as root :
edit /etc/apt/sources.list with nano (contrib non-free)
apt update && apt dist-upgrade
apt install sudo
usermod -a -G sudo <username>

# 3. 
apt install network-manager xorg lxde-core lightdm synaptic
(network-manager-gnome wireless-tools voor wireless)
reboot

# 4. synaptic install: 
firmware-linux firmware-linux-free firmware-linux-nonfree
leafpad lxterminal tuxcmd build-essential

# 5. synaptic install: (alsa-base) 
alsa-utils alsamixergui libalsaplayer0

sudo alsactl init
aplay /usr/share/sounds/alsa/*

remove pulseaudio
```