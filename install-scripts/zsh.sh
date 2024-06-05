#!/bin/sh
# install zsh
sudo apt install zsh -y

BASE_DIR=$(dirname $(pwd))

# remove empty zshrc file
rm $HOME/.zshrc

# pull config file and symlink to root directory
sudo ln -sf $BASE_DIR/zsh/.zshrc $HOME/.zshrc

# set zsh as default shell
chsh -s $(which zsh)
