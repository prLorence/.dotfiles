return {
  'supermaven-inc/supermaven-nvim',
  event = 'VeryLazy',
  config = function()
    require('supermaven-nvim').setup {
      keymaps = {
        accept_suggestion = '<M-[>',
        clear_suggestion = '<M-]>',
        accept_word = '<C-j>',
      },
      disable_keymaps = false, -- disables built in keymaps for more manual control
    }
  end,
}
