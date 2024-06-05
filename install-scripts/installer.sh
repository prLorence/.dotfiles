#!/bin/sh
sudo apt upgrade && sudo apt update -y

sudo apt install curl -y

# some apps im using live in this directory
mkdir -p $HOME/Dekstop/Applications

echo "setting executable permissions to all scripts"
chmod +x *.sh

# TODO: implement some sort of looping for all scripts present in the directory

echo "installing discord..."
/bin/bash discord.sh

echo "installing logseq..."
/bin/bash logseq.sh

echo "installing neovim..."
/bin/bash neovim.sh

echo "installing spotify..."
/bin/bash spotify.sh

echo "installing zsh..."
/bin/bash zsh.sh

echo "installing solaar..."
/bin/bash solaar.sh

echo "installing starship..."
/bin/bash starship.sh

echo "installing alacritty..."
/bin/bash alacritty.sh

# TODO: symlinks for the configs
