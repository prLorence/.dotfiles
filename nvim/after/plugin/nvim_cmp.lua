local cmp_autopairs = require('nvim-autopairs.completion.cmp')
local cmp = require('cmp')
local cmp_select = { behavior = cmp.SelectBehavior.Select }

local has_words_before = function()
    unpack = unpack or table.unpack
    local line, col = unpack(vim.api.nvim_win_get_cursor(0))
    return col ~= 0 and vim.api.nvim_buf_get_lines(0, line - 1, line, true)[1]:sub(col, col):match("%s") == nil
end

local luasnip = require('luasnip')

require("luasnip.loaders.from_vscode").lazy_load({ exclude = { "cs" } });

require("luasnip.loaders.from_vscode").lazy_load({ paths = { "./snippets-extension" } })

cmp.setup({
    snippet = {
        expand = function(args)
            luasnip.lsp_expand(args.body)
        end,
    },
    sources = {
        { name = 'nvim_lsp', max_item_count = 20 },
        { name = 'luasnip', },
        { name = 'buffer',   keyword_length = 2 },
        { name = 'path' },
        -- { name = 'nvim_lsp_signature_help' }
    },
    mapping = cmp.mapping.preset.insert({
        ['<C-b>'] = cmp.mapping.scroll_docs(-4),
        ['<C-f>'] = cmp.mapping.scroll_docs(4),
        ['<C-Space>'] = cmp.mapping.complete(),
        ['<C-e>'] = cmp.mapping.abort(),
        ['<Tab>'] = cmp.mapping.confirm({ select = false }),
        ["<M-j>"] = cmp.mapping(function(fallback)
            if cmp.visible() then
                cmp.select_next_item(cmp_select)
                -- You could replace the expand_or_jumpable() calls with expand_or_locally_jumpable()
                -- they way you will only jump inside the snippet region
            elseif luasnip.expand_or_jumpable() then
                luasnip.expand_or_jump()
            elseif has_words_before() then
                cmp.complete()
            else
                fallback()
            end
        end, { "i", "s" }),

        ["<M-k>"] = cmp.mapping(function(fallback)
            if cmp.visible() then
                cmp.select_prev_item(cmp_select)
            elseif luasnip.jumpable(-1) then
                luasnip.jump(-1)
            else
                fallback()
            end
        end, { "i", "s" }),
    }),
    window = {
        completion = cmp.config.window.bordered("Normal:NormalFloat,FloatBorder:FloatBorder,CursorLine:PmenuSel"),
        documentation = cmp.config.window.bordered("Normal:NormalFloat,FloatBorder:FloatBorder,CursorLine:PmenuSel")
    }
})

vim.cmd [[
    set completeopt=menuone,noinsert,noselect
    highlight! default link CmpItemKind CmpItemMenuDefault
]]

cmp.event:on(
    'confirm_done',
    cmp_autopairs.on_confirm_done()
)
