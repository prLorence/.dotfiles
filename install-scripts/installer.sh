#!/bin/sh

sudo apt upgrade && sudo apt update -y

mkdir -p /home/$USER/Dekstop/Applications

echo "setting executable permissions to discord-install.sh"
chmod +x ./discord-install.sh

echo "setting executable permissions to discord-update.sh"
chmod +x ./discord-update.sh

echo "setting executable permissions to logseq-install.sh"
chmod +x ./logseq-install.sh

echo "setting executable permissions to neovim.sh"
chmod +x ./neovim.sh

echo "setting executable permissions to spotify-install.sh"
chmod +x ./spotify-install.sh

echo "installing discord..."
/bin/bash ./disord-install.sh

echo "installing logseq..."
/bin/bash ./logseq-install.sh

echo "installing neovim..."
/bin/bash ./neeovim.sh

echo "installing spotify..."
/bin/bash ./spotify.sh
