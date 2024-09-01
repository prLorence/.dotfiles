return {
  'ray-x/navigator.lua',
  dependencies = {
    { 'ray-x/guihua.lua' },
    { 'neovim/nvim-lspconfig' },
  },
  config = function()
    require('navigator').setup {
      width = 0.7,
      -- icons = {icons = false}, -- disable all icons
      on_attach = function(client, bufnr)
        -- require'aerial'.on_attach(client, bufnr)
      end,
      border = 'single', -- "single",
      ts_fold = {
        enable = false,
      }, -- set to false to use "ufo"
      -- external = true, -- true: enable for goneovim multigrid otherwise false
      lsp_signature_help = true,
      combined_attach = 'their', -- both: use both customized attach and navigator default attach, mine: only use my attach defined in vimrc

      treesitter_navigation = { 'go', 'typescript' },
      -- default_mapping = false,
      --     keymaps = { { mode = 'i', key = '<M-k>', func = 'signature_help()' },
      -- { key = "<c-i>", func = "signature_help()" } },
      mason = true,
      lsp = {
        enable = true, -- skip lsp setup, and only use treesitter in navigator.
        -- Use this if you are not using LSP servers, and only want to enable treesitter support.
        -- If you only want to prevent navigator from touching your LSP server configs,
        -- use `disable_lsp = "all"` instead.
        -- If disabled, make sure add require('navigator.lspclient.mapping').setup({bufnr=bufnr, client=client}) in your
        -- own on_attach
        diagnostic = { enable = true },
        -- diagnostic_scrollbar_sign = false,
        format_on_save = { disable = { 'vue', 'go' } }, -- set to false to disasble lsp code format on save (if you are using prettier/efm/formater etc)
        disable_format_cap = {
          'sqls',
          'jsonls',
          'sumneko_lua',
          'lua_ls',
          'tflint',
          'terraform_lsp',
          'terraformls',
        }, -- a list of lsp not enable auto-format (e.g. if you using efm or vim-codeformat etc)
        disable_lsp = { 'rust_analyzer', 'tsserver' }, --e.g {denols} , use typescript.nvim
        -- code_lens = true,
        disply_diagnostic_qf = false, -- update diagnostic in quickfix window
        denols = { filetypes = {} },
        rename = { style = 'floating-preview' },
        -- lua_ls = {
        --   before_init = function()
        --     require('lazydev').setup({
        --       library = {
        --         { path = 'luvit-meta/library', words = { 'vim%.uv' } },
        --       },
        --     })
        --     require('neodev.lsp').before_init({}, { settings = { Lua = {} } })
        --   end,
        -- },
        tsserver = {
          filetypes = {
            'javascript',
            'javascriptreact',
            'javascript.jsx',
            'typescript',
            'typescriptreact',
            'typescript.tsx',
          },
          on_attach = function(client, bufnr, opts)
            client.server_capabilities.documentFormattingProvider = false -- allow efm to format
            -- require("aerial").on_attach(client, bufnr, opts)
          end,
        },
        flow = { autostart = false },

        -- sqlls = {},
        -- sqls = {
        --   on_attach = function(client, bufnr)
        --     client.server_capabilities.documentFormattingProvider = false -- efm
        --     require('sqls').on_attach(client, bufnr)
        --   end,
        -- },
        -- ccls = { filetypes = {} }, -- using clangd
        -- clangd = { filetypes = {} }, -- using clangd

        jedi_language_server = { filetypes = {} }, --another way to disable lsp
        servers = {
          'terraform_lsp',
          'vuels',
          'tailwindcss',
          'htmx',
          'html',
          'svelte',
          'cssls',
          'typos_lsp',
        }, -- , 'marksman' },
      },
    }
  end,
}
