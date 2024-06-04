#!/bin/sh
# install zsh
sudo apt install zsh

# remove empty zshrc file
rm /home/$USER/.zshrc

# pull config file and symlink to root directory
sudo ln -sf ../zsh/.zshrc /home/$USER/.zshrc
