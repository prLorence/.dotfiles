zsh_plugins=${ZDOTDIR:-$HOME}/.zsh_plugins
if [[ ! ${zsh_plugins}.zsh -nt ${zsh_plugins}.txt ]]; then
    (
        source /usr/share/zsh-antidote/antidote.zsh
        antidote bundle <${zsh_plugins}.txt >${zsh_plugins}.zsh
    )
fi
source ${zsh_plugins}.zsh
setopt histignorealldups sharehistory

# Keep 1000 lines of history within the shell and save it to ~/.zsh_history:
HISTSIZE=1000
SAVEHIST=1000
HISTFILE=~/.zsh_history

# Use modern completion system

bindkey '^ ' autosuggest-accept
export STARSHIP_CONFIG="~/.config/starship/starship.toml"

if [[ -e $HOME/.cargo/env ]]; then
    . $HOME/.cargo/env
fi

eval "$(starship init zsh)"

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

# programming tools
export PATH="$PATH:/home/phetoush/.dotnet/tools"
export DOTNET_ROOT="/usr/share/dotnet"

export NVM_DIR="$HOME/.nvm"

export GOPATH="$HOME/go"

export PATH="$GOPATH/bin:$PATH"
export PATH="$PATH:/usr/local/go/bin"
export PATH=~/.npm-global/bin:$PATH

export ZVM_VI_INSERT_ESCAPE_BINDKEY=kj

export PATH="$PATH:/home/phetoush/.local/bin"

# aliases for common commands

[ -s "$NVM_DIR/bash_completion" ] && \. "$NVM_DIR/bash_completion" # This loads nvm bash_completion
alias nvm="unalias nvm; [ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"; nvm $@"

alias ls='exa'
alias tf='terraform'
alias dk='docker'
alias dkc='docker compose'
alias k='kubectl'
alias lg='lazygit'

# Created by `pipx` on 2024-11-08 07:34:29

eval "$(zoxide init zsh)"

# Set up fzf key bindings and fuzzy completion
source <(fzf --zsh)
export FZF_ALT_C_OPTS="--preview 'tree -C {} | head -200'"

enable-fzf-tab
