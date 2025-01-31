source ~/.zplug/init.zsh
# G# Set up the prompt

# autoload -Uz promptinit
# promptinit
# prompt adam1

setopt histignorealldups sharehistory

# autoload -Uz compinit
# compinit

# zsh plugins through zplug
zplug "jeffreytse/zsh-vi-mode"
# zplug "agkozak/zsh-z"
zplug "zsh-users/zsh-syntax-highlighting", defer:2
zplug "plugins/git", from:oh-my-zsh
zplug "zsh-users/zsh-history-substring-search"
zplug "zsh-users/zsh-autosuggestions"

# Keep 1000 lines of history within the shell and save it to ~/.zsh_history:
HISTSIZE=1000
SAVEHIST=1000
HISTFILE=~/.zsh_history

# Use modern completion system
export ZVM_VI_INSERT_ESCAPE_BINDKEY=kj

bindkey '^ ' autosuggest-accept
export STARSHIP_CONFIG="~/.config/starship/starship.toml"

if [[ -e $HOME/.cargo/env ]]; then
    . $HOME/.cargo/env
fi

eval "$(starship init zsh)"

# Install plugins if there are plugins that have not been installed
if ! zplug check --verbose; then
    printf "Install? [y/N]: "
    if read -q; then
        echo
        zplug install
    fi
fi

# Then, source plugins and add commands to $PATH
zplug load

# programming tools
export PATH="$PATH:/home/phetoush/.dotnet/tools"
export DOTNET_ROOT="/usr/share/dotnet"

export NVM_DIR="$HOME/.nvm"

[ -s "$NVM_DIR/bash_completion" ] && \. "$NVM_DIR/bash_completion" # This loads nvm bash_completion
alias nvm="unalias nvm; [ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"; nvm $@"

export GOPATH="$HOME/go"

export PATH="$GOPATH/bin:$PATH"
export PATH="$PATH:/usr/local/go/bin"

if [ -z "$WAYLAND_DISPLAY" ] && [ -n "$XDG_VTNR" ] && [ "$XDG_VTNR" -eq 1 ]; then
    exec sway
    export DESKTOP_SESSION="sway"
fi

if [ "$XDG_SESSION_TYPE" = "wayland" ]; then
    export ELECTRON_OZONE_PLATFORM_HINT=wayland
    if [ "$DESKTOP_SESSION" = "sway" ]; then
        export XDG_CURRENT_DESKTOP="sway"
        export BEMENU_BACKEND="wayland"
        export MOZ_ENABLE_WAYLAND=1
    fi
fi

if [[ -z "${SSH_CONNECTION}" ]]; then
    export SSH_AUTH_SOCK="$XDG_RUNTIME_DIR/ssh-agent.socket"
fi

# aliases for common commands
alias ls='exa'
alias tf='terraform'
alias dk='docker'
alias dkc='docker compose'
alias k='kubectl'

# Created by `pipx` on 2024-11-08 07:34:29
export PATH="$PATH:/home/phetoush/.local/bin"

eval "$(zoxide init zsh)"

# Set up fzf key bindings and fuzzy completion
source <(fzf --zsh)
export FZF_ALT_C_OPTS="--preview 'tree -C {} | head -200'"
