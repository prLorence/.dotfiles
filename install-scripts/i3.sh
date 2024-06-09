#!/bin/sh


BASE_DIR=$(dirname $(pwd))

# installing i3
echo "installing i3..."


sudo apt install i3

# link config to .config

sudo ln -sf $BASE_DIR/i3 $HOME/.config/i3


