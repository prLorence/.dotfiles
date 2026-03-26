local M = {}

function M.lsp_client_settings()
  local go_settings = {
    gopls = {
      gofumpt = true,
      codelenses = {
        gc_details = false,
        generate = true,
        regenerate_cgo = true,
        run_govulncheck = true,
        test = true,
        tidy = true,
        upgrade_dependency = true,
        vendor = true,
      },
      hints = {
        assignVariableTypes = true,
        compositeLiteralFields = true,
        compositeLiteralTypes = true,
        constantValues = true,
        functionTypeParameters = true,
        parameterNames = true,
        rangeVariableTypes = true,
      },
      analyses = {
        fieldalignment = true,
        nilness = true,
        unusedparams = true,
        unusedwrite = true,
        useany = true,
      },
      usePlaceholders = true,
      completeUnimported = true,
      staticcheck = true,
      directoryFilters = { '-.git', '-.vscode', '-.idea', '-.vscode-test', '-node_modules', '-.nvim' },
      semanticTokens = true,
    },
  }

  local tf_settings = {
    indexing = {
      ignoreDirectoryNames = { '.git', 'terraform.tfstate.d' },
    },
    experimentalFeatures = {
      validateOnSave = true,
      prefillRequiredFields = true,
    },
  }

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
      enumMemberValues = { enabled = true },
      functionLikeReturnTypes = { enabled = true },
      parameterNames = { enabled = 'literals' },
      parameterTypes = { enabled = true },
      propertyDeclarationTypes = { enabled = true },
      variableTypes = { enabled = false },
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
      nodePath = '~/.config/nvim/run-electron-as-node',
      experimental = {
        enableProjectDiagnostics = true,
      },
      useSeparateSyntaxServer = false,
      useSyntaxServer = 'never',
      -- maxTsServerMemory = 4096,
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
    gopls = {
      settings = go_settings,
    },
    yamlls = {
      cmd = { 'yaml-language-server', '--stdio' },
    },
    terraformls = {
      cmd = { 'terraform-ls', 'serve' },
      init_options = tf_settings,
    },
    tflint = {
      cmd = { 'tflint', '--langserver' },
    },
    bashls = {
      cmd = { 'bash-language-server', 'start' },
      settings = {
        bashIde = {
          -- Glob pattern for finding and parsing shell script files in the workspace.
          -- Used by the background analysis features across files.

          -- Prevent recursive scanning which will cause issues when opening a file
          -- directly in the home directory (e.g. ~/foo.sh).
          --
          -- Default upstream pattern is "**/*@(.sh|.inc|.bash|.command)".
          globPattern = vim.env.GLOB_PATTERN or '*@(.sh|.inc|.bash|.command)',
        },
      },
      filetypes = {
        'sh',
        'zsh',
        'zshrc',
      },
    },
    prismals = {
      cmd = { 'prisma-language-server', '--stdio' },
      filetypes = { 'prisma' },
      settings = {
        prisma = {
          prismaFmtBinPath = '',
        },
      },
      root_markers = { '.git', 'package.json' },
    },
    -- shellcheck = {
    --   filetypes = {
    --     'sh',
    --     'zsh',
    --     'zshrc',
    --   },
    -- },
    -- shfmt = {
    --   filetypes = {
    --     'sh',
    --     'zsh',
    --     'zshrc',
    --   },
    -- },
    html = {
      cmd = { 'vscode-html-language-server', '--stdio' },
      filetypes = { 'html', 'templ' },
      root_markers = { 'package.json', '.git' },
      settings = {},
      init_options = {
        provideFormatter = true,
        embeddedLanguages = { css = true, javascript = true },
        configurationSection = { 'html', 'css', 'javascript' },
      },
    },
    buf_ls = {
      cmd = { 'buf', 'beta', 'lsp', '--timeout=0', '--log-format=text' },
      filetypes = { 'proto' },
      root_markers = { 'buf.yaml', '.git' },
    },
    dockerls = {
      cmd = { 'docker-langserver', '--stdio' },
      filetypes = { 'dockerfile' },
      root_markers = { 'Dockerfile' },
    },
    docker_compose_language_service = {
      cmd = { 'docker-compose-langserver', '--stdio' },
      filetypes = { 'yaml.docker-compose' },
      root_markers = { 'docker-compose.yaml', 'docker-compose.yml', 'compose.yaml', 'compose.yml' },
    },
    -- ruby_lsp = {},
    -- ts_ls = {
    --   cmd = { 'bun', 'run', 'typescript-language-server', '--stdio' },
    --   init_options = { hostInfo = 'neovim' },
    --   settings = {
    --     tsserver = {
    --       useSyntaxServer = 'never',
    --     },
    --   },
    --   handlers = {
    --     -- handle rename request for certain code actions like extracting functions / types
    --     ['_typescript.rename'] = function(_, result, ctx)
    --       local client = assert(vim.lsp.get_client_by_id(ctx.client_id))
    --       vim.lsp.util.show_document({
    --         uri = result.textDocument.uri,
    --         range = {
    --           start = result.position,
    --           ['end'] = result.position,
    --         },
    --       }, client.offset_encoding)
    --       vim.lsp.buf.rename()
    --       return vim.NIL
    --     end,
    --     ['textDocument/publishDiagnostics'] = function(_, result, ctx)
    --       if result.diagnostics == nil then
    --         return
    --       end
    --
    --       -- ignore some tsserver diagnostics
    --       local idx = 1
    --       while idx <= #result.diagnostics do
    --         local entry = result.diagnostics[idx]
    --
    --         local formatter = require('format-ts-errors')[entry.code]
    --         entry.message = formatter and formatter(entry.message) or entry.message
    --
    --         -- codes: https://github.com/microsoft/TypeScript/blob/main/src/compiler/diagnosticMessages.json
    --         if entry.code == 80001 then
    --           -- { message = "File is a CommonJS module; it may be converted to an ES module.", }
    --           table.remove(result.diagnostics, idx)
    --         else
    --           idx = idx + 1
    --         end
    --       end
    --
    --       vim.lsp.diagnostic.on_publish_diagnostics(_, result, ctx)
    --     end,
    --   },
    --   on_attach = function(client)
    --     -- ts_ls provides `source.*` code actions that apply to the whole file. These only appear in
    --     -- `vim.lsp.buf.code_action()` if specified in `context.only`.
    --     vim.api.nvim_buf_create_user_command(0, 'LspTypescriptSourceAction', function()
    --       local source_actions = vim.tbl_filter(function(action)
    --         return vim.startswith(action, 'source.')
    --       end, client.server_capabilities.codeActionProvider.codeActionKinds)
    --
    --       vim.lsp.buf.code_action {
    --         context = {
    --           only = source_actions,
    --         },
    --       }
    --     end, {})
    --   end,
    --   root_markers = { 'tsconfig.json', 'jsconfig.json', 'package.json', '.git' },
    --   root_dir = require('lspconfig.util').root_pattern '.git',
    -- filetypes = {
    --   'javascript',
    --   'javascriptreact',
    --   'javascript.jsx',
    --   'typescript',
    --   'typescriptreact',
    --   'typescript.tsx',
    -- },
    -- settings = {
    --   typescript = jsts_settings,
    --   javascript = jsts_settings,
    -- },
    -- },
    jsonls = {
      cmd = { 'vscode-json-language-server', '--stdio' },
      filetypes = { 'json', 'jsonc' },
      init_options = {
        provideFormatter = true,
      },
      root_markers = { '.git' },
      settings = {
        json = {
          schemas = require('schemastore').json.schemas(),
          validate = { enable = true },
        },
      },
    },
    vtsls = {
      -- cmd = { 'bun', 'run', '/home/phetoush/.bun/bin/vtsls', '--stdio' },
      cmd = { 'vtsls', '--stdio' },
      root_markers = { 'tsconfig.json', 'jsconfig.json', 'package.json', '.git' },
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
      settings = {
        complete_function_calls = true,
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
      },
    },
    -- ... etc. See `:help lspconfig-all` for a list of all the pre-configured LSPs
    --
    -- Some languages (like typescript) have entire language plugins that can be useful:
    --    https://github.com/pmizio/typescript-tools.nvim
    --
    -- But for many setups, the LSP (`tsserver`) will work just fine

    lua_ls = {
      -- cmd = {...},
      -- filetypes = { ...},
      -- capabilities = {},
      settings = {
        Lua = {
          hint = { enable = true },
          workspace = { checkThirdParty = false },
          telemetry = { enable = false },
          completion = { callSnippet = 'Replace' },
          diagnostics = {
            globals = { 'vim' },
          },
        },
      },
    },
    tailwindcss = {},
    cssls = {},
  }
end

return M
