require("noice").setup({
    -- add any options here
    lsp = {
        signature = {
            enabled = false
        },
        hover = {
            enabled = false
        }
    },
    routes = {
        {
            filter = {
                event = 'msg_show',
                any = {
                    { find = '%d+L, %d+B' },
                    { find = '; after #%d+' },
                    { find = '; before #%d+' },
                    { find = '%d fewer lines' },
                    { find = '%d more lines' },
                },
            },
            opts = { skip = true },
        }
    },
})
