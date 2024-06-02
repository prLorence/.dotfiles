source ~/.zplug/init.zsh
# G# Set up the prompt

# autoload -Uz promptinit
# promptinit
# prompt adam1

setopt histignorealldups sharehistory

autoload -Uz compinit
compinit

zplug "jeffreytse/zsh-vi-mode"
zplug "agkozak/zsh-z"
zplug "zsh-users/zsh-syntax-highlighting", defer:2

# Keep 1000 lines of history within the shell and save it to ~/.zsh_history:
HISTSIZE=1000
SAVEHIST=1000
HISTFILE=~/.zsh_history

# Use modern completion system

ZVM_VI_INSERT_ESCAPE_BINDKEY=kj

alias kubectl="minikube kubectl --"

# Add .NET Core SDK tools
export PATH="$PATH:/home/phetoush/.dotnet/tools"
export DOTNET_ROOT="/usr/share/dotnet"

export NVM_DIR="$HOME/.nvm"

[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"  # This loads nvm
[ -s "$NVM_DIR/bash_completion" ] && \. "$NVM_DIR/bash_completion"  # This loads nvm bash_completion

export STARSHIP_CONFIG="~/.config/starship/starship.toml"

eval "$(starship init zsh)"

# Install plugins if there are plugins that have not been installed
if ! zplug check --verbose; then
    printf "Install? [y/N]: "
    if read -q; then
        echo; zplug install
    fi
fi

# Then, source plugins and add commands to $PATH
zplug load --verbose