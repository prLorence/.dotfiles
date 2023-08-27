// Decompiled with JetBrains decompiler
// Type: System.Exception
// Assembly: System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 1228D55B-FDDD-436D-8525-FDF4491B3FE9
// Assembly location: /usr/lib/dotnet/shared/Microsoft.NETCore.App/7.0.9/System.Private.CoreLib.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.NETCore.App.Ref/7.0.9/ref/net7.0/System.Runtime.xml

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;


#nullable enable
namespace System
{
  /// <summary>Represents errors that occur during application execution.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class Exception : ISerializable
  {

    #nullable disable
    private MethodBase _exceptionMethod;
    internal string _message;
    private IDictionary _data;
    private readonly Exception _innerException;
    private string _helpURL;
    private byte[] _stackTrace;
    private byte[] _watsonBuckets;
    private string _stackTraceString;
    private string _remoteStackTraceString;
    private readonly object[] _dynamicMethods;
    private string _source;
    private UIntPtr _ipForWatsonBuckets;
    private readonly IntPtr _xptrs;
    private readonly int _xcode = -532462766;
    private int _HResult;
    private const int _COMPlusExceptionCode = -532462766;
    private protected const string InnerExceptionPrefix = " ---> ";

    private IDictionary CreateDataContainer() => Exception.IsImmutableAgileException(this) ? (IDictionary) new EmptyReadOnlyDictionaryInternal() : (IDictionary) new ListDictionaryInternal();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool IsImmutableAgileException(Exception e);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern IRuntimeMethodInfo GetMethodFromStackTrace(object stackTrace);

    private MethodBase GetExceptionMethodFromStackTrace()
    {
      IRuntimeMethodInfo methodFromStackTrace = Exception.GetMethodFromStackTrace((object) this._stackTrace);
      return methodFromStackTrace == null ? (MethodBase) null : RuntimeType.GetMethodBase(methodFromStackTrace);
    }


    #nullable enable
    /// <summary>Gets the method that throws the current exception.</summary>
    /// <returns>The <see cref="T:System.Reflection.MethodBase" /> that threw the current exception.</returns>
    public MethodBase? TargetSite
    {
      [RequiresUnreferencedCode("Metadata for the method might be incomplete or removed")] get
      {
        if (this._exceptionMethod != (MethodBase) null)
          return this._exceptionMethod;
        if (this._stackTrace == null)
          return (MethodBase) null;
        this._exceptionMethod = this.GetExceptionMethodFromStackTrace();
        return this._exceptionMethod;
      }
    }

    [System.Runtime.Serialization.OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
      this._stackTrace = (byte[]) null;
      this._ipForWatsonBuckets = UIntPtr.Zero;
    }

    internal void InternalPreserveStackTrace()
    {
      string source = this.Source;
      string stackTrace = this.StackTrace;
      if (!string.IsNullOrEmpty(stackTrace))
        this._remoteStackTraceString = stackTrace + "\n";
      this._stackTrace = (byte[]) null;
      this._stackTraceString = (string) null;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void PrepareForForeignExceptionRaise();


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetStackTracesDeepCopy(
      Exception exception,
      out byte[] currentStackTrace,
      out object[] dynamicMethodArray);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SaveStackTracesFromDeepCopy(
      Exception exception,
      byte[] currentStackTrace,
      object[] dynamicMethodArray);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern uint GetExceptionCount();

    internal void RestoreDispatchState(in Exception.DispatchState dispatchState)
    {
      if (Exception.IsImmutableAgileException(this))
        return;
      byte[] currentStackTrace = (byte[]) dispatchState.StackTrace?.Clone();
      object[] dynamicMethodArray = (object[]) dispatchState.DynamicMethods?.Clone();
      this._watsonBuckets = dispatchState.WatsonBuckets;
      this._ipForWatsonBuckets = dispatchState.IpForWatsonBuckets;
      this._remoteStackTraceString = dispatchState.RemoteStackTrace;
      Exception.SaveStackTracesFromDeepCopy(this, currentStackTrace, dynamicMethodArray);
      this._stackTraceString = (string) null;
      Exception.PrepareForForeignExceptionRaise();
    }

    private bool HasBeenThrown => this._stackTrace != null;


    #nullable enable
    private object? SerializationWatsonBuckets => (object) this._watsonBuckets;


    #nullable disable
    internal static string GetMessageFromNativeResources(Exception.ExceptionMessageKind kind)
    {
      string s = (string) null;
      Exception.GetMessageFromNativeResources(kind, new StringHandleOnStack(ref s));
      return s;
    }

    [LibraryImport("QCall", EntryPoint = "ExceptionNative_GetMessageFromNativeResources")]
    [DllImport("QCall", EntryPoint = "ExceptionNative_GetMessageFromNativeResources")]
    private static extern void GetMessageFromNativeResources(
      Exception.ExceptionMessageKind kind,
      StringHandleOnStack retMesg);

    internal Exception.DispatchState CaptureDispatchState()
    {
      byte[] currentStackTrace;
      object[] dynamicMethodArray;
      Exception.GetStackTracesDeepCopy(this, out currentStackTrace, out dynamicMethodArray);
      return new Exception.DispatchState(currentStackTrace, dynamicMethodArray, this._remoteStackTraceString, this._ipForWatsonBuckets, this._watsonBuckets);
    }

    private bool CanSetRemoteStackTrace()
    {
      if (Exception.IsImmutableAgileException(this))
        return false;
      if (this._stackTrace != null || this._stackTraceString != null || this._remoteStackTraceString != null)
        ThrowHelper.ThrowInvalidOperationException();
      return true;
    }

    internal string GetHelpContext(out uint helpContext)
    {
      helpContext = 0U;
      string text = this.HelpLink;
      int length;
      if (text == null || (length = text.LastIndexOf('#')) == -1)
        return text;
      int index = length + 1;
      while (index < text.Length && !char.IsWhiteSpace(text[index]))
        ++index;
      if (uint.TryParse(text.AsSpan(length + 1, index - length - 1), out helpContext))
        text = text.Substring(0, length);
      return text;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class.</summary>
    public Exception() => this._HResult = -2146233088;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    public Exception(string? message)
      : this()
    {
      this._message = message;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public Exception(string? message, Exception? innerException)
      : this()
    {
      this._message = message;
      this._innerException = innerException;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with serialized data.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
    protected Exception(SerializationInfo info, StreamingContext context)
    {
      ArgumentNullException.ThrowIfNull((object) info, nameof (info));
      this._message = info.GetString(nameof (Message));
      this._data = (IDictionary) info.GetValueNoThrow(nameof (Data), typeof (IDictionary));
      this._innerException = (Exception) info.GetValue(nameof (InnerException), typeof (Exception));
      this._helpURL = info.GetString("HelpURL");
      this._stackTraceString = info.GetString("StackTraceString");
      this._remoteStackTraceString = info.GetString("RemoteStackTraceString");
      this._HResult = info.GetInt32(nameof (HResult));
      this._source = info.GetString(nameof (Source));
      this.RestoreRemoteStackTrace(info, context);
    }

    /// <summary>Gets a message that describes the current exception.</summary>
    /// <returns>The error message that explains the reason for the exception, or an empty string ("").</returns>
    public virtual string Message => this._message ?? SR.Format(SR.Exception_WasThrown, (object) this.GetClassName());

    /// <summary>Gets a collection of key/value pairs that provide additional user-defined information about the exception.</summary>
    /// <returns>An object that implements the <see cref="T:System.Collections.IDictionary" /> interface and contains a collection of user-defined key/value pairs. The default is an empty collection.</returns>
    public virtual IDictionary Data => this._data ?? (this._data = this.CreateDataContainer());


    #nullable disable
    private string GetClassName() => this.GetType().ToString();


    #nullable enable
    /// <summary>When overridden in a derived class, returns the <see cref="T:System.Exception" /> that is the root cause of one or more subsequent exceptions.</summary>
    /// <returns>The first exception thrown in a chain of exceptions. If the <see cref="P:System.Exception.InnerException" /> property of the current exception is a null reference (<see langword="Nothing" /> in Visual Basic), this property returns the current exception.</returns>
    public virtual Exception GetBaseException()
    {
      Exception innerException = this.InnerException;
      Exception baseException = this;
      for (; innerException != null; innerException = innerException.InnerException)
        baseException = innerException;
      return baseException;
    }

    /// <summary>Gets the <see cref="T:System.Exception" /> instance that caused the current exception.</summary>
    /// <returns>An object that describes the error that caused the current exception. The <see cref="P:System.Exception.InnerException" /> property returns the same value as was passed into the <see cref="M:System.Exception.#ctor(System.String,System.Exception)" /> constructor, or <see langword="null" /> if the inner exception value was not supplied to the constructor. This property is read-only.</returns>
    public Exception? InnerException => this._innerException;

    /// <summary>Gets or sets a link to the help file associated with this exception.</summary>
    /// <returns>The Uniform Resource Name (URN) or Uniform Resource Locator (URL).</returns>
    public virtual string? HelpLink
    {
      get => this._helpURL;
      set => this._helpURL = value;
    }

    /// <summary>Gets or sets the name of the application or the object that causes the error.</summary>
    /// <exception cref="T:System.ArgumentException">The object must be a runtime <see cref="N:System.Reflection" /> object.</exception>
    /// <returns>The name of the application or the object that causes the error.</returns>
    public virtual string? Source
    {
      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "The API will return <unknown> if the metadata for current method cannot be established.")] get => this._source ?? (this._source = this.HasBeenThrown ? this.TargetSite?.Module.Assembly.GetName().Name ?? "<unknown>" : (string) null);
      set => this._source = value;
    }

    /// <summary>When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is a null reference (<see langword="Nothing" /> in Visual Basic).</exception>
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      ArgumentNullException.ThrowIfNull((object) info, nameof (info));
      if (this._source == null)
        this._source = this.Source;
      info.AddValue("ClassName", (object) this.GetClassName(), typeof (string));
      info.AddValue("Message", (object) this._message, typeof (string));
      info.AddValue("Data", (object) this._data, typeof (IDictionary));
      info.AddValue("InnerException", (object) this._innerException, typeof (Exception));
      info.AddValue("HelpURL", (object) this._helpURL, typeof (string));
      info.AddValue("StackTraceString", (object) this.SerializationStackTraceString, typeof (string));
      info.AddValue("RemoteStackTraceString", (object) this._remoteStackTraceString, typeof (string));
      info.AddValue("RemoteStackIndex", (object) 0, typeof (int));
      info.AddValue("ExceptionMethod", (object) null, typeof (string));
      info.AddValue("HResult", this._HResult);
      info.AddValue("Source", (object) this._source, typeof (string));
      info.AddValue("WatsonBuckets", this.SerializationWatsonBuckets, typeof (byte[]));
    }

    /// <summary>Creates and returns a string representation of the current exception.</summary>
    /// <returns>A string representation of the current exception.</returns>
    public override string ToString()
    {
      string className = this.GetClassName();
      string message = this.Message;
      string source = this._innerException?.ToString() ?? "";
      string innerExceptionStack = SR.Exception_EndOfInnerExceptionStack;
      string stackTrace = this.StackTrace;
      int length = className.Length;
      if (!string.IsNullOrEmpty(message))
        checked { length += 2 + message.Length; }
      if (this._innerException != null)
        checked { length += "\n".Length + " ---> ".Length + source.Length + "\n".Length + 3 + innerExceptionStack.Length; }
      if (stackTrace != null)
        checked { length += "\n".Length + stackTrace.Length; }
      string str = string.FastAllocateString(length);
      Span<char> dest = new Span<char>(ref str.GetRawStringData(), str.Length);
      Write(className, ref dest);
      if (!string.IsNullOrEmpty(message))
      {
        Write(": ", ref dest);
        Write(message, ref dest);
      }
      if (this._innerException != null)
      {
        Write("\n", ref dest);
        Write(" ---> ", ref dest);
        Write(source, ref dest);
        Write("\n", ref dest);
        Write("   ", ref dest);
        Write(innerExceptionStack, ref dest);
      }
      if (stackTrace != null)
      {
        Write("\n", ref dest);
        Write(stackTrace, ref dest);
      }
      return str;


      #nullable disable
      static void Write(string source, ref Span<char> dest)
      {
        source.CopyTo(dest);
        dest = dest.Slice(source.Length);
      }
    }


    #nullable enable
    /// <summary>Occurs when an exception is serialized to create an exception state object that contains serialized data about the exception.</summary>
    [Obsolete("BinaryFormatter serialization is obsolete and should not be used. See https://aka.ms/binaryformatter for more information.", DiagnosticId = "SYSLIB0011", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    protected event EventHandler<SafeSerializationEventArgs>? SerializeObjectState
    {
      add => throw new PlatformNotSupportedException(SR.PlatformNotSupported_SecureBinarySerialization);
      remove => throw new PlatformNotSupportedException(SR.PlatformNotSupported_SecureBinarySerialization);
    }

    /// <summary>Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.</summary>
    /// <returns>The HRESULT value.</returns>
    public int HResult
    {
      get => this._HResult;
      set => this._HResult = value;
    }

    /// <summary>Gets the runtime type of the current instance.</summary>
    /// <returns>A <see cref="T:System.Type" /> object that represents the exact runtime type of the current instance.</returns>
    public new Type GetType() => base.GetType();


    #nullable disable
    private void RestoreRemoteStackTrace(SerializationInfo info, StreamingContext context)
    {
      this._watsonBuckets = (byte[]) info.GetValueNoThrow("WatsonBuckets", typeof (byte[]));
      if (context.State != StreamingContextStates.CrossAppDomain)
        return;
      this._remoteStackTraceString += this._stackTraceString;
      this._stackTraceString = (string) null;
    }


    #nullable enable
    /// <summary>Gets a string representation of the immediate frames on the call stack.</summary>
    /// <returns>A string that describes the immediate frames of the call stack.</returns>
    public virtual string? StackTrace
    {
      get
      {
        string stackTraceString1 = this._stackTraceString;
        string stackTraceString2 = this._remoteStackTraceString;
        if (stackTraceString1 != null)
          return stackTraceString2 + stackTraceString1;
        return !this.HasBeenThrown ? stackTraceString2 : stackTraceString2 + this.GetStackTrace();
      }
    }


    #nullable disable
    private string GetStackTrace() => new System.Diagnostics.StackTrace(this, true).ToString(System.Diagnostics.StackTrace.TraceFormat.Normal);

    [StackTraceHidden]
    internal void SetCurrentStackTrace()
    {
      if (!this.CanSetRemoteStackTrace())
        return;
      StringBuilder sb = new StringBuilder(256);
      new System.Diagnostics.StackTrace(true).ToString(System.Diagnostics.StackTrace.TraceFormat.TrailingNewLine, sb);
      sb.AppendLine(SR.Exception_EndStackTraceFromPreviousThrow);
      this._remoteStackTraceString = sb.ToString();
    }

    internal void SetRemoteStackTrace(string stackTrace)
    {
      if (!this.CanSetRemoteStackTrace())
        return;
      this._remoteStackTraceString = stackTrace + "\n" + SR.Exception_EndStackTraceFromPreviousThrow + "\n";
    }


    #nullable enable
    private string? SerializationStackTraceString
    {
      get
      {
        string stackTraceString = this._stackTraceString;
        if (stackTraceString == null && this.HasBeenThrown)
          stackTraceString = this.GetStackTrace();
        return stackTraceString;
      }
    }


    #nullable disable
    internal enum ExceptionMessageKind
    {
      ThreadAbort = 1,
      ThreadInterrupted = 2,
      OutOfMemory = 3,
    }

    internal readonly struct DispatchState
    {
      public readonly byte[] StackTrace;
      public readonly object[] DynamicMethods;
      public readonly string RemoteStackTrace;
      public readonly UIntPtr IpForWatsonBuckets;
      public readonly byte[] WatsonBuckets;

      public DispatchState(
        byte[] stackTrace,
        object[] dynamicMethods,
        string remoteStackTrace,
        UIntPtr ipForWatsonBuckets,
        byte[] watsonBuckets)
      {
        this.StackTrace = stackTrace;
        this.DynamicMethods = dynamicMethods;
        this.RemoteStackTrace = remoteStackTrace;
        this.IpForWatsonBuckets = ipForWatsonBuckets;
        this.WatsonBuckets = watsonBuckets;
      }
    }
  }
}
