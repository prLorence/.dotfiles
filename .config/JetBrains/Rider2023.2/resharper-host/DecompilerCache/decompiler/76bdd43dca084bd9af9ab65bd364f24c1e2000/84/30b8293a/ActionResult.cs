// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Mvc.ActionResult
// Assembly: Microsoft.AspNetCore.Mvc.Core, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 76BDD43D-CA08-4BD9-AF9A-B65BD364F24C
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.AspNetCore.Mvc.Core.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.AspNetCore.Mvc.Core.xml

using System.Threading.Tasks;


#nullable enable
namespace Microsoft.AspNetCore.Mvc
{
  /// <summary>
  /// A default implementation of <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult" />.
  /// </summary>
  public abstract class ActionResult : IActionResult
  {
    /// <summary>
    /// Executes the result operation of the action method asynchronously. This method is called by MVC to process
    /// the result of an action method.
    /// The default implementation of this method calls the <see cref="M:Microsoft.AspNetCore.Mvc.ActionResult.ExecuteResult(Microsoft.AspNetCore.Mvc.ActionContext)" /> method and
    /// returns a completed task.
    /// </summary>
    /// <param name="context">The context in which the result is executed. The context information includes
    /// information about the action that was executed and request information.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public virtual Task ExecuteResultAsync(ActionContext context)
    {
      this.ExecuteResult(context);
      return Task.CompletedTask;
    }

    /// <summary>
    /// Executes the result operation of the action method synchronously. This method is called by MVC to process
    /// the result of an action method.
    /// </summary>
    /// <param name="context">The context in which the result is executed. The context information includes
    /// information about the action that was executed and request information.</param>
    public virtual void ExecuteResult(ActionContext context)
    {
    }
  }
}
