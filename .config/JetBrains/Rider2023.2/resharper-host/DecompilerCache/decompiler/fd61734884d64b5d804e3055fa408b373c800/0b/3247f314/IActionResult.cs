// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Mvc.IActionResult
// Assembly: Microsoft.AspNetCore.Mvc.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: FD617348-84D6-4B5D-804E-3055FA408B37
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.AspNetCore.Mvc.Abstractions.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.AspNetCore.Mvc.Abstractions.xml

using System.Threading.Tasks;


#nullable enable
namespace Microsoft.AspNetCore.Mvc
{
  /// <summary>
  /// Defines a contract that represents the result of an action method.
  /// </summary>
  public interface IActionResult
  {
    /// <summary>
    /// Executes the result operation of the action method asynchronously. This method is called by MVC to process
    /// the result of an action method.
    /// </summary>
    /// <param name="context">The context in which the result is executed. The context information includes
    /// information about the action that was executed and request information.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    Task ExecuteResultAsync(ActionContext context);
  }
}
