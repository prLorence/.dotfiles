return {
  enabled = true,
  'pmizio/typescript-tools.nvim',
  dependencies = {
    'nvim-lua/plenary.nvim',
    'neovim/nvim-lspconfig',
  },
  event = 'BufEnter',
  cond = not vim.g.vscode,
  opts = {
    filetypes = {
      'javascript',
      'javascriptreact',
      'javascript.jsx',
      'typescript',
      'typescriptreact',
      'typescript.tsx',
      'vue',
    },
    settings = {
      tskerver_max_memory = 'auto',
      tsserver_file_preferences = {
        importModuleSpecifierPreference = 'non-relative',
        includeInlayParameterNameHints = 'all',
        includeCompletionsForModuleExports = true,
        quotePreference = 'auto',
        includeCompletionsForImportStatements = true,
        includeCompletionsWithClassMemberSnippets = true,
        importModuleSpecifierEnding = 'auto',
        providePrefixAndSuffixTextForRename = false,
      },
      tsserver_format_options = {
        allowIncompleteCompletions = false,
        allowRenameOfImportPath = true,
      },
      separate_diagnostic_server = true,
      publish_diagnostic_on = 'insert_leave',
    },
  },
}
