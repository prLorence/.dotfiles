#!/bin/sh

echo "configuring kde plasma to use i3 as the window manager..."

mkdir -p $HOME/.config/systemd/user

cat > ./temp << "EOF"
[Unit]
Description=Launch Plasma with i3
Before=plasma-workspace.target

[Service]
ExecStart=/usr/bin/i3
Restart=on-failure

[Install]
WantedBy=plasma-workspace.target
EOF
sudo cp ./temp $HOME/.config/systemd/user/plasma-i3.service;rm ./temp

echo "Masking plasma-kwin_x11.service"
systemctl mask plasma-kwin_x11.service --user

echo "enabling plasma-i3 service" 
systemctl enable plasma-i3 --user

echo "NOTE: To go back to KWin, just unmask the plasma-kwin_x11.service and disable your plasma-i3 service in the same way."
