#!/bin/sh

# ~/Desktop/Applications should exist
# TODO: write an equivalent update script
wget "https://github.com/logseq/logseq/releases/download/0.10.9/Logseq-linux-x64-0.10.9.AppImage" -O $HOME/Desktop/Applications/Logseq.AppImage

chmod +x $HOME/Desktop/Applications/Logseq.AppImage

sudo ln -sf $HOME/Desktop/Applications/Logseq.AppImage /usr/bin/Logseq

cat > ./temp << "EOF"
[Desktop Entry]
Name=Logseq
StartupWMClass=logseq
Comment=An outliner note taking app
GenericName=Outliner Note Taking App
Exec=/usr/bin/Logseq
Type=Application
Categories=Note-Taking
Path=/usr/bin
EOF
sudo cp ./temp /usr/share/applications/logseq.desktop;rm ./temp

# TODO: git clone logseq to Documents, create PAT for cloning it with the following format
GITHUB_USERNAME=prlorence
GITHUB_PAT=changethis

git clone "https://$GITHUB_USERNAME:$GITHUB_PAT@github.com/$GITHUB_USERNAME/logseq.git" $HOME/Desktop/Applications
