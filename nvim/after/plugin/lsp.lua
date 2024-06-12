local lsp = require('lsp-zero').preset({
    name = "recommended",
    float_border = 'rounded',
    manage_nvim_cmp = { set_sources = 'recommended' },
    suggest_lsp_servers = true,
});

lsp.nvim_workspace();

lsp.ensure_installed({
    'tsserver',
    'omnisharp',
    'terraformls'
})

local capabilities = require('cmp_nvim_lsp').default_capabilities()

lsp.on_attach(function(client, bufnr)
    local opts = { buffer = bufnr, remap = false }

    --- Guard against servers without the signatureHelper capability
    if client.server_capabilities.signatureHelpProvider then
        require('lsp-overloads').setup(client, {
            keymaps = {
                next_signature = "<A-n>",
                previous_signature = "<A-p>",
                next_parameter = "<A-l>",
                previous_parameter = "<A-h>",
                close_signature = "<A-s>"
            },
        })
    end

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
local lspconfig = require("lspconfig")
lspconfig.lua_ls.setup(lsp.nvim_lua_ls())

-- CSHARP CONFIG
local util = require 'lspconfig.util'

lspconfig.omnisharp.setup({
    handlers = {
        ["textDocument/definition"] = require('omnisharp_extended').handler,
    },
    cmd = { 'dotnet', '/home/phetoush/.local/share/nvim/mason/packages/omnisharp/libexec/OmniSharp.dll' },
    filetypes = { 'cs', 'vb' },

    root_dir = function(fname)
        return util.root_pattern '*.sln' (fname) or util.root_pattern '*.csproj' (fname)
    end,

    capabilities = capabilities,

    -- omnisharp features config
    enable_editorconfig_support = true,
    enable_ms_build_load_projects_on_demand = true,
    enable_roslyn_analyzers = true,
    enable_inlay_hints = true,
    organize_imports_on_format = true,
    enable_import_completion = false,
    sdk_include_prereleases = true,
    analyze_open_documents_only = false,

    on_attach = function(client, bufnr)
        client.server_capabilities.document_formatting = true
        client.server_capabilities.document_range_formatting = true

        lsp.on_attach(client, bufnr)
    end,
})


lsp.setup()

-- format on save
vim.api.nvim_create_autocmd("BufWritePre", {
    callback = function()
        vim.lsp.buf.format { async = true }
    end
})
