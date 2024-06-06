BUILD_PATH=$HOME/Desktop/Applications
BASE_DIR=$(dirname $(pwd))
# install depedencies
sudo apt install cmake pkg-config libfreetype6-dev libfontconfig1-dev libxcb-xfixes0-dev libxkbcommon-dev python3 -y

# clone git repo
git clone https://github.com/alacritty/alacritty.git $BUILD_PATH

cd $BUILD_PATH/alacritty

# install rust
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh

rustup override set stable
rustup update stable

# install alacritty
cargo build --release

# Force support for only X11
cargo build --release --no-default-features --features=x11

echo "check alacritty installation"
infocmp alacritty

sudo tic -xe alacritty,alacritty-direct extra/alacritty.info

# create desktop entry
sudo cp target/release/alacritty /usr/local/bin # or anywhere else in $PATH
sudo cp extra/logo/alacritty-term.svg /usr/share/pixmaps/Alacritty.svg
sudo desktop-file-install extra/linux/Alacritty.desktop
sudo update-desktop-database

sudo mkdir -p /usr/local/share/man/man1
sudo mkdir -p /usr/local/share/man/man5
scdoc < extra/man/alacritty.1.scd | gzip -c | sudo tee /usr/local/share/man/man1/alacritty.1.gz > /dev/null
scdoc < extra/man/alacritty-msg.1.scd | gzip -c | sudo tee /usr/local/share/man/man1/alacritty-msg.1.gz > /dev/null
scdoc < extra/man/alacritty.5.scd | gzip -c | sudo tee /usr/local/share/man/man5/alacritty.5.gz > /dev/null
scdoc < extra/man/alacritty-bindings.5.scd | gzip -c | sudo tee /usr/local/share/man/man5/alacritty-bindings.5.gz > /dev/null

# symlink config
sudo -sf $BASE_DIR/alacritty $HOME/.config/alacritty
