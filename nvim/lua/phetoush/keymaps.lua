local map = function(keys, func, desc)
  vim.keymap.set('n', keys, func, { desc = desc })
end

map('<leader>smt', function()
  vim.cmd 'SupermavenToggle'
end, '[S]uper[m]aven[T]oggle')
