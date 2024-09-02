#!/bin/bash

# TODO: add config checks

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

    apt install ./discord.deb -y

    rm discord.deb
  else
    pacman -S discord -y
  fi
}

docker() {
  if [ "$OS" == "debian" ]; then
    echo "Add Docker's official GPG key"
    apt-get update

    apt-get install ca-certificates curl

    install -m 0755 -d /etc/apt/keyrings

    curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc

    chmod a+r /etc/apt/keyrings/docker.asc

    # Add the repository to Apt sources:
    echo \
      "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/debian \
    $(. /etc/os-release && echo "$VERSION_CODENAME") stable" |
      tee /etc/apt/sources.list.d/docker.list >/dev/null

    apt-get update

    echo "installing docker"
    apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

    docker run hello-world
  else
    pacman -S docker -y
  fi

}

firefox() {
  if [ "$OS" == "debian" ]; then
    install -d -m 0755 /etc/apt/keyrings

    wget -q https://packages.mozilla.org/apt/repo-signing-key.gpg -O- | tee /etc/apt/keyrings/packages.mozilla.org.asc >/dev/null

    echo "deb [signed-by=/etc/apt/keyrings/packages.mozilla.org.asc] https://packages.mozilla.org/apt mozilla main" | tee -a /etc/apt/sources.list.d/mozilla.list >/dev/null

    echo '
    Package: *
    Pin: origin packages.mozilla.org
    Pin-Priority: 1000
    ' | tee /etc/apt/preferences.d/mozilla

    apt-get install firefox
  else
    pacman -S firefox -y
  fi
}
# if i'm on arch
# TODO: hyprland() {
#
# }

i3kde() {
  echo "installing i3..."
  apt install i3

  echo "installing kde DE"
  apt install kde-stander plasma-nm

  echo "linking i3 config..."
  ln -sf "$(pwd)/i3" "$HOME"/.config/i3

  echo "configuring kde plasma to use i3 as the window manager..."

  mkdir -p "$HOME"/.config/systemd/user

  cat >./temp <<"EOF"
[Unit]
Description=Launch Plasma with i3
Before=plasma-workspace.target

[Service]
ExecStart=/usr/bin/i3
Restart=on-failure

[Install]
WantedBy=plasma-workspace.target
EOF
  cp ./temp "$HOME"/.config/systemd/user/plasma-i3.service
  rm ./temp

  echo "Masking plasma-kwin_x11.service"
  systemctl mask plasma-kwin_x11.service --user

  echo "enabling plasma-i3 service"
  systemctl enable plasma-i3 --user

  echo "NOTE: To go back to KWin, just unmask the plasma-kwin_x11.service and disable your plasma-i3 service in the same way."
}

kitty() {
  if [ "$OS" == "debian" ]; then
    curl -L https://sw.kovidgoyal.net/kitty/installer.sh | sh /dev/stdin

    # Create symbolic links to add kitty and kitten to PATH (assuming ~/.local/bin is in
    # your system-wide PATH)
    ln -sf ~/.local/kitty.app/bin/kitty ~/.local/kitty.app/bin/kitten ~/.local/bin/

    # make it runnable anywhere
    ln -sf ~/.local/kitty.app/bin/kitty /usr/local/bin/kitty
    # Place the kitty.desktop file somewhere it can be found by the OS
    cp ~/.local/kitty.app/share/applications/kitty.desktop ~/.local/share/applications/
    # If you want to open text files and images in kitty via your file manager also add the kitty-open.desktop file
    cp ~/.local/kitty.app/share/applications/kitty-open.desktop ~/.local/share/applications/
    # Update the paths to the kitty and its icon in the kitty desktop file(s)
    sed -i "s|Icon=kitty|Icon=$(readlink -f ~)/.local/kitty.app/share/icons/hicolor/256x256/apps/kitty.png|g" ~/.local/share/applications/kitty*.desktop
    sed -i "s|Exec=kitty|Exec=$(readlink -f ~)/.local/kitty.app/bin/kitty|g" ~/.local/share/applications/kitty*.desktop
    # Make xdg-terminal-exec (and hence desktop environments that support it use kitty)
    echo 'kitty.desktop' >~/.config/xdg-terminals.list
  else
    pacman -S kitty -y
  fi
}

