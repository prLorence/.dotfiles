#!/bin/bash

# Set up error handling
source ./helper-functions/functions
source ./helper-functions/installers
ask=false

prevent_sudo_or_root
if ! command -v yay >/dev/null 2>&1; then
  echo -e "\e[33m[$0]: \"yay\" not found.\e[0m"
  showfun install_yay
  v install_yay
fi

set -euo pipefail

APP_PATH="$HOME/Desktop/Applications"

# Function to log messages

mkdepdirs() {
  mkdir -p "$APP_PATH"
  mkdir -p "$HOME/Pictures/Wallpapers"
}

install_local_pkgbuild() {
  local location=$1
  local installflags=$2

  x pushd $location

  source ./PKGBUILD
  x yay -S $installflags "${depends[@]}"

  x popd
}

setup_zsh() {
  for item in ".zshrc" ".zsh_plugins.txt"; do
    ln -sf "$(pwd)/zsh/$item" "$HOME"
  done
}

setup_systemd_services() {
  mkdir -p "$HOME"/.config/systemd/user/
  cp ./systemd/* "$HOME"/.config/systemd/user/
}

metapkgs=(./arch-packages/phetoush-{audio,backlight,basic,fonts-themes,screencapture,environment,portal})

for i in "${metapkgs[@]}"; do
  metainstallflags="--needed"
  $ask && showfun install_local_pkgbuild || metainstallflags="$metainstallflags --noconfirm"
  v install_local_pkgbuild "$i" "$metainstallflags"
done

main() {
  mkdepdirs
  cp -r ./wallpapers/ "$HOME/Pictures/Wallpapers/"

  echo "Linking config files..."
  for config in foot starship nvim tmux sway waybar kanshi fontconfig kmonad fuzzel gtklock mako pipewire lazygit flameshot; do
    ln -sf "$(pwd)/$config" "$HOME/.config/$config"
  done

  echo "Setting up zsh"
  setup_zsh

  echo "Setting the theme for GTK applications to dark..."
  gsettings set org.gnome.desktop.interface color-scheme 'prefer-dark'

  echo "Installation complete!"
}

# Run the main function
main
