#!/bin/bash
#Build prerequisites
sudo apt install ninja-build gettext cmake unzip curl -y

BASE_DIR=$(dirname $(pwd))

BUILD_PATH=$HOME/Desktop/Application

# clone
git clone https://github.com/neovim/neovim $BUILD_PATH

cd $BUILD_PATH

git checkout stable

make CMAKE_BUILD_TYPE=RelWithDebInfo

cd build && cpack -G DEB && sudo dpkg -i nvim-linux64.deb

# link to config path
sudo ln -sf $(BASE_DIR)/nvim /home/$USER/.config/nvim
