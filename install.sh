#!/bin/bash

# Set up error handling
set -euo pipefail

APP_PATH="$HOME/Desktop/Applications"
OS=""
CONFIG_FILE="packages.json"

# Function to log messages
log() {
  echo "[$(date +'%Y-%m-%d %H:%M:%S')] $1"
}

# Function to check if a command exists
command_exists() {
  command -v "$1" >/dev/null 2>&1
}

# TODO: Function to check if running as root, use whoami instead
is_root() {
  [[ $EUID -eq 0 ]]
}

mkdepdirs() {
  mkdir -p "$APP_PATH"
  mkdir -p "$HOME/.config/kmonad"
  mkdir -p "$HOME/Pictures/Wallpapers"
}

detectdistro() {
  if [ -f /etc/os-release ]; then
    . /etc/os-release
    OS=$ID
  elif type lsb_release >/dev/null 2>&1; then
    OS=$(lsb_release -si)
  elif [ -f /etc/lsb-release ]; then
    . /etc/lsb-release
    OS=$DISTRIB_ID
  elif [ -f /etc/debian_version ]; then
    OS=Debian
  else
    OS=$(uname -s)
  fi
}

# Function to handle Debian-specific installations
debian_install() {
  local package=$1
  case $package in
  discord)
    wget "https://discord.com/api/download?platform=linux&format=deb" -O discord.deb
    sudo apt install ./discord.deb -y
    rm discord.deb
    ;;
  docker)
    log "Docker installation requires root privileges. Please run the Docker installation manually."
    log "Visit https://docs.docker.com/engine/install/debian/ for instructions."
    ;;
  firefox)
    log "Firefox installation requires root privileges. Please run the following commands manually:"
    log "sudo install -d -m 0755 /etc/apt/keyrings"
    log "wget -q https://packages.mozilla.org/apt/repo-signing-key.gpg -O- | sudo tee /etc/apt/keyrings/packages.mozilla.org.asc > /dev/null"
    log "echo \"deb [signed-by=/etc/apt/keyrings/packages.mozilla.org.asc] https://packages.mozilla.org/apt mozilla main\" | sudo tee -a /etc/apt/sources.list.d/mozilla.list > /dev/null"
    log "echo 'Package: *\nPin: origin packages.mozilla.org\nPin-Priority: 1000' | sudo tee /etc/apt/preferences.d/mozilla"
    log "sudo apt-get update && sudo apt-get install firefox"
    ;;
    # Add more Debian-specific installations here
  esac
}

# Function to handle Arch-specific installations
arch_install() {
  local package=$1
  yay --noconfirm -S "$package"
}

# Function to install a package based on the OS
install() {
  local package=$1
  if [ "$OS" == "debian" ]; then
    debian_install "$package"
  else
    arch_install "$package"
  fi
}

# Main installation function
main() {
  detectdistro
  mkdepdirs

  if [ ! -f "$CONFIG_FILE" ]; then
    log "Configuration file not found. Please create $CONFIG_FILE with the list of packages to install."
    exit 1
  fi

  # Read packages from config file
  packages=$(jq -r '.packages[]' "$CONFIG_FILE")

  for package in $packages; do
    log "Installing $package"
    install "$package"
  done

  log "Linking config files"
  for config in kitty starship nvim tmux sway waybar kanshi; do
    ln -sf "$(pwd)/$config" "$HOME/.config/$config"
  done

  log "Installation complete!"
}

# Run the main function
main
