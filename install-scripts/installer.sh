#!/bin/sh
export BASE_DIR=$(dirname $(pwd))

# create required directories
mkdir -p $HOME/Desktop/Applications
mkdir -p $HOME/.config
mkdir -p $HOME/Pictures/Wallpapers

sudo apt upgrade && sudo sudo apt update -y

sudo apt install curl \
    wget \
    build-essential \
    feh \
    exa \
    fzf \
    -y

echo "setting executable permissions to all scripts"
chmod +x *.sh

echo "copying wallpapers"
/bin/bash wallpaper.sh

echo "installing firefox"
/bin/bash firefox.sh

echo "installing discord..."
/bin/bash discord.sh

echo "installing spotify..."
/bin/bash spotify.sh

echo "installing logseq..."
/bin/bash logseq.sh

echo "installing alacritty..."
/bin/bash alacritty.sh

echo "installing neovim..."
/bin/bash neovim.sh

echo "installing zsh..."
/bin/bash zsh.sh

echo "installing starship..."
/bin/bash starship.sh

echo "installing zplug..."
/bin/bash zplug.sh

echo "installing solaar..."
/bin/bash solaar.sh

echo "installing tmux..."
/bin/bash tmux.sh

echo "executing i3 installer..."
/bin/bash i3.sh

echo "executing configuration for i3-plasma workspace"
/bin/bash i3-kde.sh

echo "installing docker..."
/bin/bash docker.sh

# echo "migrating personal finance..."
# /bin/bash finance.sh

echo "installing minikube(k8s)..."
/bin/bash minikube.sh

echo "installing terraform..."
/bin/bash terraform.sh

if [[ $XDG_SESSION_TYPE == "x11" ]]; then
    /bin/bash xmodmap.sh;
fi;
