local jsts_settings = {
  suggest = {
    includeCompletionsForImportStatements = false,
    completeFunctionCalls = true,
  },
  updateImportsOnFileMove = {
    enabled = 'always',
  },
  format = {
    enable = true,
    semicolons = 'insert',
    insertSpaceAfterOpeningAndBeforeClosingEmptyBraces = false,
    insertSpaceAfterOpeningAndBeforeClosingNonemptyBraces = false,
  },
  inlayHints = {
    -- parameterNames = { enabled = 'literals' },
    -- parameterTypes = { enabled = true },
    -- variableTypes = { enabled = true },
    -- propertyDeclarationTypes = { enabled = true },
    -- functionLikeReturnTypes = { enabled = true },
    -- enumMemberValues = { enabled = true },
    functionLikeReturnTypes = { enabled = true },
    parameterNames = { enabled = 'literals' },
    variableTypes = { enabled = true },
  },
  implementationsCodeLens = {
    enabled = false,
    showOnInterfaceMethods = true,
  },
  referenceCodeLens = {
    enabled = false,
    showOnAllFunctions = false,
  },
  tsserver = {
    nodePath = '/home/phetoush/.bun/bin/bun',
    experimental = {
      enableProjectDiagnostics = true,
    },
    useSyntaxServer = 'auto',
    maxTsServerMemory = 4096,
    preferences = {
      importModuleSpecifier = os.getenv 'LSP_TS_IMPORT_MODULE_SPECIFIER_PROJECT_RELATIVE' and 'project-relative' or 'auto',
      includePackageJsonAutoImports = 'auto',
    },
    workspaceSymbols = {
      scope = 'currentProject',
    },
  },
}

return {
  cmd = { '/home/phetoush/.bun/bin/bun', '/home/phetoush/.local/share/nvim/mason/bin/vtsls', '--stdio' },
  root_markers = { 'tsconfig.json', 'jsonconfig.json' },
  handlers = {
    ['textDocument/publishDiagnostics'] = function(_, result, ctx)
      if result.diagnostics == nil then
        return
      end

      -- ignore some tsserver diagnostics
      local idx = 1
      while idx <= #result.diagnostics do
        local entry = result.diagnostics[idx]

        local formatter = require('format-ts-errors')[entry.code]
        entry.message = formatter and formatter(entry.message) or entry.message

        -- codes: https://github.com/microsoft/TypeScript/blob/main/src/compiler/diagnosticMessages.json
        if entry.code == 80001 then
          -- { message = "File is a CommonJS module; it may be converted to an ES module.", }
          table.remove(result.diagnostics, idx)
        else
          idx = idx + 1
        end
      end

      vim.lsp.diagnostic.on_publish_diagnostics(_, result, ctx)
    end,
  },
  typescript = jsts_settings,
  javascript = jsts_settings,
  vtsls = {
    enableMoveToFileCodeAction = true,
    autoUseWorkspaceTsdk = true,
    experimental = {
      maxInlayHintLength = 30,
      completion = {
        enableServerSideFuzzyMatch = true,
        -- entriesLimit = 20,
      },
    },
  },
  -- settings = {
  -- },
}
