return {
  'olimorris/codecompanion.nvim',
  dependencies = {
    'nvim-lua/plenary.nvim',
    'nvim-treesitter/nvim-treesitter',
  },
  opts = {
    -- Display configuration table
    adapters = {
      opts = {
        show_model_choice = true,
      },
    },
    display = {
      chat = {
        -- Introductory message shown at the top of the chat buffer
        intro_message = 'Welcome to CodeCompanion ✨! Press ? for options',
        -- Show header separators in the chat buffer? Set this to false if you're using an external markdown formatting plugin
        show_header_separator = false,
        -- Automatically scroll to the bottom when new messages arrive
        auto_scroll = false,
      },
    },
    -- print hellow world
    --
    strategies = {
      -- Change the default chat adapter
      chat = {
        adapter = {
          name = 'copilot',
          model = 'claude-sonnet-4',
        },
        roles = {
          llm = 'CodeCompanion',
          user = 'Me',
        },
        keymaps = {
          close = {
            modes = {
              n = 'q',
            },
            index = 3,
            callback = 'keymaps.close',
            description = 'Close Chat',
          },
          stop = {
            modes = {
              n = '<C-c>',
            },
            index = 4,
            callback = 'keymaps.stop',
            description = 'Stop Request',
          },
        },
      },
      inline = {
        adapter = {
          name = 'copilot',
          model = 'claude-sonnet-4',
        },
      },
    },
  },
  keys = {
    {
      '<leader>ac',
      '<cmd>CodeCompanionActions<cr>',
      mode = { 'n', 'v' },
      noremap = true,
      silent = true,
      desc = 'CodeCompanion actions',
    },
    {
      '<leader>aa',
      '<cmd>CodeCompanionChat Toggle<cr>',
      mode = { 'n', 'v' },
      noremap = true,
      silent = true,
      desc = 'CodeCompanion chat',
    },
    {
      '<leader>ad',
      '<cmd>CodeCompanionChat Add<cr>',
      mode = 'v',
      noremap = true,
      silent = true,
      desc = 'CodeCompanion add to chat',
    },
  },
}
