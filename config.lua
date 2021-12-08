--[[
lvim is the global options object

Linters should be
filled in as strings with either
a global executable or a path to
an executable
]]
-- THESE ARE EXAMPLE CONFIGS FEEL FREE TO CHANGE TO WHATEVER YOU WANT
-- general
lvim.log.level = "warn"
lvim.format_on_save = true
lvim.colorscheme = "gruvbox"
vim.opt.relativenumber = true
vim.opt.fileencoding = "utf-8"
vim.opt.showmode = true
vim.opt.clipboard = "unnamedplus"
vim.opt.cmdheight = 2
vim.opt.tabstop = 8
vim.opt.shiftwidth = 2
vim.opt.swapfile = false
vim.opt.termguicolors = true
vim.opt.title = true
vim.opt.titlestring = "%<%F%=%l/%L - nvim"
vim.opt.updatetime = 300
vim.opt.numberwidth = 2
vim.opt.scrolloff = 8
-- keymappings [view all the defaults by pressing <leader>Lk]
lvim.leader = "space"
-- add your own keymapping
lvim.keys.normal_mode["<C-s>"] = ":w<cr>"
lvim.keys.normal_mode["<C-p>"] = ":Telescope find_files<cr>"
lvim.keys.normal_mode["<C-A-p>"] = ":Telescope git_files<cr>"
lvim.keys.normal_mode["<A-w"] = ":BufferClose<cr>"
lvim.keys.normal_mode["<C-r>"] = ":LvimCacheReset<cr>"
-- vim.cmd("map <kj> <ESC>")

-- unmap a default keymapping
lvim.keys.normal_mode["<C-Up>"] = ""
-- edit a default keymapping
-- lvim.keys.normal_mode["<C-q>"] = ":q<cr>"
lvim.keys.insert_mode["<kj>"] = "<ESC><cr>"
-- Change Telescope navigation to use j and k for navigation and n and p for history in both input and normal mode.
-- we use protected-mode (pcall) just in case the plugin wasn't loaded yet.
 local _, actions = pcall(require, "telescope.actions")
 lvim.builtin.telescope.defaults.mappings = {
   -- for input mode
   i = {
     ["<C-j>"] = actions.move_selection_next,
     ["<C-k>"] = actions.move_selection_previous,
     ["<C-n>"] = actions.cycle_history_next,
     ["<C-p>"] = actions.cycle_history_prev,
   },
   -- for normal mode
   n = {
     ["<C-j>"] = actions.move_selection_next,
     ["<C-k>"] = actions.move_selection_previous,
   },
 }

-- Use which-key to add extra bindings with the leader-key prefix
 lvim.builtin.which_key.mappings["P"] = { "<cmd>Telescope projects<CR>", "Projects" }
 lvim.builtin.which_key.mappings["t"] = {
   name = "+Trouble",
   r = { "<cmd>Trouble lsp_references<cr>", "References" },
   f = { "<cmd>Trouble lsp_definitions<cr>", "Definitions" },
   d = { "<cmd>Trouble lsp_document_diagnostics<cr>", "Diagnostics" },
   q = { "<cmd>Trouble quickfix<cr>", "QuickFix" },
   l = { "<cmd>Trouble loclist<cr>", "LocationList" },
   w = { "<cmd>Trouble lsp_workspace_diagnostics<cr>", "Diagnostics" },
 }
-- TODO: User Config for predefined plugins
-- After changing plugin config exit and reopen LunarVim, Run :PackerInstall :PackerCompile
lvim.builtin.dashboard.active = true
lvim.builtin.terminal.active = true
lvim.builtin.nvimtree.setup.view.side = "left"
lvim.builtin.nvimtree.show_icons.git = 0

-- if you don't want all the parsers change this to a table of the ones you want
lvim.builtin.treesitter.ensure_installed = {
  "bash",
  "c",
  "javascript",
  "json",
  "lua",
  "python",
  "typescript",
  "css",
  "rust",
  "java",
  "yaml",
}

