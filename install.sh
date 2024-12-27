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
  mkdir -p "$HOME/.config/kmonad"
  mkdir -p "$HOME/Pictures/Wallpapers"
}

install_local_pkgbuild() {
  local location=$1
  local installflags=$2

  x pushd $location

  source ./PKGBUILD
  echo x yay -S $installflags --asdeps "${depends[@]}"
  echo x makepkg -si --noconfirm

  x popd
}

metapkgs=(./arch-packages/phetoush-{audio,backlight,basic,fonts-themes,screencapture,environment,portal})

for i in "${metapkgs[@]}"; do
  metainstallflags="--needed"
  $ask && showfun install_local_pkgbuild || metainstallflags="$metainstallflags --noconfirm"
  v install_local_pkgbuild "$i" "$metainstallflags"
done

main() {
  mkdepdirs

  log "Linking config files"
  for config in kitty starship nvim tmux sway waybar kanshi; do
    echo ln -sf "$(pwd)/$config" "$HOME/.config/$config"
  done

  log "Installation complete!"
}

# Run the main function
main
