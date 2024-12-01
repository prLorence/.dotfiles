return {
  'luukvbaal/statuscol.nvim',
  config = function()
    -- from https://github.com/kevinhwang91/nvim-ufo/issues/4#issuecomment-1512772530
    local builtin = require 'statuscol.builtin'
    require('statuscol').setup {
      relculright = true,
      segments = {
        { text = { builtin.foldfunc }, click = 'v:lua.ScFa' },
        { text = { '%s' }, click = 'v:lua.ScSa' },
        { text = { builtin.lnumfunc, ' ' }, click = 'v:lua.ScLa' },
      },
    }
  end,
}
