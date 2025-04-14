-- return { -- Autoformat
--   'stevearc/conform.nvim',
--   event = { 'BufWritePre' },
--   cmd = { 'ConformInfo' },
--   keys = {
--     {
--       '<leader>fb',
--       function()
--         require('conform').format { async = true, lsp_fallback = true }
--       end,
--       mode = '',
--       desc = '[F]ormat [B]uffer',
--     },
--   },
--   opts = {
--     async = true,
--     lsp_fallback = true,
--     notify_on_error = false,
--     default_format_opts = {
--       lsp_format = 'fallback',
--     },
--     format_on_save = function(bufnr)
--       -- Disable autoformat on certain filetypes
--       local ignore_filetypes = { 'sql', 'java', 'c', 'cpp', 'go' }
--       if vim.tbl_contains(ignore_filetypes, vim.bo[bufnr].filetype) then
--         return
--       end
--       -- Disable with a global or buffer-local variable
--       if vim.g.disable_autoformat or vim.b[bufnr].disable_autoformat then
--         return
--       end
--       -- Disable autoformat for files in a certain path
--       local bufname = vim.api.nvim_buf_get_name(bufnr)
--       if bufname:match '/node_modules/' then
--         return
--       end
--       -- ...additional logic...
--       return { timeout_ms = 1000, lsp_format = 'fallback' }
--     end,
--     formatters_by_ft = {
--       lua = { 'stylua' },
--       tf = { 'tflint' },
--       svelte = { 'prettierd', stop_after_first = true },
--       javascript = { 'prettierd', stop_after_first = true },
--       typescript = { 'prettierd', stop_after_first = true },
--       javascriptreact = { 'prettierd', stop_after_first = true },
--       typescriptreact = { 'prettierd', stop_after_first = true },
--       json = { 'jq', stop_after_first = true },
--       proto = { 'buf' },
--       -- Conform can also run multiple formatters sequentially
--       -- python = { "isort", "black" },
--       --
--       -- You can use 'stop_after_first' to run the first available formatter from the list
--     },
--   },
-- }
return {
  'stevearc/conform.nvim',
  event = { 'BufReadPre', 'BufNewFile' },
  config = function()
    local conform = require 'conform'

    conform.setup {
      formatters_by_ft = {
        javascript = { 'prettier' },
        typescript = { 'prettier' },
        javascriptreact = { 'prettier' },
        typescriptreact = { 'prettier' },
        svelte = { 'prettier' },
        css = { 'prettier' },
        html = { 'prettier' },
        json = { 'prettier' },
        yaml = { 'prettier' },
        markdown = { 'prettier' },
        graphql = { 'prettier' },
        liquid = { 'prettier' },
        lua = { 'stylua' },
        tf = { 'tflint' },
      },
      format_after_save = {
        lsp_fallback = true,
        async = true,
        timeout_ms = 1000,
      },
    }

    vim.keymap.set({ 'n', 'v' }, '<leader>mp', function()
      conform.format {
        lsp_fallback = true,
        async = false,
        timeout_ms = 1000,
      }
    end, { desc = 'Format file or range (in visual mode)' })
  end,
}
