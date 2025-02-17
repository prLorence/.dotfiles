vim.g.mapleader = ' '
vim.g.maplocalleader = ' '

-- vim.keymap.set('i', 'kj', '<Esc>:w<CR>')

vim.keymap.set('v', '<M-S-j>', ":m '>+1<CR>gv=gv", { silent = true })
vim.keymap.set('v', '<M-S-k>', ":m '<-2<CR>gv=gv", { silent = true })

vim.keymap.set('n', '<C-d>', '<C-d>zz')
vim.keymap.set('n', '<C-u>', '<C-u>zz')
vim.keymap.set('n', 'n', 'nzzzv')
vim.keymap.set('n', 'N', 'Nzzzv')

-- greatest remap ever
vim.keymap.set('x', '<leader>p', [["_dP]])

-- next greatest remap ever : asbjornHaland
vim.keymap.set({ 'n', 'v' }, '<leader>y', [["+y]])
vim.keymap.set('n', '<leader>Y', [["+Y]])

vim.keymap.set({ 'n', 'v' }, '<leader>d', [["_d]])

vim.keymap.set('n', 'Q', '<nop>')

vim.keymap.set('n', '<C-k>', '<cmd>cnext<CR>zz')
vim.keymap.set('n', '<C-j>', '<cmd>cprev<CR>zz')
vim.keymap.set('n', '<leader>k', '<cmd>lnext<CR>zz')
vim.keymap.set('n', '<leader>j', '<cmd>lprev<CR>zz')

vim.keymap.set('n', '<M-h>', '<C-w>h', {})
vim.keymap.set('n', '<M-l>', '<C-w>l', {})
vim.keymap.set('n', '<M-j>', '<C-w>j', {})
vim.keymap.set('n', '<M-k>', '<C-w>k', {})

vim.keymap.set('n', '<M-=>', ':vertical resize +20<CR>', { silent = true })
vim.keymap.set('n', '<M-->', ':vertical resize -20<CR>', { silent = true })
vim.keymap.set('n', '<C-M-=>', ':resize -20<CR>', { silent = true })
vim.keymap.set('n', '<C-M-->', ':resize +20<CR>', { silent = true })

-- Clear highlights on search when pressing <Esc> in normal mode
--  See `:help hlsearch`
vim.keymap.set('n', '<Esc>', '<cmd>nohlsearch<CR>')

-- Diagnostic keymaps
vim.keymap.set('n', '<leader>q', vim.diagnostic.setloclist, { desc = 'Open diagnostic [Q]uickfix list' })

vim.keymap.set('n', '<leader><leader>', function()
  vim.cmd 'edit'
end)

-- Highlight when yanking (copying) text
--  Try it with `yap` in normal mode
--  See `:help vim.highlight.on_yank()`
vim.api.nvim_create_autocmd('TextYankPost', {
  desc = 'Highlight when yanking (copying) text',
  group = vim.api.nvim_create_augroup('kickstart-highlight-yank', { clear = true }),
  callback = function()
    vim.highlight.on_yank()
  end,
})

-- NOTE: This won't work in all terminal emulators/tmux/etc. Try your own mapping
-- or just use <C-\><C-n> to exit terminal mode
vim.keymap.set('t', '<Esc><Esc>', '<C-\\><C-n>', { desc = 'Exit terminal mode' })

-- TIP: Disable arrow keys in normal mode
vim.keymap.set('n', '<left>', '<cmd>echo "Use h to move!!"<CR>')
vim.keymap.set('n', '<right>', '<cmd>echo "Use l to move!!"<CR>')
vim.keymap.set('n', '<up>', '<cmd>echo "Use k to move!!"<CR>')
vim.keymap.set('n', '<down>', '<cmd>echo "Use j to move!!"<CR>')

vim.keymap.set('n', '<A-|>', function()
  vim.cmd 'vertical Git'
end, { silent = true, noremap = true })

vim.keymap.set('n', '<leader>gc', function()
  vim.cmd 'Git commit'
end)

-- vim.keymap.set('n', '-', '<CMD>Oil --float<CR>', { desc = 'Open parent directory' })
vim.keymap.set('n', '-', '<CMD>lua MiniFiles.open()<CR>', { desc = 'Open parent directory' })
