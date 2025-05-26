return {
  enabled = true,
  'pmizio/typescript-tools.nvim',
  dependencies = {
    'nvim-lua/plenary.nvim',
    'neovim/nvim-lspconfig',
  },
  event = 'BufEnter',
  cond = not vim.g.vscode,
  config = function(_, opts)
    opts.handlers = {
      ['textDocument/publishDiagnostics'] = function(_, result, ctx, _)
        if not result.diagnostics then
          return
        end
        local idx = 1
        while idx <= #result.diagnostics do
          local entry = result.diagnostics[idx]
          local formatter = require('format-ts-errors')[entry.code]
          entry.message = formatter and formatter(entry.message) or entry.message
          if entry.code == 80001 then
            table.remove(result.diagnostics, idx) -- Remove specific TS diagnostics
          else
            idx = idx + 1
          end
        end
        vim.lsp.diagnostic.on_publish_diagnostics(_, result, ctx)
      end,
    }

    require('typescript-tools').setup(opts)
  end,
  opts = {
    filetypes = {
      'javascript',
      'javascriptreact',
      'javascript.jsx',
      'typescript',
      'typescriptreact',
      'typescript.tsx',
    },
    settings = {
      expose_as_code_action = 'all',
      tsserver_max_memory = 'auto',
      tsserver_file_preferences = {
        importModuleSpecifierPreference = 'non-relative',
        includeInlayParameterNameHints = 'literals',
        includeCompletionsForModuleExports = true,
        quotePreference = 'auto',
        includeCompletionsForImportStatements = true,
        includeCompletionsWithClassMemberSnippets = true,
        includePackageJsonAutoImports = 'auto',
        importModuleSpecifierEnding = 'auto',
        displayPartsForJSDoc = true,
        generateReturnInDocTemplate = true,
        providePrefixAndSuffixTextForRename = false,
        allowRenameOfImportPath = true,
      },
      tsserver_format_options = {
        allowIncompleteCompletions = false,
        trimTrailingWhitespace = true,
      },
      separate_diagnostic_server = true,
      publish_diagnostic_on = 'insert_leave',
      complete_function_calls = true,
    },
  },
}
