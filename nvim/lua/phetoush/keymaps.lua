local map = function(keys, func, desc)
  vim.keymap.set('n', keys, func, { desc = desc })
end

map('<leader>smt', function()
  vim.cmd 'SupermavenToggle'
end, '[S]uper[m]aven[T]oggle')

-- terraform commands
map('<leader>ti', function()
  vim.cmd 'vsplit'
  vim.cmd 'terminal terraform init'
end, '[T]erraform [i]nit')

map('<leader>tv', function()
  vim.cmd 'vsplit'
  vim.cmd 'terminal terraform validate'
end, '[T]erraform [v]alidate')

map('<leader>tp', function()
  vim.cmd 'vsplit'
  vim.cmd 'terminal terraform plan'
end, '[T]erraform [p]lan')
