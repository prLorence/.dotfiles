#!/bin/bash
#Build prerequisites
sudo apt install file ninja-build gettext cmake unzip curl -y

BASE_DIR=$(dirname $(pwd))

BUILD_PATH=$HOME/Desktop/Applications

# clone
git clone https://github.com/neovim/neovim.git $BUILD_PATH/neovim

cd $BUILD_PATH/neovim

git checkout stable

make CMAKE_BUILD_TYPE=RelWithDebInfo

cd build && cpack -G DEB && sudo dpkg -i nvim-linux64.deb

# link to config path
sudo ln -sf $BASE_DIR/nvim $HOME/.config/nvim
