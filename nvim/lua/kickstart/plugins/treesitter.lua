return { -- Highlight, edit, and navigate code
  'nvim-treesitter/nvim-treesitter',
  build = ':TSUpdate',
  opts = {
    ensure_installed = {
      'c_sharp',
      'lua',
      'vim',
      'javascript',
      'typescript',
      'python',
      'terraform',
      'go',
      'sql',
      'html',
      'css',
      'regex',
      'bash',
      'markdown',
      'markdown_inline',
      'json',
      'http',
    },
    -- Autoinstall languages that are not installed
    auto_install = true,
    highlight = {
      enable = true,
      -- Some languages depend on vim's regex highlighting system (such as Ruby) for indent rules.
      --  If you are experiencing weird indenting issues, add the language to
      --  the list of additional_vim_regex_highlighting and disabled languages for indent.
      additional_vim_regex_highlighting = { 'ruby' },
    },
    indent = { enable = true, disable = { 'ruby' } },

    textobjects = {
      select = {
        enable = true,

        -- Automatically jump forward to textobj, similar to targets.vim
        lookahead = true,
      },
    },
  },
  config = function(_, opts)
    -- [[ Configure Treesitter ]] See `:help nvim-treesitter`

    ---@diagnostic disable-next-line: missing-fields
    require('nvim-treesitter.configs').setup(opts)

    -- -- https://github.com/windwp/nvim-ts-autotag/issues/19
    vim.lsp.handlers['textDocument/publishDiagnostics'] = vim.lsp.with(vim.lsp.diagnostic.on_publish_diagnostics, {
      underline = {
        min = 'Warning',
      },
      virtual_text = {
        spacing = 5,
        min = 'Error',
      },
      update_in_insert = false,
    })

    vim.api.nvim_create_augroup('diagnostics', { clear = true })

    vim.api.nvim_create_autocmd('DiagnosticChanged', {
      group = 'diagnostics',
      callback = function()
        vim.diagnostic.setloclist { open = false }
      end,
    })
    -- There are additional nvim-treesitter modules that you can use to interact
    -- with nvim-treesitter. You should go explore a few and see what interests you:
    --
    --    - Incremental selection: Included, see `:help nvim-treesitter-incremental-selection-mod`
    --    - Show your current context: https://github.com/nvim-treesitter/nvim-treesitter-context
    --    - Treesitter + textobjects: https://github.com/nvim-treesittr/nvim-treesitter-textobjects
    --    vim.lsp.handlers['textDocument/publishDiagnostics'] = vim.lsp.with(
  end,
}
