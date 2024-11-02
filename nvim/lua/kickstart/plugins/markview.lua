return {
  'OXY2DEV/markview.nvim',
  lazy = false, -- Recommended
  -- ft = "markdown" -- If you decide to lazy-load anyway

  dependencies = {
    -- You will not need this if you installed the
    -- parsers manually
    -- Or if the parsers are in your $RUNTIMEPATH
    'nvim-treesitter/nvim-treesitter',

    'nvim-tree/nvim-web-devicons',
  },
  config = function()
    local presets = require 'markview.presets'
    require('markview').setup {
      -- modes = { 'n', 'no', 'c' }, -- Change these modes
      -- to what you need

      hybrid_modes = { 'n' }, -- Uses this feature on
      -- normal mode
      --    -- Filetypes where the plugin will be enabled
      filetypes = { 'markdown', 'quarto', 'rmd' },

      -- Buftypes to ignore
      buf_ignore = { 'nofile' },

      -- This is nice to have
      callbacks = {
        on_enable = function(_, win)
          vim.wo[win].conceallevel = 2
          vim.wo[win].concealcursor = 'c'
        end,
      },
      checkboxes = presets.checkboxes.nerd,
      headings = presets.headings.marker,
      list_items = {
        enable = true,
        shift_width = 2,
        indent_size = 2,

        marker_minus = {},
        marker_plus = {},
        marker_star = {},
        marker_dot = {},
      },
      code_blocks = {
        style = 'minimal',
        icons = true,
        position = nil,
        min_width = 70,

        pad_amount = 3,
        pad_char = ' ',

        language_direction = 'left',
        language_names = {},

        hl = 'CursorLine',

        sign = true,
        sign_hl = nil,
      },
    }
  end,
}