lvim.builtin.treesitter.ignore_install = { "haskell" }
lvim.builtin.treesitter.highlight.enabled = true

-- generic LSP settings

-- ---@usage disable automatic installation of servers
-- lvim.lsp.automatic_servers_installation = false


-- See the full default list `:lua print(vim.inspect(lvim.lsp.override))`
-- vim.list_extend(lvim.lsp.override, { "pyright" })

-- ---@usage setup a server -- see: https://www.lunarvim.org/languages/#overriding-the-default-configuration
-- local opts = {} -- check the lspconfig documentation for a list of all possible options
-- require("lvim.lsp.manager").setup("pylsp", opts)

-- you can set a custom on_attach function that will be used for all the language servers
-- See <https://github.com/neovim/nvim-lspconfig#keybindings-and-completion>
-- lvim.lsp.on_attach_callback = function(client, bufnr)
--   local function buf_set_option(...)
--     vim.api.nvim_buf_set_option(bufnr, ...)
--   end
--   --Enable completion triggered by <c-x><c-o>
--   buf_set_option("omnifunc", "v:lua.vim.lsp.omnifunc")
-- end
-- you can overwrite the null_ls setup table (useful for setting the root_dir function)
 lvim.lsp.null_ls.setup = {
   root_dir = require("lspconfig").util.root_pattern("Makefile", ".git", "node_modules"),
 }
-- or if you need something more advanced
-- lvim.lsp.null_ls.setup.root_dir = function(fname)
--   if vim.bo.filetype == "javascript" then
--     return require("lspconfig/util").root_pattern("Makefile", ".git", "node_modules")(fname)
--       or require("lspconfig/util").path.dirname(fname)
--   elseif vim.bo.filetype == "php" then
--     return require("lspconfig/util").root_pattern("Makefile", ".git", "composer.json")(fname) or vim.fn.getcwd()
--   else
--     return require("lspconfig/util").root_pattern("Makefile", ".git")(fname) or require("lspconfig/util").path.dirname(fname)
--   end
-- end

-- set a formatter, this will override the language server formatting capabilities (if it exists)
 local formatters = require "formatter"
 formatters.setup {
   { exe = "black", filetypes = { "python" } },
   { exe = "clang-format", filetypes = {"java"} },
   {
     exe = "prettier",
     ---@usage arguments to pass to the formatter
     -- these cannot contain whitespaces, options such as `--line-width 80` become either `{'--line-width', '80'}` or `{'--line-width=80'}`
     args = { "--print-with", "100" },
     ---@usage specify which filetypes to enable. By default a providers will attach to all the filetypes it supports.
     filetypes = { "javascript", "javascriptreact", "typescript", "typescriptreact", "vue", "css", "scss", "less", "html", "json", "yaml", "markdown", "graphql"},
   },
 }

 -- set additional linters
-- local linters = require "lvim.lsp.null-ls.linters"
-- linters.setup {
--   { exe = "flake8", filetypes = { "python" } },
--   {
--     exe = "shellcheck",
     ---@usage arguments to pass to the formatter
     -- these cannot contain whitespaces, options such as `--line-width 80` become either `{'--line-width', '80'}` or `{'--line-width=80'}`
--     args = { "--severity", "warning" },
--   },
--   {
--     exe = "codespell",
--     ---@usage specify which filetypes to enable. By default a providers will attach to all the filetypes it supports.
--     filetypes = { "javascript", "python" },
--   },
-- }

