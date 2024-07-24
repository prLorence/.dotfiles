local lsp = require('lsp-zero').preset({
    name = "recommended",
    float_border = 'rounded',
    manage_nvim_cmp = { set_sources = 'recommended' },
    suggest_lsp_servers = true,
});

lsp.nvim_workspace()

lsp.ensure_installed({
    'tsserver',
    "pyright",
    'terraformls',
    'gopls',
    "tflint",
})

local capabilities = vim.lsp.protocol.make_client_capabilities()
capabilities = require('cmp_nvim_lsp').default_capabilities(capabilities)


lsp.on_attach(function(client, bufnr)
    local opts = { buffer = bufnr, remap = false }
    capabilities = capabilities

    -- lsp keybindings
    vim.keymap.set("n", "<leader>ca", vim.lsp.buf.code_action, opts)
    vim.keymap.set("n", "gD", vim.lsp.buf.declaration, opts)
    vim.keymap.set("n", "gd", vim.lsp.buf.definition, opts)
    vim.keymap.set('n', '<leader>td', vim.lsp.buf.type_definition, opts)
    vim.keymap.set("n", "E", vim.diagnostic.open_float, opts)
    vim.keymap.set({ "i", "n" }, "<A-s>", function() vim.cmd("LspOverloadsSignature") end,
        { noremap = true, silent = true })
    vim.keymap.set("n", "<leader>rn", vim.lsp.buf.rename, opts)

    vim.keymap.set('n', 'gr', require('telescope.builtin').lsp_references, opts)
    vim.keymap.set('n', 'gi', require('telescope.builtin').lsp_implementations, opts)
    vim.keymap.set('n', '<leader>ds', require('telescope.builtin').lsp_document_symbols, opts)
end)


-- (Optional) Configure lua language server for neovim
-- require("lsp_signature").setup()
local lspconfig = require("lspconfig")
lspconfig.lua_ls.setup(lsp.nvim_lua_ls())

-- TF SETUP
lspconfig.terraformls.setup({
    capabilities = capabilities
})

lspconfig.tflint.setup({
    capabilities = capabilities
})

-- GOLANG SETUP
require('go').setup()

local format_sync_grp = vim.api.nvim_create_augroup("goimports", {})

vim.api.nvim_create_autocmd("BufWritePre", {
    pattern = "*.go",
    callback = function()
        require('go.format').goimports()
    end,
    group = format_sync_grp,
})

lsp.setup()
