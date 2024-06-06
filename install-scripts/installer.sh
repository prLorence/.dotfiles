#!/bin/sh
sudo apt upgrade && sudo apt update -y

sudo apt install curl -y

# create required directories
mkdir -p $HOME/Desktop/Applications
mkdir -p $HOME/.config

echo "setting executable permissions to all scripts"
chmod +x *.sh

# TODO: implement some sort of looping for all scripts present in the directory

echo "installing discord..."
/bin/bash discord.sh

echo "installing spotify..."
/bin/bash spotify.sh

# echo "installing logseq..."
# /bin/bash logseq.sh

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
