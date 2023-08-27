// Decompiled with JetBrains decompiler
// Type: FluentResults.Error
// Assembly: FluentResults, Version=3.15.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 84CCAC16-9EE0-4087-BDB7-D78E6096FE38
// Assembly location: /home/phetoush/.nuget/packages/fluentresults/3.15.2/lib/netstandard2.1/FluentResults.dll
// XML documentation location: /home/phetoush/.nuget/packages/fluentresults/3.15.2/lib/netstandard2.1/FluentResults.xml

using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults
{
  /// <summary>Objects from Error class cause a failed result</summary>
  public class Error : IError, IReason
  {
    /// <summary>Message of the error</summary>
    public string Message { get; protected set; }

    /// <summary>Metadata of the error</summary>
    public Dictionary<string, object> Metadata { get; }

    /// <summary>Get the reasons of an error</summary>
    public List<IError> Reasons { get; }

    protected Error()
    {
      this.Metadata = new Dictionary<string, object>();
      this.Reasons = new List<IError>();
    }

    /// <summary>
    /// Creates a new instance of <see cref="T:FluentResults.Error" />
    /// </summary>
    /// <param name="message">Discription of the error</param>
    public Error(string message)
      : this()
    {
      this.Message = message;
    }

    /// <summary>
    /// Creates a new instance of <see cref="T:FluentResults.Error" />
    /// </summary>
    /// <param name="message">Discription of the error</param>
    /// <param name="causedBy">The root cause of the <see cref="T:FluentResults.Error" /></param>
    public Error(string message, IError causedBy)
      : this(message)
    {
      if (causedBy == null)
        throw new ArgumentNullException(nameof (causedBy));
      this.Reasons.Add(causedBy);
    }

    /// <summary>Set the root cause of the error</summary>
    public Error CausedBy(IError error)
    {
      if (error == null)
        throw new ArgumentNullException(nameof (error));
      this.Reasons.Add(error);
      return this;
    }

    /// <summary>Set the root cause of the error</summary>
    public Error CausedBy(Exception exception)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      this.Reasons.Add((IError) Result.Settings.ExceptionalErrorFactory((string) null, exception));
      return this;
    }

    /// <summary>Set the root cause of the error</summary>
    public Error CausedBy(string message, Exception exception)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      this.Reasons.Add((IError) Result.Settings.ExceptionalErrorFactory(message, exception));
      return this;
    }

    /// <summary>Set the root cause of the error</summary>
    public Error CausedBy(string message)
    {
      this.Reasons.Add(Result.Settings.ErrorFactory(message));
      return this;
    }

    /// <summary>Set the root cause of the error</summary>
    public Error CausedBy(IEnumerable<IError> errors)
    {
      if (errors == null)
        throw new ArgumentNullException(nameof (errors));
      this.Reasons.AddRange(errors);
      return this;
    }

    /// <summary>Set the root cause of the error</summary>
    public Error CausedBy(IEnumerable<string> errors)
    {
      if (errors == null)
        throw new ArgumentNullException(nameof (errors));
      this.Reasons.AddRange(errors.Select<string, IError>((Func<string, IError>) (errorMessage => Result.Settings.ErrorFactory(errorMessage))));
      return this;
    }

    /// <summary>Set the metadata</summary>
    public Error WithMetadata(string metadataName, object metadataValue)
    {
      this.Metadata.Add(metadataName, metadataValue);
      return this;
    }

    /// <summary>Set the metadata</summary>
    public Error WithMetadata(Dictionary<string, object> metadata)
    {
      foreach (KeyValuePair<string, object> keyValuePair in metadata)
        this.Metadata.Add(keyValuePair.Key, keyValuePair.Value);
      return this;
    }

    public override string ToString() => new ReasonStringBuilder().WithReasonType(this.GetType()).WithInfo("Message", this.Message).WithInfo("Metadata", string.Join<KeyValuePair<string, object>>("; ", (IEnumerable<KeyValuePair<string, object>>) this.Metadata)).WithInfo("Reasons", ReasonFormat.ErrorReasonsToString((IReadOnlyCollection<IError>) this.Reasons)).Build();
  }
}
