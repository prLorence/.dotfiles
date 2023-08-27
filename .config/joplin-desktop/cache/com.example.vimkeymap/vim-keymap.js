function plugin(CodeMirror) {
    CodeMirror.Vim.map('J', '10j-zz', 'normal')
    CodeMirror.Vim.map('K', '10k-zz', 'normal')

    CodeMirror.Vim.map('kj', '<Esc>', 'insert')

    CodeMirror.Vim.map('Ctrl-d', 'Ctrl-d-zz', 'normal')
    CodeMirror.Vim.map('Ctrl-u', 'Ctrl-u-zz', 'normal')

    CodeMirror.Vim.map('Ctrl-p', 'Ctrl-p', 'normal')
};

module.exports = {
    default: function(CodeMirror) {
        return {
            plugin: plugin,
            codeMirrorResources: [''],
            codeMirrorOptions: {},
            assets: function() {
                return [
                ];
            },
        }
    },
}
