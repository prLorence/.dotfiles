vim.g.mapleader = " "

vim.keymap.set('i', 'kj', '<Esc>:w<CR>')

vim.keymap.set("v", "<M-S-j>", ":m '>+1<CR>gv=gv")
vim.keymap.set("v", "<M-S-k>", ":m '<-2<CR>gv=gv")

vim.keymap.set("n", "<C-d>", "<C-d>zz")
vim.keymap.set("n", "<C-u>", "<C-u>zz")
vim.keymap.set("n", "n", "nzzzv")
vim.keymap.set("n", "N", "Nzzzv")

-- greatest remap ever
vim.keymap.set("x", "<leader>p", [["_dP]])

-- next greatest remap ever : asbjornHaland
vim.keymap.set({ "n", "v" }, "<leader>y", [["+y]])
vim.keymap.set("n", "<leader>Y", [["+Y]])

vim.keymap.set({ "n", "v" }, "<leader>d", [["_d]])

vim.keymap.set("n", "Q", "<nop>")

vim.keymap.set("n", "<C-k>", "<cmd>cnext<CR>zz")
vim.keymap.set("n", "<C-j>", "<cmd>cprev<CR>zz")
vim.keymap.set("n", "<leader>k", "<cmd>lnext<CR>zz")
vim.keymap.set("n", "<leader>j", "<cmd>lprev<CR>zz")

vim.keymap.set("n", "<M-S-h>", "<C-w>h", {})
vim.keymap.set("n", "<M-S-l>", "<C-w>l", {})
vim.keymap.set("n", "<M-S-j>", "<C-w>j", {})
vim.keymap.set("n", "<M-S-k>", "<C-w>k", {})

vim.keymap.set("n", "<leader>gc", function()
    vim.cmd("Git commit")
end)

vim.keymap.set("n", "<leader><leader>", function()
    vim.cmd("edit")
end)

vim.keymap.set("n", "<leader>rp", function()
    local class_name = vim.fn.input("Reverse nuget package search: ")
    local res = assert(io.popen("reverse-nuget -c " .. class_name .. "| head -n 10"))
    local output = res:read('*all')
    res:close()
    print(output)
end)

