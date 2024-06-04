#!/bin/sh
# install starship
curl -sS https://starship.rs/install.sh | sh

# symlink the config path to ~/.config/starship/starship.toml
mkdir -p /home/$USER/.config/starship

sudo ln -sf ../starship/starship.toml /home/$USER/.config/starship/starship.toml


