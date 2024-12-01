--  NOTE: Must happen before plugins are loaded (otherwise wrong leader will be used)

require 'phetoush.set'
require 'phetoush.remap'

-- [[ Install `lazy.nvim` plugin manager ]]
--    See `:help lazy.nvim.txt` or https://github.com/folke/lazy.nvim for more info
local lazypath = vim.fn.stdpath 'data' .. '/lazy/lazy.nvim'
if not vim.uv.fs_stat(lazypath) then
  local lazyrepo = 'https://github.com/folke/lazy.nvim.git'
  local out = vim.fn.system { 'git', 'clone', '--filter=blob:none', '--branch=stable', lazyrepo, lazypath }
  if vim.v.shell_error ~= 0 then
    error('Error cloning lazy.nvim:\n' .. out)
  end
end ---@diagnostic disable-next-line: undefined-field
vim.opt.rtp:prepend(lazypath)

-- NOTE: Here is where you install your plugins.
require('lazy').setup({
  'tpope/vim-sleuth', -- Detect tabstop and shiftwidth automatically
  require 'kickstart.plugins.indent_line',
  require 'kickstart.plugins.autopairs',
  require 'kickstart.plugins.gitsigns', -- adds gitsigns recommend keymaps
  require 'kickstart.plugins.go-nvim',
  require 'kickstart.plugins.cmp',
  require 'kickstart.plugins.telescope',
  require 'kickstart.plugins.treesitter',
  require 'kickstart.plugins.mini-nvim',
  require 'kickstart.plugins.lsp',
  require 'kickstart.plugins.lazydev',
  require 'kickstart.plugins.which-key',
  require 'kickstart.plugins.fugitive',
  require 'kickstart.plugins.oil',
  require 'kickstart.plugins.conform',
  require 'kickstart.plugins.lualine',
  require 'kickstart.plugins.snippets',
  require 'kickstart.plugins.luvit-meta',
  require 'kickstart.plugins.todo-comments',
  require 'kickstart.plugins.markview',
  require 'kickstart.plugins.noneckpain',
  require 'kickstart.plugins.better-escape',
  require 'kickstart.plugins.early-retirement',
  require 'kickstart.plugins.supermaven',
  require 'kickstart.plugins.noneckpain',
  require 'kickstart.plugins.nvim-vtsls',
  require 'kickstart.plugins.ufo',
  require 'kickstart.plugins.statuscol',

  -- TYPESCRIPT PLUGINS
  -- require 'kickstart.plugins.ts.typescript-tools',
  require 'kickstart.plugins.ts.ts-error-translator',
  require 'kickstart.plugins.ts.workspace-diagnostics',
  require 'kickstart.plugins.ts.nvim-lint',

  -- CSHARP PLUGINS
  -- require 'kickstart.plugins.roslyn',

  -- COLORSCHEMES
  require 'kickstart.coloschemes.tokyonight',

  -- NOTE: The import below can automatically add your own plugins, configuration, etc from `lua/custom/plugins/*.lua`
  --    This is the easiest way to modularize your config.
  --
  --  Uncomment the following line and add your plugins to `lua/custom/plugins/*.lua` to get going.
  --    For additional information, see `:help lazy.nvim-lazy.nvim-structuring-your-plugins`
  -- { import = 'custom.plugins' },
}, {
  ui = {
    -- If you are using a Nerd Font: set icons to an empty table which will use the
    -- default lazy.nvim defined Nerd Font icons, otherwise define a unicode icons table
    icons = vim.g.have_nerd_font and {} or {
      cmd = '⌘',
      config = '🛠',
      event = '📅',
      ft = '📂',
      init = '⚙',
      keys = '🗝',
      plugin = '🔌',
      runtime = '💻',
      require = '🌙',
      source = '📄',
      start = '🚀',
      task = '📌',
      lazy = '💤 ',
    },
  },
})

-- The line beneath this is called `modeline`. See `:help modeline`
-- vim: ts=2 sts=2 sw=2 et
