// Decompiled with JetBrains decompiler
// Type: Microsoft.Extensions.Hosting.BackgroundService
// Assembly: Microsoft.Extensions.Hosting.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 19873F1F-DA1A-442C-9CB8-AC0EBF06E619
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.Extensions.Hosting.Abstractions.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.Extensions.Hosting.Abstractions.xml

using System;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Microsoft.Extensions.Hosting
{
  /// <summary>
  /// Base class for implementing a long running <see cref="T:Microsoft.Extensions.Hosting.IHostedService" />.
  /// </summary>
  public abstract class BackgroundService : IHostedService, IDisposable
  {

    #nullable disable
    private Task _executeTask;
    private CancellationTokenSource _stoppingCts;


    #nullable enable
    /// <summary>Gets the Task that executes the background operation.</summary>
    /// <remarks>
    /// Will return <see langword="null" /> if the background operation hasn't started.
    /// </remarks>
    public virtual Task? ExecuteTask => this._executeTask;

    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
    /// <remarks>See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.</remarks>
    protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
      this._stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
      this._executeTask = this.ExecuteAsync(this._stoppingCts.Token);
      return this._executeTask.IsCompleted ? this._executeTask : Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
      if (this._executeTask == null)
        return;
      try
      {
        this._stoppingCts.Cancel();
      }
      finally
      {
        Task task = await Task.WhenAny(this._executeTask, Task.Delay(-1, cancellationToken)).ConfigureAwait(false);
      }
    }

    public virtual void Dispose() => this._stoppingCts?.Cancel();
  }
}
