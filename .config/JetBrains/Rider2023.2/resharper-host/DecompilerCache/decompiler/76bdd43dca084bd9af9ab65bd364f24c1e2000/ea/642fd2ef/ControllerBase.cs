// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Mvc.ControllerBase
// Assembly: Microsoft.AspNetCore.Mvc.Core, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 76BDD43D-CA08-4BD9-AF9A-B65BD364F24C
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.AspNetCore.Mvc.Core.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.AspNetCore.Mvc.Core.xml

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


#nullable enable
namespace Microsoft.AspNetCore.Mvc
{
  /// <summary>
  /// A base class for an MVC controller without view support.
  /// </summary>
  [Controller]
  public abstract class ControllerBase
  {

    #nullable disable
    private ControllerContext _controllerContext;
    private IModelMetadataProvider _metadataProvider;
    private IModelBinderFactory _modelBinderFactory;
    private IObjectModelValidator _objectValidator;
    private IUrlHelper _url;
    private ProblemDetailsFactory _problemDetailsFactory;


    #nullable enable
    /// <summary>
    /// Gets the <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the executing action.
    /// </summary>
    public HttpContext HttpContext => this.ControllerContext.HttpContext;

    /// <summary>
    /// Gets the <see cref="T:Microsoft.AspNetCore.Http.HttpRequest" /> for the executing action.
    /// </summary>
    public HttpRequest Request => this.HttpContext?.Request;

    /// <summary>
    /// Gets the <see cref="T:Microsoft.AspNetCore.Http.HttpResponse" /> for the executing action.
    /// </summary>
    public HttpResponse Response => this.HttpContext?.Response;

    /// <summary>
    /// Gets the <see cref="T:Microsoft.AspNetCore.Routing.RouteData" /> for the executing action.
    /// </summary>
    public RouteData RouteData => this.ControllerContext.RouteData;

    /// <summary>
    /// Gets the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> that contains the state of the model and of model-binding validation.
    /// </summary>
    public ModelStateDictionary ModelState => this.ControllerContext.ModelState;

