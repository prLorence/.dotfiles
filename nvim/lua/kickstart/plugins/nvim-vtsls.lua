return {
  'yioneko/nvim-vtsls',
  config = function()
    require('vtsls').config {
      -- customize handlers for commands
      -- automatically trigger renaming of extracted symbol
      refactor_auto_rename = true,
    }
  end,
}
