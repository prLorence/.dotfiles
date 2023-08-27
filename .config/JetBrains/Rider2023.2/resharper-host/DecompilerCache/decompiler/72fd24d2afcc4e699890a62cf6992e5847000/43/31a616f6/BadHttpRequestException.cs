// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Http.BadHttpRequestException
// Assembly: Microsoft.AspNetCore.Http.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 72FD24D2-AFCC-4E69-9890-A62CF6992E58
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.AspNetCore.Http.Abstractions.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.AspNetCore.Http.Abstractions.xml

using System;
using System.IO;


#nullable enable
namespace Microsoft.AspNetCore.Http
{
  /// <summary>Represents an HTTP request error</summary>
  public class BadHttpRequestException : IOException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Http.BadHttpRequestException" /> class.
    /// </summary>
    /// <param name="message">The message to associate with this exception.</param>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    public BadHttpRequestException(string message, int statusCode)
      : base(message)
    {
      this.StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Http.BadHttpRequestException" /> class with the <see cref="P:Microsoft.AspNetCore.Http.BadHttpRequestException.StatusCode" /> set to 400 Bad Request.
    /// </summary>
    /// <param name="message">The message to associate with this exception</param>
    public BadHttpRequestException(string message)
      : base(message)
    {
      this.StatusCode = 400;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Http.BadHttpRequestException" /> class.
    /// </summary>
    /// <param name="message">The message to associate with this exception.</param>
    /// <param name="statusCode">The HTTP status code to associate with this exception.</param>
    /// <param name="innerException">The inner exception to associate with this exception</param>
    public BadHttpRequestException(string message, int statusCode, Exception innerException)
      : base(message, innerException)
    {
      this.StatusCode = statusCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Http.BadHttpRequestException" /> class with the <see cref="P:Microsoft.AspNetCore.Http.BadHttpRequestException.StatusCode" /> set to 400 Bad Request.
    /// </summary>
    /// <param name="message">The message to associate with this exception</param>
    /// <param name="innerException">The inner exception to associate with this exception</param>
    public BadHttpRequestException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.StatusCode = 400;
    }

    /// <summary>Gets the HTTP status code for this exception.</summary>
    public int StatusCode { get; }
  }
}