    /// <summary>
    /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.ControllerContext" />.
    /// </summary>
    /// <remarks>
    /// <see cref="T:Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator" /> activates this property while activating controllers.
    /// If user code directly instantiates a controller, the getter returns an empty
    /// <see cref="T:Microsoft.AspNetCore.Mvc.ControllerContext" />.
    /// </remarks>
    [ControllerContext]
    public ControllerContext ControllerContext
    {
      get
      {
        if (this._controllerContext == null)
          this._controllerContext = new ControllerContext();
        return this._controllerContext;
      }
      set => this._controllerContext = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>
    /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelMetadataProvider" />.
    /// </summary>
    public IModelMetadataProvider MetadataProvider
    {
      get
      {
        if (this._metadataProvider == null)
        {
          HttpContext httpContext = this.HttpContext;
          IModelMetadataProvider metadataProvider;
          if (httpContext == null)
          {
            metadataProvider = (IModelMetadataProvider) null;
          }
          else
          {
            IServiceProvider requestServices = httpContext.RequestServices;
            metadataProvider = requestServices != null ? requestServices.GetRequiredService<IModelMetadataProvider>() : (IModelMetadataProvider) null;
          }
          this._metadataProvider = metadataProvider;
        }
        return this._metadataProvider;
      }
      set => this._metadataProvider = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>
    /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IModelBinderFactory" />.
    /// </summary>
    public IModelBinderFactory ModelBinderFactory
    {
      get
      {
        if (this._modelBinderFactory == null)
        {
          HttpContext httpContext = this.HttpContext;
          IModelBinderFactory modelBinderFactory;
          if (httpContext == null)
          {
            modelBinderFactory = (IModelBinderFactory) null;
          }
          else
          {
            IServiceProvider requestServices = httpContext.RequestServices;
            modelBinderFactory = requestServices != null ? requestServices.GetRequiredService<IModelBinderFactory>() : (IModelBinderFactory) null;
          }
          this._modelBinderFactory = modelBinderFactory;
        }
        return this._modelBinderFactory;
      }
      set => this._modelBinderFactory = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>
    /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.IUrlHelper" />.
    /// </summary>
    public IUrlHelper Url
    {
      get
      {
        if (this._url == null)
        {
          HttpContext httpContext = this.HttpContext;
          IUrlHelperFactory urlHelperFactory;
          if (httpContext == null)
          {
            urlHelperFactory = (IUrlHelperFactory) null;
          }
          else
          {
            IServiceProvider requestServices = httpContext.RequestServices;
            urlHelperFactory = requestServices != null ? requestServices.GetRequiredService<IUrlHelperFactory>() : (IUrlHelperFactory) null;
          }
          this._url = urlHelperFactory?.GetUrlHelper((ActionContext) this.ControllerContext);
        }
        return this._url;
      }
      set => this._url = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>
    /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IObjectModelValidator" />.
    /// </summary>
    public IObjectModelValidator ObjectValidator
    {
      get
      {
        if (this._objectValidator == null)
        {
          HttpContext httpContext = this.HttpContext;
          IObjectModelValidator objectModelValidator;
          if (httpContext == null)
          {
            objectModelValidator = (IObjectModelValidator) null;
          }
          else
          {
            IServiceProvider requestServices = httpContext.RequestServices;
            objectModelValidator = requestServices != null ? requestServices.GetRequiredService<IObjectModelValidator>() : (IObjectModelValidator) null;
          }
          this._objectValidator = objectModelValidator;
        }
        return this._objectValidator;
      }
      set => this._objectValidator = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>
    /// Gets or sets the <see cref="P:Microsoft.AspNetCore.Mvc.ControllerBase.ProblemDetailsFactory" />.
    /// </summary>
    public ProblemDetailsFactory ProblemDetailsFactory
    {
      get
      {
        if (this._problemDetailsFactory == null)
        {
          HttpContext httpContext = this.HttpContext;
          ProblemDetailsFactory problemDetailsFactory;
          if (httpContext == null)
          {
            problemDetailsFactory = (ProblemDetailsFactory) null;
          }
          else
          {
            IServiceProvider requestServices = httpContext.RequestServices;
            problemDetailsFactory = requestServices != null ? requestServices.GetRequiredService<ProblemDetailsFactory>() : (ProblemDetailsFactory) null;
          }
          this._problemDetailsFactory = problemDetailsFactory;
        }
        return this._problemDetailsFactory;
      }
      set => this._problemDetailsFactory = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>
    /// Gets the <see cref="T:System.Security.Claims.ClaimsPrincipal" /> for user associated with the executing action.
    /// </summary>
    public ClaimsPrincipal User => this.HttpContext?.User;

    /// <summary>
    /// Gets an instance of <see cref="T:Microsoft.AspNetCore.Mvc.EmptyResult" />.
    /// </summary>
    public static EmptyResult Empty { get; } = new EmptyResult();

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.StatusCodeResult" /> object by specifying a <paramref name="statusCode" />.
    /// </summary>
    /// <param name="statusCode">The status code to set on the response.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.StatusCodeResult" /> object for the response.</returns>
    [NonAction]
    public virtual StatusCodeResult StatusCode([ActionResultStatusCode] int statusCode) => new StatusCodeResult(statusCode);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ObjectResult" /> object by specifying a <paramref name="statusCode" /> and <paramref name="value" />
    /// </summary>
    /// <param name="statusCode">The status code to set on the response.</param>
    /// <param name="value">The value to set on the <see cref="T:Microsoft.AspNetCore.Mvc.ObjectResult" />.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ObjectResult" /> object for the response.</returns>
    [NonAction]
    public virtual ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object? value) => new ObjectResult(value)
    {
      StatusCode = new int?(statusCode)
    };

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object by specifying a <paramref name="content" /> string.
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object for the response.</returns>
    [NonAction]
    public virtual ContentResult Content(string content) => this.Content(content, (MediaTypeHeaderValue) null);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object by specifying a
    /// <paramref name="content" /> string and a content type.
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object for the response.</returns>
    [NonAction]
    public virtual ContentResult Content(string content, string contentType) => this.Content(content, MediaTypeHeaderValue.Parse((StringSegment) contentType));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object by specifying a
    /// <paramref name="content" /> string, a <paramref name="contentType" />, and <paramref name="contentEncoding" />.
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <param name="contentEncoding">The content encoding.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object for the response.</returns>
    /// <remarks>
    /// If encoding is provided by both the 'charset' and the <paramref name="contentEncoding" /> parameters, then
    /// the <paramref name="contentEncoding" /> parameter is chosen as the final encoding.
    /// </remarks>
    [NonAction]
    public virtual ContentResult Content(
      string content,
      string contentType,
      Encoding contentEncoding)
    {
      MediaTypeHeaderValue contentType1 = MediaTypeHeaderValue.Parse((StringSegment) contentType);
      contentType1.Encoding = contentEncoding ?? contentType1.Encoding;
      return this.Content(content, contentType1);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object by specifying a
    /// <paramref name="content" /> string and a <paramref name="contentType" />.
    /// </summary>
    /// <param name="content">The content to write to the response.</param>
    /// <param name="contentType">The content type (MIME type).</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ContentResult" /> object for the response.</returns>
    [NonAction]
    public virtual ContentResult Content(string content, MediaTypeHeaderValue? contentType) => new ContentResult()
    {
      Content = content,
      ContentType = contentType?.ToString()
    };

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.NoContentResult" /> object that produces an empty
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.NoContentResult" /> object for the response.</returns>
    [NonAction]
    public virtual NoContentResult NoContent() => new NoContentResult();

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.OkResult" /> object that produces an empty <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.OkResult" /> for the response.</returns>
    [NonAction]
    public virtual OkResult Ok() => new OkResult();

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.OkObjectResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" /> response.
    /// </summary>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.OkObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual OkObjectResult Ok([ActionResultObjectValue] object? value) => new OkObjectResult(value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> object that redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />)
    /// to the specified <paramref name="url" />.
    /// </summary>
    /// <param name="url">The URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectResult Redirect(string url) => !string.IsNullOrEmpty(url) ? new RedirectResult(url) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (url));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> object with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectResult.Permanent" /> set to true
    /// (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) using the specified <paramref name="url" />.
    /// </summary>
    /// <param name="url">The URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectResult RedirectPermanent(string url) => !string.IsNullOrEmpty(url) ? new RedirectResult(url, true) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (url));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> object with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectResult.Permanent" /> set to false
    /// and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectResult.PreserveMethod" /> set to true (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect" />)
    /// using the specified <paramref name="url" />.
    /// </summary>
    /// <param name="url">The URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectResult RedirectPreserveMethod(string url) => !string.IsNullOrEmpty(url) ? new RedirectResult(url, false, true) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (url));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> object with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectResult.Permanent" /> set to true
    /// and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectResult.PreserveMethod" /> set to true (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect" />)
    /// using the specified <paramref name="url" />.
    /// </summary>
    /// <param name="url">The URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectResult RedirectPermanentPreserveMethod(string url) => !string.IsNullOrEmpty(url) ? new RedirectResult(url, true, true) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (url));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> object that redirects
    /// (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified local <paramref name="localUrl" />.
    /// </summary>
    /// <param name="localUrl">The local URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual LocalRedirectResult LocalRedirect(string localUrl) => !string.IsNullOrEmpty(localUrl) ? new LocalRedirectResult(localUrl) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (localUrl));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> object with <see cref="P:Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent" /> set to
    /// true (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) using the specified <paramref name="localUrl" />.
    /// </summary>
    /// <param name="localUrl">The local URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual LocalRedirectResult LocalRedirectPermanent(string localUrl) => !string.IsNullOrEmpty(localUrl) ? new LocalRedirectResult(localUrl, true) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (localUrl));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> object with <see cref="P:Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent" /> set to
    /// false and <see cref="P:Microsoft.AspNetCore.Mvc.LocalRedirectResult.PreserveMethod" /> set to true
    /// (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect" />) using the specified <paramref name="localUrl" />.
    /// </summary>
    /// <param name="localUrl">The local URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual LocalRedirectResult LocalRedirectPreserveMethod(string localUrl) => !string.IsNullOrEmpty(localUrl) ? new LocalRedirectResult(localUrl, false, true) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (localUrl));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> object with <see cref="P:Microsoft.AspNetCore.Mvc.LocalRedirectResult.Permanent" /> set to
    /// true and <see cref="P:Microsoft.AspNetCore.Mvc.LocalRedirectResult.PreserveMethod" /> set to true
    /// (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect" />) using the specified <paramref name="localUrl" />.
    /// </summary>
    /// <param name="localUrl">The local URL to redirect to.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.LocalRedirectResult" /> for the response.</returns>
    [NonAction]
    public virtual LocalRedirectResult LocalRedirectPermanentPreserveMethod(string localUrl) => !string.IsNullOrEmpty(localUrl) ? new LocalRedirectResult(localUrl, true, true) : throw new ArgumentException(Microsoft.AspNetCore.Mvc.Core.Resources.ArgumentCannotBeNullOrEmpty, nameof (localUrl));

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to an action with the same name as current one.
    /// The 'controller' and 'action' names are retrieved from the ambient values of the current request.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    /// <example>
    /// A POST request to an action named "Product" updates a product and redirects to an action, also named
    /// "Product", showing details of the updated product.
    /// <code>
    /// [HttpGet]
    /// public IActionResult Product(int id)
    /// {
    ///     var product = RetrieveProduct(id);
    ///     return View(product);
    /// }
    /// 
    /// [HttpPost]
    /// public IActionResult Product(int id, Product product)
    /// {
    ///     UpdateProduct(product);
    ///     return RedirectToAction();
    /// }
    /// </code>
    /// </example>
    [NonAction]
    public virtual RedirectToActionResult RedirectToAction() => this.RedirectToAction((string) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified action using the <paramref name="actionName" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToAction(string? actionName) => this.RedirectToAction(actionName, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified action using the
    /// <paramref name="actionName" /> and <paramref name="routeValues" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToAction(string? actionName, object? routeValues) => this.RedirectToAction(actionName, (string) null, routeValues);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified action using the
    /// <paramref name="actionName" /> and the <paramref name="controllerName" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToAction(string? actionName, string? controllerName) => this.RedirectToAction(actionName, controllerName, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified action using the specified
    /// <paramref name="actionName" />, <paramref name="controllerName" />, and <paramref name="routeValues" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToAction(
      string? actionName,
      string? controllerName,
      object? routeValues)
    {
      return this.RedirectToAction(actionName, controllerName, routeValues, (string) null);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified action using the specified
    /// <paramref name="actionName" />, <paramref name="controllerName" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToAction(
      string? actionName,
      string? controllerName,
      string? fragment)
    {
      return this.RedirectToAction(actionName, controllerName, (object) null, fragment);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified action using the specified <paramref name="actionName" />,
    /// <paramref name="controllerName" />, <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToAction(
      string? actionName,
      string? controllerName,
      object? routeValues,
      string? fragment)
    {
      return new RedirectToActionResult(actionName, controllerName, routeValues, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to false and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.PreserveMethod" />
    /// set to true, using the specified <paramref name="actionName" />, <paramref name="controllerName" />,
    /// <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPreserveMethod(
      string? actionName = null,
      string? controllerName = null,
      object? routeValues = null,
      string? fragment = null)
    {
      return new RedirectToActionResult(actionName, controllerName, routeValues, false, true, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to true using the specified <paramref name="actionName" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPermanent(string? actionName) => this.RedirectToActionPermanent(actionName, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to true using the specified <paramref name="actionName" />
    /// and <paramref name="routeValues" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPermanent(
      string? actionName,
      object? routeValues)
    {
      return this.RedirectToActionPermanent(actionName, (string) null, routeValues);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to true using the specified <paramref name="actionName" />
    /// and <paramref name="controllerName" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPermanent(
      string? actionName,
      string? controllerName)
    {
      return this.RedirectToActionPermanent(actionName, controllerName, (object) null);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to true using the specified <paramref name="actionName" />,
    /// <paramref name="controllerName" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPermanent(
      string? actionName,
      string? controllerName,
      string? fragment)
    {
      return this.RedirectToActionPermanent(actionName, controllerName, (object) null, fragment);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to true using the specified <paramref name="actionName" />,
    /// <paramref name="controllerName" />, and <paramref name="routeValues" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPermanent(
      string? actionName,
      string? controllerName,
      object? routeValues)
    {
      return this.RedirectToActionPermanent(actionName, controllerName, routeValues, (string) null);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to true using the specified <paramref name="actionName" />,
    /// <paramref name="controllerName" />, <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPermanent(
      string? actionName,
      string? controllerName,
      object? routeValues,
      string? fragment)
    {
      return new RedirectToActionResult(actionName, controllerName, routeValues, true, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect" />) to the specified action with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.Permanent" /> set to true and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToActionResult.PreserveMethod" />
    /// set to true, using the specified <paramref name="actionName" />, <paramref name="controllerName" />,
    /// <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="actionName">The name of the action.</param>
    /// <param name="controllerName">The name of the controller.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToActionResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToActionResult RedirectToActionPermanentPreserveMethod(
      string? actionName = null,
      string? controllerName = null,
      object? routeValues = null,
      string? fragment = null)
    {
      return new RedirectToActionResult(actionName, controllerName, routeValues, true, true, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified route using the specified <paramref name="routeName" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoute(string? routeName) => this.RedirectToRoute(routeName, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified route using the specified <paramref name="routeValues" />.
    /// </summary>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoute(object? routeValues) => this.RedirectToRoute((string) null, routeValues);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified route using the specified
    /// <paramref name="routeName" /> and <paramref name="routeValues" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoute(string? routeName, object? routeValues) => this.RedirectToRoute(routeName, routeValues, (string) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified route using the specified
    /// <paramref name="routeName" /> and <paramref name="fragment" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoute(string? routeName, string? fragment) => this.RedirectToRoute(routeName, (object) null, fragment);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified route using the specified
    /// <paramref name="routeName" />, <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoute(
      string? routeName,
      object? routeValues,
      string? fragment)
    {
      return new RedirectToRouteResult(routeName, routeValues, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to false and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod" />
    /// set to true, using the specified <paramref name="routeName" />, <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoutePreserveMethod(
      string? routeName = null,
      object? routeValues = null,
      string? fragment = null)
    {
      return new RedirectToRouteResult(routeName, routeValues, false, true, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to true using the specified <paramref name="routeName" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoutePermanent(string? routeName) => this.RedirectToRoutePermanent(routeName, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to true using the specified <paramref name="routeValues" />.
    /// </summary>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoutePermanent(object? routeValues) => this.RedirectToRoutePermanent((string) null, routeValues);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to true using the specified <paramref name="routeName" />
    /// and <paramref name="routeValues" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoutePermanent(
      string? routeName,
      object? routeValues)
    {
      return this.RedirectToRoutePermanent(routeName, routeValues, (string) null);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to true using the specified <paramref name="routeName" />
    /// and <paramref name="fragment" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoutePermanent(string? routeName, string? fragment) => this.RedirectToRoutePermanent(routeName, (object) null, fragment);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to true using the specified <paramref name="routeName" />,
    /// <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoutePermanent(
      string? routeName,
      object? routeValues,
      string? fragment)
    {
      return new RedirectToRouteResult(routeName, routeValues, true, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to true and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod" />
    /// set to true, using the specified <paramref name="routeName" />, <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="routeName">The name of the route.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToRouteResult RedirectToRoutePermanentPreserveMethod(
      string? routeName = null,
      object? routeValues = null,
      string? fragment = null)
    {
      return new RedirectToRouteResult(routeName, routeValues, true, true, fragment)
      {
        UrlHelper = this.Url
      };
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified <paramref name="pageName" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" />.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPage(string pageName) => this.RedirectToPage(pageName, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="routeValues" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" />.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPage(string pageName, object? routeValues) => this.RedirectToPage(pageName, (string) null, routeValues, (string) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="pageHandler" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" />.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPage(string pageName, string? pageHandler) => this.RedirectToPage(pageName, pageHandler, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified <paramref name="pageName" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" />.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPage(
      string pageName,
      string? pageHandler,
      object? routeValues)
    {
      return this.RedirectToPage(pageName, pageHandler, routeValues, (string) null);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="fragment" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" />.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPage(
      string pageName,
      string? pageHandler,
      string? fragment)
    {
      return this.RedirectToPage(pageName, pageHandler, (object) null, fragment);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status302Found" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="routeValues" /> and <paramref name="fragment" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" />.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPage(
      string pageName,
      string? pageHandler,
      object? routeValues,
      string? fragment)
    {
      return new RedirectToPageResult(pageName, pageHandler, routeValues, fragment);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified <paramref name="pageName" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" /> with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent" /> set.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPagePermanent(string pageName) => this.RedirectToPagePermanent(pageName, (object) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="routeValues" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" /> with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent" /> set.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, object? routeValues) => this.RedirectToPagePermanent(pageName, (string) null, routeValues, (string) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="pageHandler" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" /> with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent" /> set.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPagePermanent(string pageName, string? pageHandler) => this.RedirectToPagePermanent(pageName, pageHandler, (object) null, (string) null);

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="fragment" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" /> with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent" /> set.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPagePermanent(
      string pageName,
      string? pageHandler,
      string? fragment)
    {
      return this.RedirectToPagePermanent(pageName, pageHandler, (object) null, fragment);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status301MovedPermanently" />) to the specified <paramref name="pageName" />
    /// using the specified <paramref name="routeValues" /> and <paramref name="fragment" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="routeValues">The parameters for a route.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToPageResult" /> with <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToPageResult.Permanent" /> set.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPagePermanent(
      string pageName,
      string? pageHandler,
      object? routeValues,
      string? fragment)
    {
      return new RedirectToPageResult(pageName, pageHandler, routeValues, true, fragment);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status307TemporaryRedirect" />) to the specified page with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to false and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod" />
    /// set to true, using the specified <paramref name="pageName" />, <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPagePreserveMethod(
      string pageName,
      string? pageHandler = null,
      object? routeValues = null,
      string? fragment = null)
    {
      if (pageName == null)
        throw new ArgumentNullException(nameof (pageName));
      return new RedirectToPageResult(pageName, pageHandler, routeValues, false, true, fragment);
    }

    /// <summary>
    /// Redirects (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect" />) to the specified route with
    /// <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.Permanent" /> set to true and <see cref="P:Microsoft.AspNetCore.Mvc.RedirectToRouteResult.PreserveMethod" />
    /// set to true, using the specified <paramref name="pageName" />, <paramref name="routeValues" />, and <paramref name="fragment" />.
    /// </summary>
    /// <param name="pageName">The name of the page.</param>
    /// <param name="pageHandler">The page handler to redirect to.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="fragment">The fragment to add to the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.RedirectToRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual RedirectToPageResult RedirectToPagePermanentPreserveMethod(
      string pageName,
      string? pageHandler = null,
      object? routeValues = null,
      string? fragment = null)
    {
      if (pageName == null)
        throw new ArgumentNullException(nameof (pageName));
      return new RedirectToPageResult(pageName, pageHandler, routeValues, true, true, fragment);
    }

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />),
    /// and the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(byte[] fileContents, string contentType) => this.File(fileContents, contentType, (string) null);

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />),
    /// and the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      bool enableRangeProcessing)
    {
      return this.File(fileContents, contentType, (string) null, enableRangeProcessing);
    }

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      string? fileDownloadName)
    {
      FileContentResult fileContentResult = new FileContentResult(fileContents, contentType);
      fileContentResult.FileDownloadName = fileDownloadName;
      return fileContentResult;
    }

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      string? fileDownloadName,
      bool enableRangeProcessing)
    {
      FileContentResult fileContentResult = new FileContentResult(fileContents, contentType);
      fileContentResult.FileDownloadName = fileDownloadName;
      fileContentResult.EnableRangeProcessing = enableRangeProcessing;
      return fileContentResult;
    }

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />),
    /// and the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      FileContentResult fileContentResult = new FileContentResult(fileContents, contentType);
      fileContentResult.LastModified = lastModified;
      fileContentResult.EntityTag = entityTag;
      return fileContentResult;
    }

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />),
    /// and the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      FileContentResult fileContentResult = new FileContentResult(fileContents, contentType);
      fileContentResult.LastModified = lastModified;
      fileContentResult.EntityTag = entityTag;
      fileContentResult.EnableRangeProcessing = enableRangeProcessing;
      return fileContentResult;
    }

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      FileContentResult fileContentResult = new FileContentResult(fileContents, contentType);
      fileContentResult.LastModified = lastModified;
      fileContentResult.EntityTag = entityTag;
      fileContentResult.FileDownloadName = fileDownloadName;
      return fileContentResult;
    }

    /// <summary>
    /// Returns a file with the specified <paramref name="fileContents" /> as content (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileContents">The file contents.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileContentResult" /> for the response.</returns>
    [NonAction]
    public virtual FileContentResult File(
      byte[] fileContents,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      FileContentResult fileContentResult = new FileContentResult(fileContents, contentType);
      fileContentResult.LastModified = lastModified;
      fileContentResult.EntityTag = entityTag;
      fileContentResult.FileDownloadName = fileDownloadName;
      fileContentResult.EnableRangeProcessing = enableRangeProcessing;
      return fileContentResult;
    }

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), with the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(Stream fileStream, string contentType) => this.File(fileStream, contentType, (string) null);

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), with the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      bool enableRangeProcessing)
    {
      return this.File(fileStream, contentType, (string) null, enableRangeProcessing);
    }

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type and the
    /// specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      string? fileDownloadName)
    {
      FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType);
      fileStreamResult.FileDownloadName = fileDownloadName;
      return fileStreamResult;
    }

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type and the
    /// specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      string? fileDownloadName,
      bool enableRangeProcessing)
    {
      FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType);
      fileStreamResult.FileDownloadName = fileDownloadName;
      fileStreamResult.EnableRangeProcessing = enableRangeProcessing;
      return fileStreamResult;
    }

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />),
    /// and the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType);
      fileStreamResult.LastModified = lastModified;
      fileStreamResult.EntityTag = entityTag;
      return fileStreamResult;
    }

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />),
    /// and the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType);
      fileStreamResult.LastModified = lastModified;
      fileStreamResult.EntityTag = entityTag;
      fileStreamResult.EnableRangeProcessing = enableRangeProcessing;
      return fileStreamResult;
    }

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType);
      fileStreamResult.LastModified = lastModified;
      fileStreamResult.EntityTag = entityTag;
      fileStreamResult.FileDownloadName = fileDownloadName;
      return fileStreamResult;
    }

