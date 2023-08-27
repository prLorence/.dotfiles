// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Mvc.RouteAttribute
// Assembly: Microsoft.AspNetCore.Mvc.Core, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 76BDD43D-CA08-4BD9-AF9A-B65BD364F24C
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.AspNetCore.Mvc.Core.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.AspNetCore.Mvc.Core.xml

using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace Microsoft.AspNetCore.Mvc
{
  /// <summary>Specifies an attribute route on a controller.</summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  public class RouteAttribute : Attribute, IRouteTemplateProvider
  {

    #nullable disable
    private int? _order;


    #nullable enable
    /// <summary>
    /// Creates a new <see cref="T:Microsoft.AspNetCore.Mvc.RouteAttribute" /> with the given route template.
    /// </summary>
    /// <param name="template">The route template. May not be null.</param>
    public RouteAttribute([StringSyntax("Route")] string template) => this.Template = template ?? throw new ArgumentNullException(nameof (template));

    /// <inheritdoc />
    [StringSyntax("Route")]
    public string Template { get; }

    /// <summary>
    /// Gets the route order. The order determines the order of route execution. Routes with a lower order
    /// value are tried first. If an action defines a route by providing an <see cref="T:Microsoft.AspNetCore.Mvc.Routing.IRouteTemplateProvider" />
    /// with a non <c>null</c> order, that order is used instead of this value. If neither the action nor the
    /// controller defines an order, a default value of 0 is used.
    /// </summary>
    public int Order
    {
      get => this._order.GetValueOrDefault();
      set => this._order = new int?(value);
    }

    /// <inheritdoc />
    int? IRouteTemplateProvider.Order => this._order;

    /// <inheritdoc />
    public string? Name { get; set; }
  }
}
