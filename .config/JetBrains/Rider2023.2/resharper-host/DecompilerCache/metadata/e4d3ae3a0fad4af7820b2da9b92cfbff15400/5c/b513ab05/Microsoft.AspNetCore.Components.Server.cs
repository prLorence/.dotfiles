using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;

// Assembly Microsoft.AspNetCore.Components.Server, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: E4D3AE3A-0FAD-4AF7-820B-2DA9B92CFBFF
// Assembly references:
// System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// System.Memory, Version=7.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// Microsoft.Extensions.DependencyInjection.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.SignalR.Core, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// System.ComponentModel, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Microsoft.AspNetCore.StaticFiles, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// System.Collections, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Microsoft.AspNetCore.SignalR, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.Http.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.Routing, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.Http.Connections, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.Extensions.Options, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.SignalR.Common, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// System.Text.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// Microsoft.AspNetCore.Components, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.DataProtection.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.Extensions.Logging.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.Http.Features, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.Components.Web, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.Extensions.Configuration.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.Components.Authorization, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.AspNetCore.Hosting.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.JSInterop, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// System.Security.Claims, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// System.Collections.Concurrent, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Microsoft.Extensions.Caching.Memory, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// Microsoft.Extensions.Caching.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// System.IO.Pipelines, Version=7.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// Microsoft.AspNetCore.Connections.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60

[assembly: Extension]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: ReferenceAssembly]
[assembly: InternalsVisibleTo("Microsoft.AspNetCore.Blazor.DevServer, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: InternalsVisibleTo("Microsoft.AspNetCore.Components.Server.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
[assembly: TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: AssemblyMetadata("CommitHash", "b506e2cb7b6c0fe787305f8cfdee046b7b3f9a53")]
[assembly: AssemblyMetadata("SourceCommitUrl", "https://github.com/dotnet/aspnetcore/tree/b506e2cb7b6c0fe787305f8cfdee046b7b3f9a53")]
[assembly: AssemblyMetadata("Serviceable", "True")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: AssemblyDescription("Runtime server features for ASP.NET Core Components.")]
[assembly: AssemblyFileVersion("7.0.923.32110")]
[assembly: AssemblyInformationalVersion("7.0.9+b506e2cb7b6c0fe787305f8cfdee046b7b3f9a53")]
[assembly: AssemblyProduct("Microsoft ASP.NET Core")]
[assembly: AssemblyTitle("Microsoft.AspNetCore.Components.Server")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/aspnetcore")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyVersion("7.0.0.0")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

[module: UnverifiableCode]
[module: RefSafetyRules(11)]
[module: NullablePublicOnly(true)]
