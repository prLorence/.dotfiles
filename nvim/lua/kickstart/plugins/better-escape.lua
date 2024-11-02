return {
  'max397574/better-escape.nvim',
  config = function()
    require('better_escape').setup {
      timeout = vim.o.timeoutlen,
      default_mappings = false,
      mappings = {
        i = {
          k = {
            -- These can all also be functions
            j = '<Esc>:w<CR>',
          },
        },
        c = {
          k = {
            j = '<Esc>',
          },
        },
        t = {
          k = {
            j = '<C-\\><C-n>',
          },
        },
        v = {
          k = {
            j = '<Esc>',
          },
        },
        s = {
          k = {
            j = '<Esc>',
          },
        },
      },
    }
  end,
}
