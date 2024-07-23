local lsp = require('lsp-zero').preset({
    name = "recommended",
    float_border = 'rounded',
    manage_nvim_cmp = { set_sources = 'recommended' },
    suggest_lsp_servers = true,
});

lsp.nvim_workspace();

lsp.ensure_installed({
    'tsserver',
    "pyright",
    'terraformls',
    'gopls'
})

local capabilities = vim.lsp.protocol.make_client_capabilities()
capabilities = require('cmp_nvim_lsp').default_capabilities(capabilities)

lsp.on_attach(function(client, bufnr)
    local opts = { buffer = bufnr, remap = false }

    --- Guard against servers without the signatureHelper capability
    -- if client.server_capabilities.signatureHelpProvider then
    --     require('lsp-overloads').setup(client, {
    --         keymaps = {
    --             next_signature = "<A-n>",
    --             previous_signature = "<A-p>",
    --             next_parameter = "<A-l>",
    --             previous_parameter = "<A-h>",
    --             close_signature = "<A-s>"
    --         },
    --     })
    -- end

    -- lsp keybindings
    vim.keymap.set("n", "<leader>ca", vim.lsp.buf.code_action, opts)
    vim.keymap.set('n', '<leader>td', vim.lsp.buf.type_definition, opts)
    vim.keymap.set("n", "<leader>rn", vim.lsp.buf.rename, opts)

    vim.keymap.set("n", "gD", vim.lsp.buf.declaration, opts)
    vim.keymap.set("n", "gd", vim.lsp.buf.definition, opts)
    vim.keymap.set("n", "E", vim.diagnostic.open_float, opts)
    -- vim.keymap.set({ "i", "n" }, "<M-s>", function() vim.cmd("LspOverloadsSignature") end,
    --     { noremap = true, silent = true })

    vim.keymap.set('n', 'gr', require('telescope.builtin').lsp_references, opts)
    vim.keymap.set('n', 'gi', require('telescope.builtin').lsp_implementations, opts)
    vim.keymap.set('n', '<leader>ds', require('telescope.builtin').lsp_document_symbols, opts)
end)


-- (Optional) Configure lua language server for neovim
-- require("lsp_signature").setup()
local lspconfig = require("lspconfig")
lspconfig.lua_ls.setup(lsp.nvim_lua_ls())

-- GOLANG SETUP
require('go').setup()
-- local cfg = require 'go.lsp'.config() -- config() return the go.nvim gopls setup
--
-- lspconfig.gopls.setup()
