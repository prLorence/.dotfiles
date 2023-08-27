local lazypath = vim.fn.stdpath("data") .. "/lazy/lazy.nvim"
if not vim.loop.fs_stat(lazypath) then
    vim.fn.system({
        "git",
        "clone",
        "--filter=blob:none",
        "https://github.com/folke/lazy.nvim.git",
        "--branch=stable", -- latest stable release
        lazypath,
    })
end
vim.opt.rtp:prepend(lazypath)

local plugins = {
    -- THEMES
    { "bluz71/vim-moonfly-colors",              name = "moonfly", lazy = false, priority = 1000 },
    {
        "folke/tokyonight.nvim",
        lazy = false,
        priority = 1000,
        opts = {},
    },
    {
        'navarasu/onedark.nvim',
        priority = 1000 -- Ensure it loads first
    },

    -- COSMETICS
    {
        'nvim-lualine/lualine.nvim',
        dependencies = { 'nvim-tree/nvim-web-devicons' }
    },
    {
        'stevearc/dressing.nvim',
        opts = {},
    },
    { "folke/zen-mode.nvim" },

    -- CONFIG QOL
    { "folke/neodev.nvim" },

    -- TREESITTER PLUGINS
    { 'windwp/nvim-ts-autotag' },
    { 'numToStr/Comment.nvim' },
    { 'nvim-treesitter/nvim-treesitter-context' },
    {
        "HiPhish/rainbow-delimiters.nvim",
        dependencies = { 'nvim-treesitter/nvim-treesitter' }
    },

    -- EDITOR PLUGINS
    { 'lukas-reineke/indent-blankline.nvim' },
    { 'jinh0/eyeliner.nvim' },
    { 'windwp/nvim-autopairs' },
    { 'tpope/vim-surround' },
    {
        "nvim-neo-tree/neo-tree.nvim",
        branch = "v3.x",
        dependencies = {
            "nvim-lua/plenary.nvim",
            "nvim-tree/nvim-web-devicons", -- not strictly required, but recommended
            "MunifTanjim/nui.nvim",
        }
    },
    {
        "folke/which-key.nvim",
        event = "VeryLazy",
        init = function()
            vim.o.timeout = true
            vim.o.timeoutlen = 500
        end,
    },

    -- GIT PLUGINS
    { 'tpope/vim-fugitive' },
    {
        'NeogitOrg/neogit',
        dependencies = {
            'nvim-lua/plenary.nvim',
            'nvim-telescope/telescope.nvim'
        }
    },
    { 'lewis6991/gitsigns.nvim' },

    -- AUTOCOMPLETION STUFF
    { 'hrsh7th/cmp-path' },
    { 'hrsh7th/cmp-buffer' },
    { 'hrsh7th/cmp-nvim-lsp-signature-help' },

    -- BETTER TELESCOPE FZF
    { 'nvim-telescope/telescope-fzf-native.nvim', build = 'make' },

    -- SNIPPETS
    { "rafamadriz/friendly-snippets" },
    {
        "L3MON4D3/LuaSnip",
        -- follow latest release.
        version = "2.*", -- Replace <CurrentMajor> by the latest released major (first number of latest release)
        dependencies = {
            "rafamadriz/friendly-snippets",
        },
        -- install jsregexp (optional!).
        build = "make install_jsregexp"
    },

    -- LSP STUFF
    {
        'folke/trouble.nvim',
        dependencies = { "nvim-tree/nvim-web-devicons" },
    },
    { 'Hoffs/omnisharp-extended-lsp.nvim' },
    { 'Issafalcon/lsp-overloads.nvim' },
    { 'nvim-treesitter/nvim-treesitter',  cmd = "TSUpdate" },
    {
        'nvim-telescope/telescope.nvim',
        tag = '0.1.2',
        -- or                              , branch = '0.1.1',
        dependencies = { 'nvim-lua/plenary.nvim' }
    },
    {
        'VonHeikemen/lsp-zero.nvim',
        branch = 'v2.x',
        dependencies = {
            -- LSP Support
            { 'neovim/nvim-lspconfig' }, -- Required
            {
                -- Optional
                'williamboman/mason.nvim',
                build = function()
                    vim.cmd('MasonUpdate')
                end,
            },
            { 'williamboman/mason-lspconfig.nvim' }, -- Optional

            -- Autocompletion
            { 'hrsh7th/nvim-cmp' },                                  -- Required
            { 'hrsh7th/cmp-nvim-lsp' },                              -- Required
            { 'L3MON4D3/LuaSnip',                 version = "2.*" }, -- Required
            { 'saadparwaiz1/cmp_luasnip' },
        }
    }
}

local opts = {
    performance = {
        cache = {
            enabled = true,
        }
    }
}

require("lazy").setup(plugins, opts)

require("nvim-autopairs").setup()

require("nvim-ts-autotag").setup()

-- require('lsp_signature').setup()

-- some indent guides config
-- vim.opt.list = true
-- vim.opt.listchars:append "eol:↴"
require('indent_blankline').setup {
    char = '┊',
    show_trailing_blankline_indent = false,
}

require('lualine').setup {
    options = {
        icons_enabled = true,
        theme = 'moonfly',
        component_separators = { left = '|', right = '|' },
        section_separators = { left = '', right = '' },
        disabled_filetypes = {
            statusline = {},
            winbar = {},
        },
        ignore_focus = {},
        always_divide_middle = true,
        globalstatus = false,
        refresh = {
            statusline = 1000,
            tabline = 1000,
            winbar = 1000,
        }
    },
    sections = {
        lualine_a = { 'mode' },
        lualine_b = { 'branch', 'diff', 'diagnostics' },
        lualine_c = { 'filetype', 'filename' },
        lualine_x = { 'encoding', },
        lualine_y = { 'progress' },
        lualine_z = { 'location' }
    },
    inactive_sections = {
        lualine_a = {},
        lualine_b = {},
        lualine_c = { 'filename' },
        lualine_x = { 'location' },
        lualine_y = {},
        lualine_z = {}
    },
    tabline = {},
    winbar = {},
    inactive_winbar = {},
    extensions = {}
}

require('Comment').setup({
    ---Add a space b/w comment and the line
    padding = true,
    ---Whether the cursor should stay at its position
    sticky = true,
    ---Lines to be ignored while (un)comment
    ignore = nil,
    ---LHS of toggle mappings in NORMAL mode
    toggler = {
        ---Line-comment toggle keymap
        line = 'gcc',
        ---Block-comment toggle keymap
        block = 'gbc',
    },
    ---LHS of operator-pending mappings in NORMAL and VISUAL mode
    opleader = {
        ---Line-comment keymap
        line = 'gc',
        ---Block-comment keymap
        block = 'gb',
    },
    ---LHS of extra mappings
    extra = {
        ---Add comment on the line above
        above = 'gcO',
        ---Add comment on the line below
        below = 'gco',
        ---Add comment at the end of line
        eol = 'gcA',
    },
    ---Enable keybindings
    ---NOTE: If given `false` then the plugin won't create any mappings
    mappings = {
        ---Operator-pending mapping; `gcc` `gbc` `gc[count]{motion}` `gb[count]{motion}`
        basic = true,
        ---Extra mapping; `gco`, `gcO`, `gcA`
        extra = true,
    },
    ---Function to call before (un)comment
    pre_hook = nil,
    ---Function to call after (un)comment
    post_hook = nil,
})
