return {
  'ray-x/go.nvim',
  dependencies = { -- optional packages
    'ray-x/guihua.lua',
    'neovim/nvim-lspconfig',
    'nvim-treesitter/nvim-treesitter',
  },
  config = function()
    -- Run gofmt + goimports on save
    require('go').setup {
      -- verbose = plugin_debug(), -- enable for debug
      fillstruct = 'gopls',
      log_path = vim.fn.expand '$HOME' .. '/tmp/gonvim.log',
      lsp_codelens = false, -- use navigator
      lsp_gofumpt = true,
      dap_debug = true,
      gofmt = 'gopls',
      goimports = 'gopls',
      dap_debug_vt = true,
      dap_debug_gui = true,
      diagnostic = {
        signs = {
          text = { '🚑', '🔧', '🪛', '🪠' },
        },
        update_in_insert = false,
      },
      -- diagnostic = { -- set diagnostic to false to disable vim.diagnostic setup
      --   hdlr = true, -- hook lsp diag handler and send diag to quickfix
      --   underline = true,
      --   -- virtual text setup
      --   virtual_text = { spacing = 0, prefix = '■' },
      --   signs = true,
      --   update_in_insert = true,
      -- },
      test_runner = 'go', -- go test, dlv, ginkgo
      -- run_in_floaterm = true, -- set to true to run in float window.
      lsp_document_formatting = true,
      preludes = {
        default = function()
          return { 'AWS_PROFILE=test' }
        end,
        GoRun = function()
          local pwd = vim.fn.getcwd()
          local cmdl = { 'watchexec', '--restart', '-v', '-e', 'go' }
          -- if current folder contains sub folder with name pattern .\w+-env
          -- list all subfolders see if match .\w+-env
          local hasenv = false
          for _, v in ipairs(vim.fn.readdir(pwd)) do
            if string.match(v, '%p%a+%p*env') then
              hasenv = true
              break
            end
          end

          if hasenv then
            local cwdl = vim.split(pwd, '/')
            local cwd = cwdl[#cwdl]
            local cwdp = vim.split(cwd, '-')
            local cwdps = cwdp[#cwdp]
            return vim.list_extend(cmdl, { 'awsenv', cwdps })
          end
          return {}
        end,
      },
      luasnip = true,
      -- lsp_on_attach = require("navigator.lspclient.attach").on_attach,
      -- lsp_cfg = true,
      -- test_efm = true, -- errorfomat for quickfix, default mix mode, set to true will be efm only
    }

    local format_sync_grp = vim.api.nvim_create_augroup('goimports', {})
    vim.api.nvim_create_autocmd('BufWritePre', {
      pattern = '*.go',
      callback = function()
        require('go.format').goimports()
      end,
      group = format_sync_grp,
    })
  end,
  event = { 'CmdlineEnter' },
  ft = { 'go', 'gomod' },
  build = ':lua require("go.install").update_all_sync()', -- if you need to install/update all binaries
}
