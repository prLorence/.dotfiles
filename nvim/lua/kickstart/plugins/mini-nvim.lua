return { -- Collection of various small independent plugins/modules
  'echasnovski/mini.nvim',
  -- event = 'VeryLazy',
  config = function()
    -- Better Around/Inside textobjects
    --
    -- Examples:
    --  - va)  - [V]isually select [A]round [)]paren
    --  - yinq - [Y]ank [I]nside [N]ext [Q]uote
    --  - ci'  - [C]hange [I]nside [']quote
    require('mini.ai').setup { n_lines = 500 }
    require('mini.sessions').setup()

    -- Add/delete/replace surroundings (brackets, quotes, etc.)
    --
    -- - saiw) - [S]urround [A]dd [I]nner [W]ord [)]Paren
    -- - sd'   - [S]urround [D]elete [']quotes
    -- - sr)'  - [S]urround [R]eplace [)] [']
    require('mini.diff').setup()
    require('mini.surround').setup()
    require('mini.files').setup {
      mappings = {
        close = 'q',
        go_in = '<M-l>',
        go_in_plus = '<CR>',
        go_out = '<M-h>',
        go_out_plus = '<BS>',
        mark_goto = "'",
        mark_set = 'm',
        reset = '<BS>',
        reveal_cwd = '-',
        show_help = 'g?',
        synchronize = '=',
        trim_left = '<',
        trim_right = '>',
      },
      options = {
        -- Whether to delete permanently or move into module-specific trash
        permanent_delete = false,
        -- Whether to use for editing directories
        use_as_default_explorer = false,
      },
    }
    require('mini.icons').setup()
    require('mini.trailspace').setup()

    -- Simple and easy statusline.
    --  You could remove this setup call if you don't like it,
    --  and try some other statusline plugin
    local statusline = require 'mini.statusline'
    -- set use_icons to true if you have a Nerd Font
    statusline.setup { use_icons = vim.g.have_nerd_font }

    -- You can configure sections in the statusline by overriding their
    -- default behavior. For example, here we set the section for
    -- cursor location to LINE:COLUMN
    ---@diagnostic disable-next-line: duplicate-set-field
    statusline.section_location = function()
      return '%2l:%-2v'
    end

    local map_split = function(buf_id, lhs, direction)
      local rhs = function()
        -- Make new window and set it as target
        local cur_target = MiniFiles.get_explorer_state().target_window
        local new_target = vim.api.nvim_win_call(cur_target, function()
          vim.cmd(direction .. ' split')
          return vim.api.nvim_get_current_win()
        end)

        MiniFiles.set_target_window(new_target)
        MiniFiles.go_in()
        MiniFiles.close()
      end

      -- Adding `desc` will result into `show_help` entries
      local desc = 'Split ' .. direction
      vim.keymap.set('n', lhs, rhs, { buffer = buf_id, desc = desc })
    end

    vim.api.nvim_create_autocmd('User', {
      pattern = 'MiniFilesBufferCreate',
      callback = function(args)
        local buf_id = args.data.buf_id
        -- Tweak keys to your liking
        map_split(buf_id, '<C-x>', 'belowright horizontal')
        map_split(buf_id, '<C-v>', 'belowright vertical')
      end,
    })

    vim.api.nvim_create_autocmd('User', {
      pattern = 'MiniFilesWindowOpen',
      callback = function(args)
        local win_id = args.data.win_id

        -- Customize window-local settings
        local config = vim.api.nvim_win_get_config(win_id)
        config.border, config.title_pos = 'double', 'right'
        vim.api.nvim_win_set_config(win_id, config)
      end,
    })

    -- sync on exit insert mode
    vim.api.nvim_create_autocmd('User', {
      pattern = 'MiniFilesWindowOpen',
      callback = function(args)
        local win_id = args.data.win_id
        local buf_id = vim.api.nvim_win_get_buf(win_id) -- Get buffer ID for the window

        -- Customize window-local settings
        -- Map kj to MiniFiles.synchronize() specifically for this buffer
        vim.keymap.set('i', 'kj', function()
          -- It's good practice to require the module inside the callback
          -- just in case, though it's likely already loaded.
          MiniFiles.synchronize()
          vim.api.nvim_input '<Esc>'
        end, {
          buffer = buf_id, -- Make the mapping local to the MiniFiles buffer
          noremap = true, -- Standard practice: non-recursive mapping
          silent = true, -- Don't echo the command being run
          desc = 'MiniFiles: Synchronize directory', -- Description for which-key etc.
        })
      end,
    })

    vim.api.nvim_create_autocmd('User', {
      pattern = 'MiniFilesWindowUpdate',
      callback = function(args)
        local config = vim.api.nvim_win_get_config(args.data.win_id)

        -- Ensure fixed height
        config.height = 10

        -- Ensure no title padding
        local n = #config.title
        config.title[1][1] = config.title[1][1]:gsub('^ ', '')
        config.title[n][1] = config.title[n][1]:gsub(' $', '')

        vim.api.nvim_win_set_config(args.data.win_id, config)
      end,
    })

    -- lsp rename on file opreation
    vim.api.nvim_create_autocmd('User', {
      pattern = 'MiniFilesActionRename',
      callback = function(event)
        Snacks.rename.on_rename_file(event.data.from, event.data.to)
      end,
    })
  end,
}
