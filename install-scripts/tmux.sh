# install tmux
BASE_DIR=$(dirname $(pwd))

sudo apt install tmux

# link tmux config
sudo ln -sf $BASE_DIR/tmux $HOME/tmux

