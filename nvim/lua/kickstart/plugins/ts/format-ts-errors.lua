return {
  'davidosomething/format-ts-errors.nvim',
  config = function()
    require('format-ts-errors').setup {
      start_indent_level = 0, -- initial indent
    }
  end,
}
