#!/bin/sh
# install starship
BASE_DIR=$(dirname $(pwd))

curl -sS https://starship.rs/install.sh | sh

# symlink the config path to ~/.config/starship/starship.toml
sudo ln -sf $BASE_DIR/starship $HOME/.config/starship
