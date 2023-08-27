// Decompiled with JetBrains decompiler
// Type: Microsoft.Extensions.Hosting.IHostedService
// Assembly: Microsoft.Extensions.Hosting.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 19873F1F-DA1A-442C-9CB8-AC0EBF06E619
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.Extensions.Hosting.Abstractions.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.Extensions.Hosting.Abstractions.xml

using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Microsoft.Extensions.Hosting
{
  /// <summary>
  /// Defines methods for objects that are managed by the host.
  /// </summary>
  public interface IHostedService
  {
    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    Task StartAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    Task StopAsync(CancellationToken cancellationToken);
  }
}