    /// <summary>
    /// Returns a file in the specified <paramref name="fileStream" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="fileStream">The <see cref="T:System.IO.Stream" /> with the contents of the file.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult" /> for the response.</returns>
    /// <remarks>
    /// The <paramref name="fileStream" /> parameter is disposed after the response is sent.
    /// </remarks>
    [NonAction]
    public virtual FileStreamResult File(
      Stream fileStream,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType);
      fileStreamResult.LastModified = lastModified;
      fileStreamResult.EntityTag = entityTag;
      fileStreamResult.FileDownloadName = fileDownloadName;
      fileStreamResult.EnableRangeProcessing = enableRangeProcessing;
      return fileStreamResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(string virtualPath, string contentType) => this.File(virtualPath, contentType, (string) null);

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(
      string virtualPath,
      string contentType,
      bool enableRangeProcessing)
    {
      return this.File(virtualPath, contentType, (string) null, enableRangeProcessing);
    }

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type and the
    /// specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(
      string virtualPath,
      string contentType,
      string? fileDownloadName)
    {
      VirtualFileResult virtualFileResult = new VirtualFileResult(virtualPath, contentType);
      virtualFileResult.FileDownloadName = fileDownloadName;
      return virtualFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type and the
    /// specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(
      string virtualPath,
      string contentType,
      string? fileDownloadName,
      bool enableRangeProcessing)
    {
      VirtualFileResult virtualFileResult = new VirtualFileResult(virtualPath, contentType);
      virtualFileResult.FileDownloadName = fileDownloadName;
      virtualFileResult.EnableRangeProcessing = enableRangeProcessing;
      return virtualFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), and the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(
      string virtualPath,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      VirtualFileResult virtualFileResult = new VirtualFileResult(virtualPath, contentType);
      virtualFileResult.LastModified = lastModified;
      virtualFileResult.EntityTag = entityTag;
      return virtualFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), and the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(
      string virtualPath,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      VirtualFileResult virtualFileResult = new VirtualFileResult(virtualPath, contentType);
      virtualFileResult.LastModified = lastModified;
      virtualFileResult.EntityTag = entityTag;
      virtualFileResult.EnableRangeProcessing = enableRangeProcessing;
      return virtualFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(
      string virtualPath,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      VirtualFileResult virtualFileResult = new VirtualFileResult(virtualPath, contentType);
      virtualFileResult.LastModified = lastModified;
      virtualFileResult.EntityTag = entityTag;
      virtualFileResult.FileDownloadName = fileDownloadName;
      return virtualFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="virtualPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="virtualPath">The virtual path of the file to be returned.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.VirtualFileResult" /> for the response.</returns>
    [NonAction]
    public virtual VirtualFileResult File(
      string virtualPath,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      VirtualFileResult virtualFileResult = new VirtualFileResult(virtualPath, contentType);
      virtualFileResult.LastModified = lastModified;
      virtualFileResult.EntityTag = entityTag;
      virtualFileResult.FileDownloadName = fileDownloadName;
      virtualFileResult.EnableRangeProcessing = enableRangeProcessing;
      return virtualFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(string physicalPath, string contentType) => this.PhysicalFile(physicalPath, contentType, (string) null);

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(
      string physicalPath,
      string contentType,
      bool enableRangeProcessing)
    {
      return this.PhysicalFile(physicalPath, contentType, (string) null, enableRangeProcessing);
    }

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type and the
    /// specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(
      string physicalPath,
      string contentType,
      string? fileDownloadName)
    {
      PhysicalFileResult physicalFileResult = new PhysicalFileResult(physicalPath, contentType);
      physicalFileResult.FileDownloadName = fileDownloadName;
      return physicalFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />) with the
    /// specified <paramref name="contentType" /> as the Content-Type and the
    /// specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(
      string physicalPath,
      string contentType,
      string? fileDownloadName,
      bool enableRangeProcessing)
    {
      PhysicalFileResult physicalFileResult = new PhysicalFileResult(physicalPath, contentType);
      physicalFileResult.FileDownloadName = fileDownloadName;
      physicalFileResult.EnableRangeProcessing = enableRangeProcessing;
      return physicalFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), and
    /// the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(
      string physicalPath,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      PhysicalFileResult physicalFileResult = new PhysicalFileResult(physicalPath, contentType);
      physicalFileResult.LastModified = lastModified;
      physicalFileResult.EntityTag = entityTag;
      return physicalFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), and
    /// the specified <paramref name="contentType" /> as the Content-Type.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(
      string physicalPath,
      string contentType,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      PhysicalFileResult physicalFileResult = new PhysicalFileResult(physicalPath, contentType);
      physicalFileResult.LastModified = lastModified;
      physicalFileResult.EntityTag = entityTag;
      physicalFileResult.EnableRangeProcessing = enableRangeProcessing;
      return physicalFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(
      string physicalPath,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag)
    {
      PhysicalFileResult physicalFileResult = new PhysicalFileResult(physicalPath, contentType);
      physicalFileResult.LastModified = lastModified;
      physicalFileResult.EntityTag = entityTag;
      physicalFileResult.FileDownloadName = fileDownloadName;
      return physicalFileResult;
    }