-- Additional Plugins
lvim.plugins = {
     {"folke/tokyonight.nvim"},
     {"morhetz/gruvbox"},
     {"tpope/vim-fugitive"},
     {"tpope/vim-rhubarb"},
     {"BurntSushi/ripgrep"},
     {
       "folke/trouble.nvim",
       cmd = "TroubleToggle",
     },
     {
       "p00f/nvim-ts-rainbow",
       config = function ()
         require("nvim-treesitter.configs").setup {
        rainbow = {
          -- ...
          enable = true,
          -- disable = { "jsx", "cpp" }, list of languages you want to disable the plugin for
          extended_mode = true, -- Also highlight non-bracket delimiters like html tags, boolean or table: lang -> boolean
          max_file_lines = nil, -- Do not enable for files with more than n lines, int
          -- colors = {}, -- table of hex strings
          -- termcolors = {} -- table of colour name strings
        }
      }
       end
     },
     {
      "kevinhwang91/nvim-bqf",
      event = { "bufread", "bufnew" },
      config = function()
      require("bqf").setup({
        auto_enable = true,
        preview = {
        win_height = 12,
        win_vheight = 12,
        delay_syntax = 80,
        border_chars = { "┃", "┃", "━", "━", "┏", "┓", "┗", "┛", "█" },
        },
        func_map = {
        vsplit = "",
        ptogglemode = "z,",
        stoggleup = "",
        },
        filter = {
        fzf = {
        action_for = { ["ctrl-s"] = "split" },
        extra_opts = { "--bind", "ctrl-o:toggle-all", "--prompt", "> " },
      },
      },
      })
        end,
      },
      {
       "andymass/vim-matchup",
       event = "CursorMoved",
       config = function()
       vim.g.matchup_matchparen_offscreen = { method = "popup" }
       end,
     },
     {
      "Pocco81/AutoSave.nvim",
      config = function()
      require("autosave").setup()
      end,
     },
     {
      "karb94/neoscroll.nvim",
      event = "WinScrolled",
      config = function()
      require('neoscroll').setup({
        -- All these keys will be mapped to their corresponding default scrolling animation
        mappings = {'<C-u>', '<C-d>', '<C-b>', '<C-f>',
        '<C-y>', '<C-e>', 'zt', 'zz', 'zb'},
        hide_cursor = true,          -- Hide cursor while scrolling
        stop_eof = true,             -- Stop at <EOF> when scrolling downwards
        use_local_scrolloff = false, -- Use the local scope of scrolloff instead of the global scope
        respect_scrolloff = false,   -- Stop scrolling when the cursor reaches the scrolloff margin of the file
        cursor_scrolls_alone = true, -- The cursor will keep on scrolling even if the window cannot scroll further
        easing_function = nil,        -- Default easing function
        pre_hook = nil,              -- Function to run before the scrolling animation starts
        post_hook = nil,              -- Function to run after the scrolling animation ends
        })
      end
      },
      {
        "ethanholz/nvim-lastplace",
        event = "BufRead",
        config = function()
          require("nvim-lastplace").setup({
            lastplace_ignore_buftype = { "quickfix", "nofile", "help" },
            lastplace_ignore_filetype = {
              "gitcommit", "gitrebase", "svn", "hgcommit",
            },
            lastplace_open_folds = true,
          })
          end,
       },
       {
        "folke/todo-comments.nvim",
        event = "BufRead",
        config = function()
          require("todo-comments").setup()
        end,
       },
       {
        "turbio/bracey.vim",
        cmd = {"Bracey", "BracyStop", "BraceyReload", "BraceyEval"},
        run = "npm install --prefix server",
       },
       {
        "mhartington/formatter.nvim",
        config = function ()
          require("formatter").setup()
        end
       },
       {
       "windwp/nvim-ts-autotag",
       event = "InsertEnter",
       config = function()
       require("nvim-ts-autotag").setup()
       end,
       },
       {
        "lukas-reineke/indent-blankline.nvim",
        event = "BufRead",
        setup = function()
          vim.g.indentLine_enabled = 1
          vim.g.indent_blankline_char = "▏"
          vim.g.indent_blankline_filetype_exclude = {"help", "terminal", "dashboard"}
          vim.g.indent_blankline_buftype_exclude = {"terminal"}
          vim.g.indent_blankline_show_trailing_blankline_indent = false
          vim.g.indent_blankline_show_first_indent_level = false
        end
       },

 }

-- Autocommands (https://neovim.io/doc/user/autocmd.html)
-- lvim.autocommands.custom_groups = {
--   { "BufWinEnter", "*.lua", "setlocal ts=8 sw=8" },
-- }
