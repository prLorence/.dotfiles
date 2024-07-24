vim.keymap.set("n", "<A-|>", function ()
    vim.cmd("vertical Git")
end, {silent = true, noremap = true})

vim.keymap.set("n", "<leader>gc", function()
    vim.cmd("Git commit")
end)