minikube() {
  if [ "$OS" == "debian" ]; then
    curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube_latest_amd64.deb

    dpkg -i minikube_latest_amd64.deb

    rm minikube_latest_amd64.deb
  else
    pacman -S minikube -y
  fi
}

nvim() {
  if [ "$OS" == "debian" ]; then
    apt install file ninja-build gettext cmake unzip curl -y
    git clone https://github.com/neovim/neovim.git "$APP_PATH"/neovim

    cd "$APP_PATH"/neovim || exit

    git checkout stable

    make CMAKE_BUILD_TYPE=RelWithDebInfo

    cd build && cpack -G DEB && dpkg -i nvim-linux64.deb
  else
    pacman -S neovim -y
  fi

  # clone

}

# for logitech devices
solaar() {
  if [ "$OS" == "debian" ]; then
    apt install solaar -y
  else
    pacman -S solaar -y
  fi
}

spotify() {
  if [ "$OS" == "debian" ]; then
    apt install gpg -y

    # registering spotify to apt
    curl -sS https://download.spotify.com/debian/pubkey_6224F9941A8AA6D1.gpg | gpg --dearmor --yes -o /etc/apt/trusted.gpg.d/spotify.gpg

    echo "deb http://repository.spotify.com stable non-free" | tee /etc/apt/sources.list.d/spotify.list

    # installing spotify
    apt-get update && sudo apt-get install spotify-client -y
  else
    pacman -S spotify-launcher -y
  fi
}

starship() {
  if [ "$OS" == "debian" ]; then
    curl -sS https://starship.rs/install.sh | sh
  else
    pacman -S starship -y
  fi

}

zplug() {
  curl -sL --proto-redir -all,https https://raw.githubusercontent.com/zplug/installer/master/installer.zsh | zsh
}

zsh() {
  # install zsh
  if [ "$OS" == "debian" ]; then
    apt install zsh -y
  else
    pacman -S zsh -y
  fi

  # remove empty zshrc file
  rm "$HOME"/.zshrc

  ln -sf "$(pwd)"/zsh/.zshrc "$HOME"

  # set zsh as default shell
  chsh -s "$(which zsh)"

}

terraform() {
  if [ "$OS" == "debian" ]; then
    echo "registering hashicorp repository"
    wget -O- https://apt.releases.hashicorp.com/gpg | gpg --dearmor -o /usr/share/keyrings/hashicorp-archive-keyring.gpg

    echo "deb [signed-by=/usr/share/keyrings/hashicorp-archive-keyring.gpg] https://apt.releases.hashicorp.com $(lsb_release -cs) main" | tee /etc/apt/sources.list.d/hashicorp.list

    apt update && sudo apt install terraform
  else
    pacman -S terraform -y
  fi
}

tmux() {
  if [ "$OS" == "debian" ]; then
    apt install tmux -y
  else
    pacman -S tmux -y
  fi
}

wallpapers() {
  cp ./wallpapers/* "$HOME"/Pictures/Wallpapers
}

xmodmap() {
  ln -sf ./xmodmapc/.Xmodmap "$HOME"
}

obsidian() {
  if [ "$OS" == "debian" ]; then
    # TODO: clone my notes using secret key from github
    wget https://github.com/obsidianmd/obsidian-releases/releases/download/v1.6.7/obsidian_1.6.7_amd64.deb -O "$XDG_CONFIG_HOME"/Downloads/obisidan.deb

    dpkg -i "$XDG_CONFIG_HOME"/Downloads/obisidan.deb

    rm obisidan.deb
  else
    pacman -S obsidian -y
  fi
}

nvm() {
  curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.1/install.sh | bash
}

go() {
  if [ "$OS" == "debian" ]; then
    wget https://go.dev/dl/
    rm -rf /usr/local/go && tar -C /usr/local -xzf go1.23.0.linux-amd64.tar.gz
  else
    pacman -S go -y
  fi
}

dotnet() {
  if [ "$OS" == "debian" ]; then
    wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    rm packages-microsoft-prod.deb
    apt-get update &&
      apt-get install -y dotnet-sdk-8.0
  else
    pacman -S dotnet-sdk -y
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
  wallpaper
  discord
  solaar

  echo "Linking config files"
  for i in kitty starship zplug zsh nvim tmux nvim; do
    ln -sf "$(pwd)/$i" "$HOME"/$i
  done

}

main
