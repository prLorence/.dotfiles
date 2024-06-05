#!/bin/sh
# install starship
curl -sS https://starship.rs/install.sh | sh

# symlink the config path to ~/.config/starship/starship.toml
mkdir -p $HOME/.config/starship

sudo ln -sf ../starship/starship.toml $HOME/.config/starship/starship.toml
