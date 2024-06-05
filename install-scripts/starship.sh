#!/bin/sh
# install starship
BASE_DIR=$(dirname $(pwd))

curl -sS https://starship.rs/install.sh | sh

# symlink the config path to ~/.config/starship/starship.toml
mkdir -p $HOME/.config/starship

sudo ln -sf $BASE_DIR/starship/starship.toml $HOME/.config/starship/starship.toml
