vim.api.nvim_create_autocmd({ 'VimEnter', 'BufReadPost' }, {
  pattern = { '*.md', '*.txt' },
  callback = function()
    vim.cmd 'NoNeckPain'

    vim.wo.wrap = true
    vim.wo.linebreak = true
  end,
})

vim.api.nvim_create_autocmd('BufWinLeave', {
  pattern = { '*.md', '*.txt' },
  callback = function()
    vim.wo.wrap = false
    vim.wo.linebreak = false
  end,
})
