#!/bin/sh

wget "https://discord.com/api/download?platform=linux&format=deb" -O discord.deb

sudo apt install ./discord.deb -y

rm discord.deb
