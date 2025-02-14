return { -- Autoformat
  'stevearc/conform.nvim',
  event = { 'LspAttach', 'BufReadPost', 'BufNewFile' },
  cmd = { 'ConformInfo' },
  keys = {
    {
      '<leader>f',
      function()
        require('conform').format { async = true, lsp_fallback = true }
      end,
      mode = '',
      desc = '[F]ormat buffer',
    },
  },
  opts = {
    async = true,
    lsp_fallback = true,
    notify_on_error = false,
    format_on_save = function(bufnr)
      -- Disable "format_on_save lsp_fallback" for languages that don't
      -- have a well standardized coding style. You can add additional
      -- languages here or re-enable it for the disabled ones.
      local disable_filetypes = { c = true, cpp = true, go = true }
      return {
        timeout_ms = 500,
        -- lsp_fallback = not disable_filetypes[vim.bo[bufnr].filetype],
        lsp_fallback = true,
      }
    end,
    formatters_by_ft = {
      lua = { 'stylua' },
      tf = { 'tflint' },
      svelte = { 'eslint_d', 'prettierd', stop_after_first = true },
      javascript = { 'eslint_d', 'prettierd', stop_after_first = true },
      typescript = { 'eslint_d', 'prettierd', stop_after_first = true },
      javascriptreact = { 'eslint_d', 'prettierd', stop_after_first = true },
      typescriptreact = { 'eslint_d', 'prettierd', stop_after_first = true },
      json = { 'eslint_d', 'prettierd', stop_after_first = true },
      proto = { 'buf' },
      -- Conform can also run multiple formatters sequentially
      -- python = { "isort", "black" },
      --
      -- You can use 'stop_after_first' to run the first available formatter from the list
    },
  },
}
