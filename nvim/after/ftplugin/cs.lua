local Path = require("plenary.path")

-- AUTO-FILL NAMESPACE WHEN FILE IS EMPTY
local current_file_path = Path:new { vim.fn.expand("%:p:h") }
local current_file = Path:new { vim.fn.expand("%:p") }

local path_csproj_rel = current_file_path:make_relative()

local project_path = ""
local cs_namespace = ""

if string.match(path_csproj_rel, "src/") then
    project_path = path_csproj_rel:gsub("src/", "")
    cs_namespace = project_path:gsub("/", ".")
else
    cs_namespace = path_csproj_rel:gsub("/", ".")
end

local is_empty = current_file:head(1) == "" or nil

if is_empty then
    current_file:write("namespace " .. cs_namespace .. ";", 'w')
    vim.cmd('silent! edit')
end

-- REVERSE NUGET PACKAGE SEARCH
vim.keymap.set("n", "<leader>rp", function()
    local class_name = vim.fn.input("Reverse nuget package search: ")
    local res = assert(io.popen("reverse-nuget -c " .. class_name .. "| head -n 10"))
    local output = res:read('*a')
    res:close()
    vim.api.nvim_echo({ { output } }, false, {})
end)
