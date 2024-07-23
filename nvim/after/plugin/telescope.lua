require('telescope').load_extension('fzf')
local builtin = require('telescope.builtin')
local actions = require("telescope.actions")

require('telescope').setup({
    defaults = {
        sorting_strategy = "ascending",
        layout_strategy = "horizontal",
        layout_config = { prompt_position = "top" },
    },
})

local opts = { noremap = true, silent = true }

vim.keymap.set('n', '<C-p>', builtin.find_files, opts)
vim.keymap.set('n', '<leader>?', builtin.oldfiles, opts)
vim.keymap.set('n', '<leader>lg', builtin.live_grep, opts)
vim.keymap.set('n', '<leader>vd', builtin.diagnostics, opts)
vim.keymap.set("n", "<leader>fs", builtin.treesitter, opts) -- Lists tree-sitter symbols
vim.keymap.set('n', '<leader>ps', function()
    builtin.grep_string({ search = vim.fn.input("Grep > ") })
end)

-- Git keymaps
vim.keymap.set('n', '<leader>vc', builtin.git_commits, opts)
vim.keymap.set('n', '<leader>gs', builtin.git_status, opts)
