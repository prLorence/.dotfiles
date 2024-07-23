require 'nvim-treesitter.configs'.setup {
    -- A list of parser names, or "all" (the five listed parsers should always be installed)
    ensure_installed = {
        "c_sharp",
        "lua",
        "vim",
        "javascript",
        "typescript",
        "python",
        "terraform",
        "go",
        "sql",
        "html",
        "css",
        "regex",
        "bash",
        "markdown",
        "markdown_inline"
    },

    -- Install parsers synchronously (only applied to `ensure_installed`)
    sync_install = true,

    -- Automatically install missing parsers when entering buffer
    -- Recommendation: set to false if you don't have `tree-sitter` CLI installed locally
    auto_install = true,
    highlight = { enable = true, },
    indent = { enable = true },
    textobjects = {
        select = {
            enable = true,

            -- Automatically jump forward to textobj, similar to targets.vim
            lookahead = true,

            keymaps = {
                -- You can use the capture groups defined in textobjects.scm
                ["af"] = "@function.outer",
                ["if"] = "@function.inner",
                ["ac"] = "@class.outer",
                ["ic"] = "@class.inner",
            },
        },
    },

    autotag = {
        enable = true,
    }
}


-- https://github.com/windwp/nvim-ts-autotag/issues/19
vim.lsp.handlers['textDocument/publishDiagnostics'] = vim.lsp.with(
    vim.lsp.diagnostic.on_publish_diagnostics,
    {
        underline = {
            min = 'Warning'
        },
        virtual_text = {
            spacing = 5,
            min = 'Error'
        },
        update_in_insert = false,
    }
)

vim.api.nvim_create_augroup('diagnostics', { clear = true })

vim.api.nvim_create_autocmd('DiagnosticChanged', {
    group = 'diagnostics',
    callback = function()
        vim.diagnostic.setloclist({ open = false })
    end,
})
