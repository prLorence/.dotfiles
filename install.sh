#!/bin/bash

# Set up error handling
set -e

APP_PATH="$HOME/Desktop/Applications"
OS=""

mkappdir() {
  mkdir -p "$APP_PATH"
}

detectdistro() {
  if [ -f /etc/os-release ]; then
    # freedesktop.org and systemd
    . /etc/os-release
    OS=$ID
  elif type lsb_release >/dev/null 2>&1; then
    # linuxbase.org
    OS=$(lsb_release -si)
  elif [ -f /etc/lsb-release ]; then
    # For some versions of Debian/Ubuntu without lsb_release command
    . /etc/lsb-release
    OS=$DISTRIB_ID
  elif [ -f /etc/debian_version ]; then
    # Older Debian/Ubuntu/etc.
    OS=Debian
  else
    # Fall back to uname, e.g. "Linux <version>", also works for BSD, etc.
    OS=$(uname -s)
  fi
}

discord() {
  if [ "$OS" == "debian" ]; then
    wget "https://discord.com/api/download?platform=linux&format=deb" -O discord.deb
    sudo apt install ./discord.deb -y
    rm discord.deb
  else
    sudo pacman -S discord -y
  fi
}

docker() {
  if [ "$OS" == "debian" ]; then
    echo "Docker installation requires root privileges. Please run the Docker installation manually."
    echo "Visit https://docs.docker.com/engine/install/debian/ for instructions."
  else
    sudo pacman -S docker -y
  fi
}

firefox() {
  if [ "$OS" == "debian" ]; then
    echo "Firefox installation requires root privileges. Please run the following commands manually:"
    echo "sudo install -d -m 0755 /etc/apt/keyrings"
    echo "wget -q https://packages.mozilla.org/apt/repo-signing-key.gpg -O- | sudo tee /etc/apt/keyrings/packages.mozilla.org.asc > /dev/null"
    echo "echo \"deb [signed-by=/etc/apt/keyrings/packages.mozilla.org.asc] https://packages.mozilla.org/apt mozilla main\" | sudo tee -a /etc/apt/sources.list.d/mozilla.list > /dev/null"
    echo "echo 'Package: *\nPin: origin packages.mozilla.org\nPin-Priority: 1000' | sudo tee /etc/apt/preferences.d/mozilla"
    echo "sudo apt-get update && sudo apt-get install firefox"
  else
    sudo pacman -S firefox -y
  fi
}

i3kde() {
  echo "i3 and KDE installation requires root privileges. Please run the installation manually."
  echo "Here are the commands you need to run:"
  echo "sudo apt install i3 kde-standard plasma-nm"
  echo "Then follow the instructions for configuring KDE to use i3."
}

kitty() {
  if [ "$OS" == "debian" ]; then
    curl -L https://sw.kovidgoyal.net/kitty/installer.sh | sh /dev/stdin
    mkdir -p ~/.local/bin
    ln -sf ~/.local/kitty.app/bin/kitty ~/.local/kitty.app/bin/kitten ~/.local/bin/
    mkdir -p ~/.local/share/applications
    cp ~/.local/kitty.app/share/applications/kitty.desktop ~/.local/share/applications/
    cp ~/.local/kitty.app/share/applications/kitty-open.desktop ~/.local/share/applications/
    sed -i "s|Icon=kitty|Icon=$HOME/.local/kitty.app/share/icons/hicolor/256x256/apps/kitty.png|g" ~/.local/share/applications/kitty*.desktop
    sed -i "s|Exec=kitty|Exec=$HOME/.local/kitty.app/bin/kitty|g" ~/.local/share/applications/kitty*.desktop
    echo 'kitty.desktop' >~/.config/xdg-terminals.list
  else
    sudo pacman -S kitty -y
  fi
}

minikube() {
  if [ "$OS" == "debian" ]; then
    curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube_latest_amd64.deb
    sudo dpkg -i minikube_latest_amd64.deb
    rm minikube_latest_amd64.deb
  else
    sudo pacman -S minikube -y
  fi
}

