# install tmux
BASE_DIR=$(dirname $(pwd))

sudo apt install tmux -y

# link tmux config
sudo ln -sf $BASE_DIR/tmux $HOME/.config/tmux
