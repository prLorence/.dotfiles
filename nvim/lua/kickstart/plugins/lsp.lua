return {
  -- Main LSP Configuration
  'neovim/nvim-lspconfig',
  dependencies = {
    -- Automatically install LSPs and related tools to stdpath for Neovim
    { 'williamboman/mason.nvim', config = true }, -- NOTE: Must be loaded before dependants
    { 'williamboman/mason-lspconfig.nvim' },
    { 'WhoIsSethDaniel/mason-tool-installer.nvim' },

    -- Useful status updates for LSP.
    -- NOTE: `opts = {}` is the same as calling `require('fidget').setup({})`
    { 'j-hui/fidget.nvim', opts = {} },

    -- Allows extra capabilities provided by nvim-cmp
    { 'hrsh7th/cmp-nvim-lsp' },
    {
      'folke/lazydev.nvim',
      ft = 'lua',
      opts = {
        library = {
          -- See the configuration section for more details
          -- Load luvit types when the `vim.uv` word is found
          { path = '${3rd}/luv/library', words = { 'vim%.uv' } },
        },
      },
    },
  },
  config = function()
    vim.api.nvim_create_autocmd('LspAttach', {
      group = vim.api.nvim_create_augroup('kickstart-lsp-attach', { clear = true }),
      callback = function(event)
        -- When you move your cursor, the highlights will be cleared (the second autocommand).
        require('kickstart.mappings').lsp_mappings(event.buf)
        require('kickstart.core_lsp_settings').core_lsp_client_settings(event)
      end,
    })

    local capabilities = vim.lsp.protocol.make_client_capabilities()
    capabilities = vim.tbl_deep_extend('force', capabilities, require('blink.cmp').get_lsp_capabilities())
    -- Example: Add or override a specific capability
    capabilities.textDocument = capabilities.textDocument or {}
    capabilities.textDocument.completion = capabilities.textDocument.completion or {}
    capabilities.textDocument.completion.completionItem = capabilities.textDocument.completion.completionItem or {}
    capabilities.textDocument.completion.completionItem.snippetSupport = true -- Ensure snippet support is enabled

    require('mason').setup {
      ui = {
        border = 'single',
      },
      registries = {
        'github:nvim-java/mason-registry',
        'github:mason-org/mason-registry',
      },
    }

    -- You can add other tools here that you want Mason to install
    -- for you, so that they are available from within Neovim.
    local servers = require('kickstart.lsp_client_settings').lsp_client_settings()

    local ensure_installed = vim.tbl_keys(servers or {})

    vim.list_extend(ensure_installed, {
      'stylua', -- Used to format Lua code
    })

    require('mason-tool-installer').setup { ensure_installed = ensure_installed }

    vim.lsp.config('vtsls', {
      capabilities = capabilities,
      cmd = servers['vtsls'].cmd,
      root_markers = servers['vtsls'].root_markers,
      handlers = servers['vtsls'].handlers,
      settings = {
        -- handlers = servers['vtsls'].handlers,
        typescript = servers['vtsls'].settings.typescript,
        javascript = servers['vtsls'].settings.javascript,
        vtsls = {
          servers['vtsls'].settings.vtsls,
        },
      },
    })

    vim.lsp.enable 'vtsls'

    for server_name, _ in pairs(servers) do
      vim.lsp.config(server_name, {
        capabilities = capabilities,
        cmd = servers[server_name].cmd,
        root_markers = servers[server_name].root_markers,
        settings = {
          servers[server_name].settings,
        },
      })

      vim.lsp.enable(server_name)
      if server_name ~= 'vtsls' and servers[server_name].settings ~= nil then
      else
        vim.lsp.config(server_name, {
          cmd = servers[server_name].cmd,
          filetypes = servers[server_name].filetypes,
          root_markers = servers[server_name].root_markers,
          init_options = servers[server_name].init_options,
        })
        vim.lsp.enable(server_name)
      end
    end
  end,
}