nvim() {
  if [ "$OS" == "debian" ]; then
    sudo apt install file ninja-build gettext cmake unzip curl -y
    git clone https://github.com/neovim/neovim.git "$APP_PATH"/neovim
    cd "$APP_PATH"/neovim || exit
    git checkout stable
    make CMAKE_BUILD_TYPE=RelWithDebInfo
    cd build && cpack -G DEB && sudo dpkg -i nvim-linux64.deb
  else
    sudo pacman -S neovim -y
  fi
}

solaar() {
  if [ "$OS" == "debian" ]; then
    sudo apt install solaar -y
  else
    sudo pacman -S solaar -y
  fi
}

spotify() {
  if [ "$OS" == "debian" ]; then
    sudo apt install gpg -y
    curl -sS https://download.spotify.com/debian/pubkey_6224F9941A8AA6D1.gpg | gpg --dearmor | sudo tee /etc/apt/trusted.gpg.d/spotify.gpg >/dev/null
    echo "deb http://repository.spotify.com stable non-free" | sudo tee /etc/apt/sources.list.d/spotify.list
    sudo apt-get update && sudo apt-get install spotify-client -y
  else
    sudo pacman -S spotify-launcher -y
  fi
}

starship() {
  curl -sS https://starship.rs/install.sh | sh
}

zplug() {
  curl -sL --proto-redir -all,https https://raw.githubusercontent.com/zplug/installer/master/installer.zsh | zsh
}

zsh() {
  if [ "$OS" == "debian" ]; then
    sudo apt install zsh -y
  else
    sudo pacman -S zsh -y
  fi
  [ -f "$HOME/.zshrc" ] && mv "$HOME/.zshrc" "$HOME/.zshrc.bak"
  ln -sf "$(pwd)"/zsh/.zshrc "$HOME"
  echo "To change your default shell to zsh, run: chsh -s $(which zsh)"
}

terraform() {
  if [ "$OS" == "debian" ]; then
    echo "Terraform installation requires root privileges. Please run the installation manually."
    echo "Visit https://developer.hashicorp.com/terraform/downloads for instructions."
  else
    sudo pacman -S terraform -y
  fi
}

tmux() {
  if [ "$OS" == "debian" ]; then
    sudo apt install tmux -y
  else
    sudo pacman -S tmux -y
  fi
}

wallpapers() {
  mkdir -p "$HOME/Pictures/Wallpapers"
  cp ./wallpapers/* "$HOME/Pictures/Wallpapers"
}

xmodmap() {
  ln -sf ./xmodmapc/.Xmodmap "$HOME"
}

obsidian() {
  if [ "$OS" == "debian" ]; then
    wget https://github.com/obsidianmd/obsidian-releases/releases/download/v1.6.7/obsidian_1.6.7_amd64.deb -O "$HOME/Downloads/obsidian.deb"
    sudo dpkg -i "$HOME/Downloads/obsidian.deb"
    rm "$HOME/Downloads/obsidian.deb"
  else
    sudo pacman -S obsidian -y
  fi
}

nvm() {
  curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.1/install.sh | bash
}

go() {
  if [ "$OS" == "debian" ]; then
    wget https://go.dev/dl/go1.23.0.linux-amd64.tar.gz
    sudo rm -rf /usr/local/go && sudo tar -C /usr/local -xzf go1.23.0.linux-amd64.tar.gz
    rm go1.23.0.linux-amd64.tar.gz
    echo 'export PATH=$PATH:/usr/local/go/bin' >>"$HOME/.profile"
  else
    sudo pacman -S go -y
  fi
}

dotnet() {
  if [ "$OS" == "debian" ]; then
    wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
  else
    sudo pacman -S dotnet-sdk -y
  fi
}

hyprland() {
  git clone https://github.com/end-4/dots-hyprland.git
  sh ./dots-hyprland/install.sh --skip-fish
}

installde() {
  if [ "$OS" == "debian" ]; then
    i3kde
  else
    hyprland
  fi
}

main() {
  detectdistro

  echo "Installing terminal emulator dependencies"
  kitty
  starship
  zplug
  zsh
  tmux
  nvim

  echo "Installing development kits"
  terraform
  dotnet
  go
  nvm
  minikube
  docker

  echo "Installing applications"
  obsidian
  spotify
  wallpapers
  discord
  solaar

  echo "Linking config files"
  for i in kitty starship zsh nvim tmux; do
    ln -sf "$(pwd)/$i" "$HOME/.config/$i"
  done
}

main