    /// <summary>
    /// Returns the file specified by <paramref name="physicalPath" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status200OK" />), the
    /// specified <paramref name="contentType" /> as the Content-Type, and the specified <paramref name="fileDownloadName" /> as the suggested file name.
    /// This supports range requests (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent" /> or
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status416RangeNotSatisfiable" /> if the range is not satisfiable).
    /// </summary>
    /// <param name="physicalPath">The path to the file. The path must be an absolute path.</param>
    /// <param name="contentType">The Content-Type of the file.</param>
    /// <param name="fileDownloadName">The suggested file name.</param>
    /// <param name="lastModified">The <see cref="T:System.DateTimeOffset" /> of when the file was last modified.</param>
    /// <param name="entityTag">The <see cref="T:Microsoft.Net.Http.Headers.EntityTagHeaderValue" /> associated with the file.</param>
    /// <param name="enableRangeProcessing">Set to <c>true</c> to enable range requests processing.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.PhysicalFileResult" /> for the response.</returns>
    [NonAction]
    public virtual PhysicalFileResult PhysicalFile(
      string physicalPath,
      string contentType,
      string? fileDownloadName,
      DateTimeOffset? lastModified,
      EntityTagHeaderValue entityTag,
      bool enableRangeProcessing)
    {
      PhysicalFileResult physicalFileResult = new PhysicalFileResult(physicalPath, contentType);
      physicalFileResult.LastModified = lastModified;
      physicalFileResult.EntityTag = entityTag;
      physicalFileResult.FileDownloadName = fileDownloadName;
      physicalFileResult.EnableRangeProcessing = enableRangeProcessing;
      return physicalFileResult;
    }

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.UnauthorizedResult" /> that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.UnauthorizedResult" /> for the response.</returns>
    [NonAction]
    public virtual UnauthorizedResult Unauthorized() => new UnauthorizedResult();

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual UnauthorizedObjectResult Unauthorized([ActionResultObjectValue] object? value) => new UnauthorizedObjectResult(value);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.NotFoundResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.NotFoundResult" /> for the response.</returns>
    [NonAction]
    public virtual NotFoundResult NotFound() => new NotFoundResult();

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.NotFoundObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.NotFoundObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual NotFoundObjectResult NotFound([ActionResultObjectValue] object? value) => new NotFoundObjectResult(value);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestResult" /> for the response.</returns>
    [NonAction]
    public virtual BadRequestResult BadRequest() => new BadRequestResult();

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest" /> response.
    /// </summary>
    /// <param name="error">An error object to be returned to the client.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual BadRequestObjectResult BadRequest([ActionResultObjectValue] object? error) => new BadRequestObjectResult(error);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest" /> response.
    /// </summary>
    /// <param name="modelState">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> containing errors to be returned to the client.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState) => modelState != null ? new BadRequestObjectResult(modelState) : throw new ArgumentNullException(nameof (modelState));

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.UnprocessableEntityResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.UnprocessableEntityResult" /> for the response.</returns>
    [NonAction]
    public virtual UnprocessableEntityResult UnprocessableEntity() => new UnprocessableEntityResult();

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity" /> response.
    /// </summary>
    /// <param name="error">An error object to be returned to the client.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual UnprocessableEntityObjectResult UnprocessableEntity([ActionResultObjectValue] object? error) => new UnprocessableEntityObjectResult(error);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status422UnprocessableEntity" /> response.
    /// </summary>
    /// <param name="modelState">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> containing errors to be returned to the client.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual UnprocessableEntityObjectResult UnprocessableEntity(
      [ActionResultObjectValue] ModelStateDictionary modelState)
    {
      return modelState != null ? new UnprocessableEntityObjectResult(modelState) : throw new ArgumentNullException(nameof (modelState));
    }

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.ConflictResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ConflictResult" /> for the response.</returns>
    [NonAction]
    public virtual ConflictResult Conflict() => new ConflictResult();

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.ConflictObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict" /> response.
    /// </summary>
    /// <param name="error">Contains errors to be returned to the client.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ConflictObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual ConflictObjectResult Conflict([ActionResultObjectValue] object? error) => new ConflictObjectResult(error);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.ConflictObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict" /> response.
    /// </summary>
    /// <param name="modelState">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> containing errors to be returned to the client.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ConflictObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual ConflictObjectResult Conflict([ActionResultObjectValue] ModelStateDictionary modelState) => new ConflictObjectResult(modelState);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.ObjectResult" /> that produces a <see cref="T:Microsoft.AspNetCore.Mvc.ProblemDetails" /> response.
    /// </summary>
    /// <param name="statusCode">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Status" />.</param>
    /// <param name="detail">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Detail" />.</param>
    /// <param name="instance">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Instance" />.</param>
    /// <param name="title">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Title" />.</param>
    /// <param name="type">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Type" />.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ObjectResult" /> for the response.</returns>
    [NonAction]
    public virtual ObjectResult Problem(
      string? detail = null,
      string? instance = null,
      int? statusCode = null,
      string? title = null,
      string? type = null)
    {
      ProblemDetails problemDetails;
      if (this.ProblemDetailsFactory == null)
        problemDetails = new ProblemDetails()
        {
          Detail = detail,
          Instance = instance,
          Status = new int?(statusCode ?? 500),
          Title = title,
          Type = type
        };
      else
        problemDetails = this.ProblemDetailsFactory.CreateProblemDetails(this.HttpContext, new int?(statusCode ?? 500), title, type, detail, instance);
      return new ObjectResult((object) problemDetails)
      {
        StatusCode = problemDetails.Status
      };
    }

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestObjectResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestObjectResult" /> for the response.</returns>
    [NonAction]
    [DefaultStatusCode(400)]
    public virtual ActionResult ValidationProblem([ActionResultObjectValue] ValidationProblemDetails descriptor) => descriptor != null ? (ActionResult) new BadRequestObjectResult((object) descriptor) : throw new ArgumentNullException(nameof (descriptor));

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest" /> response
    /// with validation errors from <paramref name="modelStateDictionary" />.
    /// </summary>
    /// <param name="modelStateDictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.BadRequestObjectResult" /> for the response.</returns>
    [NonAction]
    [DefaultStatusCode(400)]
    public virtual ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary) => this.ValidationProblem((string) null, (string) null, new int?(), (string) null, (string) null, modelStateDictionary);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest" /> response
    /// with validation errors from <see cref="P:Microsoft.AspNetCore.Mvc.ControllerBase.ModelState" />.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult" /> for the response.</returns>
    [NonAction]
    [DefaultStatusCode(400)]
    public virtual ActionResult ValidationProblem() => this.ValidationProblem(this.ModelState);

    /// <summary>
    /// Creates an <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult" /> that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest" /> response
    /// with a <see cref="T:Microsoft.AspNetCore.Mvc.ValidationProblemDetails" /> value.
    /// </summary>
    /// <param name="detail">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Detail" />.</param>
    /// <param name="instance">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Instance" />.</param>
    /// <param name="statusCode">The status code.</param>
    /// <param name="title">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Title" />.</param>
    /// <param name="type">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Type" />.</param>
    /// <param name="modelStateDictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.
    /// When <see langword="null" /> uses <see cref="P:Microsoft.AspNetCore.Mvc.ControllerBase.ModelState" />.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult" /> for the response.</returns>
    [NonAction]
    [DefaultStatusCode(400)]
    public virtual ActionResult ValidationProblem(
      string? detail = null,
      string? instance = null,
      int? statusCode = null,
      string? title = null,
      string? type = null,
      [ActionResultObjectValue] ModelStateDictionary? modelStateDictionary = null)
    {
      if (modelStateDictionary == null)
        modelStateDictionary = this.ModelState;
      ValidationProblemDetails error;
      if (this.ProblemDetailsFactory == null)
      {
        ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(modelStateDictionary);
        validationProblemDetails.Detail = detail;
        validationProblemDetails.Instance = instance;
        validationProblemDetails.Status = statusCode;
        validationProblemDetails.Title = title;
        validationProblemDetails.Type = type;
        error = validationProblemDetails;
      }
      else
        error = this.ProblemDetailsFactory?.CreateValidationProblemDetails(this.HttpContext, modelStateDictionary, statusCode, title, type, detail, instance);
      if (error != null)
      {
        int? status = error.Status;
        if (status.HasValue && status.GetValueOrDefault() == 400)
          return (ActionResult) new BadRequestObjectResult((object) error);
      }
      return (ActionResult) new ObjectResult((object) error)
      {
        StatusCode = (int?) error?.Status
      };
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="uri">The URI at which the content has been created.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedResult Created(string uri, [ActionResultObjectValue] object? value) => uri != null ? new CreatedResult(uri, value) : throw new ArgumentNullException(nameof (uri));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="uri">The URI at which the content has been created.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedResult Created(Uri uri, [ActionResultObjectValue] object? value) => !(uri == (Uri) null) ? new CreatedResult(uri, value) : throw new ArgumentNullException(nameof (uri));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtActionResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedAtActionResult CreatedAtAction(string? actionName, [ActionResultObjectValue] object? value) => this.CreatedAtAction(actionName, (object) null, value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtActionResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedAtActionResult CreatedAtAction(
      string? actionName,
      object? routeValues,
      [ActionResultObjectValue] object? value)
    {
      return this.CreatedAtAction(actionName, (string) null, routeValues, value);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtActionResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedAtActionResult CreatedAtAction(
      string? actionName,
      string? controllerName,
      object? routeValues,
      [ActionResultObjectValue] object? value)
    {
      return new CreatedAtActionResult(actionName, controllerName, routeValues, value);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtRouteResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedAtRouteResult CreatedAtRoute(string? routeName, [ActionResultObjectValue] object? value) => this.CreatedAtRoute(routeName, (object) null, value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtRouteResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedAtRouteResult CreatedAtRoute(object? routeValues, [ActionResultObjectValue] object? value) => this.CreatedAtRoute((string) null, routeValues, value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtRouteResult" /> object that produces a <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status201Created" /> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The content value to format in the entity body.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.CreatedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual CreatedAtRouteResult CreatedAtRoute(
      string? routeName,
      object? routeValues,
      [ActionResultObjectValue] object? value)
    {
      return new CreatedAtRouteResult(routeName, routeValues, value);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedResult Accepted() => new AcceptedResult();

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedResult Accepted([ActionResultObjectValue] object? value) => new AcceptedResult((string) null, value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="uri">The optional URI with the location at which the status of requested content can be monitored.
    /// May be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedResult Accepted(Uri uri) => !(uri == (Uri) null) ? new AcceptedResult(uri, (object) null) : throw new ArgumentNullException(nameof (uri));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="uri">The optional URI with the location at which the status of requested content can be monitored.
    /// May be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedResult Accepted(string? uri) => new AcceptedResult(uri, (object) null);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="uri">The URI with the location at which the status of requested content can be monitored.</param>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedResult Accepted(string? uri, [ActionResultObjectValue] object? value) => new AcceptedResult(uri, value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="uri">The URI with the location at which the status of requested content can be monitored.</param>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedResult Accepted(Uri uri, [ActionResultObjectValue] object? value) => !(uri == (Uri) null) ? new AcceptedResult(uri, value) : throw new ArgumentNullException(nameof (uri));

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtActionResult AcceptedAtAction(string? actionName) => this.AcceptedAtAction(actionName, (object) null, (object) null);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtActionResult AcceptedAtAction(string? actionName, string? controllerName) => this.AcceptedAtAction(actionName, controllerName, (object) null, (object) null);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtActionResult AcceptedAtAction(string? actionName, [ActionResultObjectValue] object? value) => this.AcceptedAtAction(actionName, (object) null, value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtActionResult AcceptedAtAction(
      string? actionName,
      string? controllerName,
      [ActionResultObjectValue] object? routeValues)
    {
      return this.AcceptedAtAction(actionName, controllerName, routeValues, (object) null);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtActionResult AcceptedAtAction(
      string? actionName,
      object? routeValues,
      [ActionResultObjectValue] object? value)
    {
      return this.AcceptedAtAction(actionName, (string) null, routeValues, value);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="actionName">The name of the action to use for generating the URL.</param>
    /// <param name="controllerName">The name of the controller to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtActionResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtActionResult AcceptedAtAction(
      string? actionName,
      string? controllerName,
      object? routeValues,
      [ActionResultObjectValue] object? value)
    {
      return new AcceptedAtActionResult(actionName, controllerName, routeValues, value);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtRouteResult AcceptedAtRoute([ActionResultObjectValue] object? routeValues) => this.AcceptedAtRoute((string) null, routeValues, (object) null);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtRouteResult AcceptedAtRoute(string? routeName) => this.AcceptedAtRoute(routeName, (object) null, (object) null);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtRouteResult AcceptedAtRoute(string? routeName, object? routeValues) => this.AcceptedAtRoute(routeName, routeValues, (object) null);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtRouteResult AcceptedAtRoute(object? routeValues, [ActionResultObjectValue] object? value) => this.AcceptedAtRoute((string) null, routeValues, value);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> object that produces an <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status202Accepted" /> response.
    /// </summary>
    /// <param name="routeName">The name of the route to use for generating the URL.</param>
    /// <param name="routeValues">The route data to use for generating the URL.</param>
    /// <param name="value">The optional content value to format in the entity body; may be null.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.AcceptedAtRouteResult" /> for the response.</returns>
    [NonAction]
    public virtual AcceptedAtRouteResult AcceptedAtRoute(
      string? routeName,
      object? routeValues,
      [ActionResultObjectValue] object? value)
    {
      return new AcceptedAtRouteResult(routeName, routeValues, value);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" />.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" /> for the response.</returns>
    /// <remarks>
    /// The behavior of this method depends on the <see cref="T:Microsoft.AspNetCore.Authentication.IAuthenticationService" /> in use.
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized" /> and <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" />
    /// are among likely status results.
    /// </remarks>
    [NonAction]
    public virtual ChallengeResult Challenge() => new ChallengeResult();

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" /> with the specified authentication schemes.
    /// </summary>
    /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" /> for the response.</returns>
    /// <remarks>
    /// The behavior of this method depends on the <see cref="T:Microsoft.AspNetCore.Authentication.IAuthenticationService" /> in use.
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized" /> and <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" />
    /// are among likely status results.
    /// </remarks>
    [NonAction]
    public virtual ChallengeResult Challenge(params string[] authenticationSchemes) => new ChallengeResult((IList<string>) authenticationSchemes);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" /> with the specified <paramref name="properties" />.
    /// </summary>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the authentication
    /// challenge.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" /> for the response.</returns>
    /// <remarks>
    /// The behavior of this method depends on the <see cref="T:Microsoft.AspNetCore.Authentication.IAuthenticationService" /> in use.
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized" /> and <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" />
    /// are among likely status results.
    /// </remarks>
    [NonAction]
    public virtual ChallengeResult Challenge(AuthenticationProperties properties) => new ChallengeResult(properties);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" /> with the specified authentication schemes and
    /// <paramref name="properties" />.
    /// </summary>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the authentication
    /// challenge.</param>
    /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ChallengeResult" /> for the response.</returns>
    /// <remarks>
    /// The behavior of this method depends on the <see cref="T:Microsoft.AspNetCore.Authentication.IAuthenticationService" /> in use.
    /// <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized" /> and <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" />
    /// are among likely status results.
    /// </remarks>
    [NonAction]
    public virtual ChallengeResult Challenge(
      AuthenticationProperties properties,
      params string[] authenticationSchemes)
    {
      return new ChallengeResult((IList<string>) authenticationSchemes, properties);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> by default).
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> for the response.</returns>
    /// <remarks>
    /// Some authentication schemes, such as cookies, will convert <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> to
    /// a redirect to show a login page.
    /// </remarks>
    [NonAction]
    public virtual ForbidResult Forbid() => new ForbidResult();

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> by default) with the
    /// specified authentication schemes.
    /// </summary>
    /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> for the response.</returns>
    /// <remarks>
    /// Some authentication schemes, such as cookies, will convert <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> to
    /// a redirect to show a login page.
    /// </remarks>
    [NonAction]
    public virtual ForbidResult Forbid(params string[] authenticationSchemes) => new ForbidResult((IList<string>) authenticationSchemes);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> by default) with the
    /// specified <paramref name="properties" />.
    /// </summary>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the authentication
    /// challenge.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> for the response.</returns>
    /// <remarks>
    /// Some authentication schemes, such as cookies, will convert <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> to
    /// a redirect to show a login page.
    /// </remarks>
    [NonAction]
    public virtual ForbidResult Forbid(AuthenticationProperties properties) => new ForbidResult(properties);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> (<see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> by default) with the
    /// specified authentication schemes and <paramref name="properties" />.
    /// </summary>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the authentication
    /// challenge.</param>
    /// <param name="authenticationSchemes">The authentication schemes to challenge.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.ForbidResult" /> for the response.</returns>
    /// <remarks>
    /// Some authentication schemes, such as cookies, will convert <see cref="F:Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden" /> to
    /// a redirect to show a login page.
    /// </remarks>
    [NonAction]
    public virtual ForbidResult Forbid(
      AuthenticationProperties properties,
      params string[] authenticationSchemes)
    {
      return new ForbidResult((IList<string>) authenticationSchemes, properties);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" />.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> containing the user claims.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" /> for the response.</returns>
    [NonAction]
    public virtual SignInResult SignIn(ClaimsPrincipal principal) => new SignInResult(principal);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" /> with the specified authentication scheme.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> containing the user claims.</param>
    /// <param name="authenticationScheme">The authentication scheme to use for the sign-in operation.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" /> for the response.</returns>
    [NonAction]
    public virtual SignInResult SignIn(ClaimsPrincipal principal, string authenticationScheme) => new SignInResult(authenticationScheme, principal);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" /> with <paramref name="properties" />.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> containing the user claims.</param>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the sign-in operation.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" /> for the response.</returns>
    [NonAction]
    public virtual SignInResult SignIn(
      ClaimsPrincipal principal,
      AuthenticationProperties properties)
    {
      return new SignInResult(principal, properties);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" /> with the specified authentication scheme and
    /// <paramref name="properties" />.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> containing the user claims.</param>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the sign-in operation.</param>
    /// <param name="authenticationScheme">The authentication scheme to use for the sign-in operation.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignInResult" /> for the response.</returns>
    [NonAction]
    public virtual SignInResult SignIn(
      ClaimsPrincipal principal,
      AuthenticationProperties properties,
      string authenticationScheme)
    {
      return new SignInResult(authenticationScheme, principal, properties);
    }

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" />.
    /// </summary>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" /> for the response.</returns>
    [NonAction]
    public virtual SignOutResult SignOut() => new SignOutResult();

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" /> with <paramref name="properties" />.
    /// </summary>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the sign-out operation.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" /> for the response.</returns>
    [NonAction]
    public virtual SignOutResult SignOut(AuthenticationProperties properties) => new SignOutResult(properties);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" /> with the specified authentication schemes.
    /// </summary>
    /// <param name="authenticationSchemes">The authentication schemes to use for the sign-out operation.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" /> for the response.</returns>
    [NonAction]
    public virtual SignOutResult SignOut(params string[] authenticationSchemes) => new SignOutResult((IList<string>) authenticationSchemes);

    /// <summary>
    /// Creates a <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" /> with the specified authentication schemes and
    /// <paramref name="properties" />.
    /// </summary>
    /// <param name="properties"><see cref="T:Microsoft.AspNetCore.Authentication.AuthenticationProperties" /> used to perform the sign-out operation.</param>
    /// <param name="authenticationSchemes">The authentication scheme to use for the sign-out operation.</param>
    /// <returns>The created <see cref="T:Microsoft.AspNetCore.Mvc.SignOutResult" /> for the response.</returns>
    [NonAction]
    public virtual SignOutResult SignOut(
      AuthenticationProperties properties,
      params string[] authenticationSchemes)
    {
      return new SignOutResult((IList<string>) authenticationSchemes, properties);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using values from the controller's current
    /// <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object.</typeparam>
    /// <param name="model">The model instance to update.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public virtual Task<bool> TryUpdateModelAsync<TModel>(TModel model) where TModel : class => (object) model != null ? this.TryUpdateModelAsync<TModel>(model, string.Empty) : throw new ArgumentNullException(nameof (model));

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using values from the controller's current
    /// <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> and a <paramref name="prefix" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object.</typeparam>
    /// <param name="model">The model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the current <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" />.
    /// </param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public virtual async Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix) where TModel : class
    {
      if ((object) (TModel) model == null)
        throw new ArgumentNullException(nameof (model));
      if (prefix == null)
        throw new ArgumentNullException(nameof (prefix));
      (bool flag, CompositeValueProvider compositeValueProvider) = await CompositeValueProvider.TryCreateAsync((ActionContext) this.ControllerContext, this.ControllerContext.ValueProviderFactories);
      return flag && await this.TryUpdateModelAsync<TModel>(model, prefix, (IValueProvider) compositeValueProvider);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using the <paramref name="valueProvider" /> and a
    /// <paramref name="prefix" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object.</typeparam>
    /// <param name="model">The model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the <paramref name="valueProvider" />.
    /// </param>
    /// <param name="valueProvider">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> used for looking up values.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public virtual Task<bool> TryUpdateModelAsync<TModel>(
      TModel model,
      string prefix,
      IValueProvider valueProvider)
      where TModel : class
    {
      if ((object) model == null)
        throw new ArgumentNullException(nameof (model));
      if (prefix == null)
        throw new ArgumentNullException(nameof (prefix));
      if (valueProvider == null)
        throw new ArgumentNullException(nameof (valueProvider));
      return ModelBindingHelper.TryUpdateModelAsync<TModel>(model, prefix, (ActionContext) this.ControllerContext, this.MetadataProvider, this.ModelBinderFactory, valueProvider, this.ObjectValidator);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using values from the controller's current
    /// <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> and a <paramref name="prefix" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object.</typeparam>
    /// <param name="model">The model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the current <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" />.
    /// </param>
    /// <param name="includeExpressions"> <see cref="T:System.Linq.Expressions.Expression" />(s) which represent top-level properties
    /// which need to be included for the current model.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public async Task<bool> TryUpdateModelAsync<TModel>(
      TModel model,
      string prefix,
      params Expression<Func<TModel, object?>>[] includeExpressions)
      where TModel : class
    {
      if ((object) (TModel) model == null)
        throw new ArgumentNullException(nameof (model));
      if (includeExpressions == null)
        throw new ArgumentNullException(nameof (includeExpressions));
      (bool flag, CompositeValueProvider compositeValueProvider) = await CompositeValueProvider.TryCreateAsync((ActionContext) this.ControllerContext, this.ControllerContext.ValueProviderFactories);
      return flag && await ModelBindingHelper.TryUpdateModelAsync<TModel>(model, prefix, (ActionContext) this.ControllerContext, this.MetadataProvider, this.ModelBinderFactory, (IValueProvider) compositeValueProvider, this.ObjectValidator, includeExpressions);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using values from the controller's current
    /// <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> and a <paramref name="prefix" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object.</typeparam>
    /// <param name="model">The model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the current <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" />.
    /// </param>
    /// <param name="propertyFilter">A predicate which can be used to filter properties at runtime.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public async Task<bool> TryUpdateModelAsync<TModel>(
      TModel model,
      string prefix,
      Func<ModelMetadata, bool> propertyFilter)
      where TModel : class
    {
      if ((object) (TModel) model == null)
        throw new ArgumentNullException(nameof (model));
      if (propertyFilter == null)
        throw new ArgumentNullException(nameof (propertyFilter));
      (bool flag, CompositeValueProvider compositeValueProvider) = await CompositeValueProvider.TryCreateAsync((ActionContext) this.ControllerContext, this.ControllerContext.ValueProviderFactories);
      return flag && await ModelBindingHelper.TryUpdateModelAsync<TModel>(model, prefix, (ActionContext) this.ControllerContext, this.MetadataProvider, this.ModelBinderFactory, (IValueProvider) compositeValueProvider, this.ObjectValidator, propertyFilter);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using the <paramref name="valueProvider" /> and a
    /// <paramref name="prefix" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object.</typeparam>
    /// <param name="model">The model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the <paramref name="valueProvider" />.
    /// </param>
    /// <param name="valueProvider">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> used for looking up values.</param>
    /// <param name="includeExpressions"> <see cref="T:System.Linq.Expressions.Expression" />(s) which represent top-level properties
    /// which need to be included for the current model.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public Task<bool> TryUpdateModelAsync<TModel>(
      TModel model,
      string prefix,
      IValueProvider valueProvider,
      params Expression<Func<TModel, object?>>[] includeExpressions)
      where TModel : class
    {
      if ((object) model == null)
        throw new ArgumentNullException(nameof (model));
      if (valueProvider == null)
        throw new ArgumentNullException(nameof (valueProvider));
      if (includeExpressions == null)
        throw new ArgumentNullException(nameof (includeExpressions));
      return ModelBindingHelper.TryUpdateModelAsync<TModel>(model, prefix, (ActionContext) this.ControllerContext, this.MetadataProvider, this.ModelBinderFactory, valueProvider, this.ObjectValidator, includeExpressions);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using the <paramref name="valueProvider" /> and a
    /// <paramref name="prefix" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the model object.</typeparam>
    /// <param name="model">The model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the <paramref name="valueProvider" />.
    /// </param>
    /// <param name="valueProvider">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> used for looking up values.</param>
    /// <param name="propertyFilter">A predicate which can be used to filter properties at runtime.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public Task<bool> TryUpdateModelAsync<TModel>(
      TModel model,
      string prefix,
      IValueProvider valueProvider,
      Func<ModelMetadata, bool> propertyFilter)
      where TModel : class
    {
      if ((object) model == null)
        throw new ArgumentNullException(nameof (model));
      if (valueProvider == null)
        throw new ArgumentNullException(nameof (valueProvider));
      if (propertyFilter == null)
        throw new ArgumentNullException(nameof (propertyFilter));
      return ModelBindingHelper.TryUpdateModelAsync<TModel>(model, prefix, (ActionContext) this.ControllerContext, this.MetadataProvider, this.ModelBinderFactory, valueProvider, this.ObjectValidator, propertyFilter);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using values from the controller's current
    /// <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> and a <paramref name="prefix" />.
    /// </summary>
    /// <param name="model">The model instance to update.</param>
    /// <param name="modelType">The type of model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the current <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" />.
    /// </param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public virtual async Task<bool> TryUpdateModelAsync(
      object model,
      Type modelType,
      string prefix)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      if (modelType == (Type) null)
        throw new ArgumentNullException(nameof (modelType));
      (bool flag, CompositeValueProvider compositeValueProvider) = await CompositeValueProvider.TryCreateAsync((ActionContext) this.ControllerContext, this.ControllerContext.ValueProviderFactories);
      return flag && await ModelBindingHelper.TryUpdateModelAsync(model, modelType, prefix, (ActionContext) this.ControllerContext, this.MetadataProvider, this.ModelBinderFactory, (IValueProvider) compositeValueProvider, this.ObjectValidator);
    }

    /// <summary>
    /// Updates the specified <paramref name="model" /> instance using the <paramref name="valueProvider" /> and a
    /// <paramref name="prefix" />.
    /// </summary>
    /// <param name="model">The model instance to update.</param>
    /// <param name="modelType">The type of model instance to update.</param>
    /// <param name="prefix">The prefix to use when looking up values in the <paramref name="valueProvider" />.
    /// </param>
    /// <param name="valueProvider">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider" /> used for looking up values.</param>
    /// <param name="propertyFilter">A predicate which can be used to filter properties at runtime.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that on completion returns <c>true</c> if the update is successful.</returns>
    [NonAction]
    public Task<bool> TryUpdateModelAsync(
      object model,
      Type modelType,
      string prefix,
      IValueProvider valueProvider,
      Func<ModelMetadata, bool> propertyFilter)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      if (modelType == (Type) null)
        throw new ArgumentNullException(nameof (modelType));
      if (valueProvider == null)
        throw new ArgumentNullException(nameof (valueProvider));
      if (propertyFilter == null)
        throw new ArgumentNullException(nameof (propertyFilter));
      return ModelBindingHelper.TryUpdateModelAsync(model, modelType, prefix, (ActionContext) this.ControllerContext, this.MetadataProvider, this.ModelBinderFactory, valueProvider, this.ObjectValidator, propertyFilter);
    }

    /// <summary>
    /// Validates the specified <paramref name="model" /> instance.
    /// </summary>
    /// <param name="model">The model to validate.</param>
    /// <returns><c>true</c> if the <see cref="P:Microsoft.AspNetCore.Mvc.ControllerBase.ModelState" /> is valid; <c>false</c> otherwise.</returns>
    [NonAction]
    public virtual bool TryValidateModel(object model) => model != null ? this.TryValidateModel(model, (string) null) : throw new ArgumentNullException(nameof (model));

    /// <summary>
    /// Validates the specified <paramref name="model" /> instance.
    /// </summary>
    /// <param name="model">The model to validate.</param>
    /// <param name="prefix">The key to use when looking up information in <see cref="P:Microsoft.AspNetCore.Mvc.ControllerBase.ModelState" />.
    /// </param>
    /// <returns><c>true</c> if the <see cref="P:Microsoft.AspNetCore.Mvc.ControllerBase.ModelState" /> is valid;<c>false</c> otherwise.</returns>
    [NonAction]
    public virtual bool TryValidateModel(object model, string? prefix)
    {
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      this.ObjectValidator.Validate((ActionContext) this.ControllerContext, (ValidationStateDictionary) null, prefix ?? string.Empty, model);
      return this.ModelState.IsValid;
    }
  }
}
