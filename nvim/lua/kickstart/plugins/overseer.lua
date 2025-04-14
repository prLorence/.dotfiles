return {
  'stevearc/overseer.nvim',
  config = function()
    require('overseer').setup()

    vim.keymap.set('n', '<leader>or', ':OverseerRun<CR>', { desc = '[O]verseer [R]un' })
    vim.keymap.set('n', '<leader>ot', ':OverseerToggle left<CR>', { desc = '[O]verseer [T]oggle' })
  end,
  opts = {},
}
