// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.Task`1
// Assembly: System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 1228D55B-FDDD-436D-8525-FDF4491B3FE9
// Assembly location: /usr/lib/dotnet/shared/Microsoft.NETCore.App/7.0.9/System.Private.CoreLib.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.NETCore.App.Ref/7.0.9/ref/net7.0/System.Runtime.xml

using System.Diagnostics;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Threading.Tasks
{
  /// <summary>Represents an asynchronous operation that can return a value.</summary>
  /// <typeparam name="TResult">The type of the result produced by this <see cref="T:System.Threading.Tasks.Task`1" />.</typeparam>
  [DebuggerTypeProxy(typeof (SystemThreadingTasks_FutureDebugView<>))]
  [DebuggerDisplay("Id = {Id}, Status = {Status}, Method = {DebuggerDisplayMethodDescription}, Result = {DebuggerDisplayResultDescription}")]
  public class Task<TResult> : Task
  {

    #nullable disable
    internal static readonly Task<TResult> s_defaultResultTask = TaskCache.CreateCacheableTask<TResult>(default (TResult));
    private static TaskFactory<TResult> s_Factory;
    internal TResult m_result;

    internal Task()
    {
    }

    internal Task(object state, TaskCreationOptions options)
      : base(state, options, true)
    {
    }

    internal Task(TResult result)
      : base(false, TaskCreationOptions.None, new CancellationToken())
    {
      this.m_result = result;
    }

    internal Task(
      bool canceled,
      TResult result,
      TaskCreationOptions creationOptions,
      CancellationToken ct)
      : base(canceled, creationOptions, ct)
    {
      if (canceled)
        return;
      this.m_result = result;
    }


    #nullable enable
    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(Func<TResult> function)
      : this(function, (Task) null, new CancellationToken(), TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to this task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(Func<TResult> function, CancellationToken cancellationToken)
      : this(function, (Task) null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and creation options.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(Func<TResult> function, TaskCreationOptions creationOptions)
      : this(function, Task.InternalCurrentIfAttached(creationOptions), new CancellationToken(), creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and creation options.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(
      Func<TResult> function,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions)
      : this(function, Task.InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified function and state.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <param name="state">An object representing data to be used by the action.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(Func<object?, TResult> function, object? state)
      : this((Delegate) function, state, (Task) null, new CancellationToken(), TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <param name="state">An object representing data to be used by the function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to the new task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(Func<object?, TResult> function, object? state, CancellationToken cancellationToken)
      : this((Delegate) function, state, (Task) null, cancellationToken, TaskCreationOptions.None, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <param name="state">An object representing data to be used by the function.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(Func<object?, TResult> function, object? state, TaskCreationOptions creationOptions)
      : this((Delegate) function, state, Task.InternalCurrentIfAttached(creationOptions), new CancellationToken(), creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }

    /// <summary>Initializes a new <see cref="T:System.Threading.Tasks.Task`1" /> with the specified action, state, and options.</summary>
    /// <param name="function">The delegate that represents the code to execute in the task. When the function has completed, the task's <see cref="P:System.Threading.Tasks.Task`1.Result" /> property will be set to return the result value of the function.</param>
    /// <param name="state">An object representing data to be used by the function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to be assigned to the new task.</param>
    /// <param name="creationOptions">The <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> used to customize the task's behavior.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="creationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskCreationOptions" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="function" /> argument is <see langword="null" />.</exception>
    public Task(
      Func<object?, TResult> function,
      object? state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions)
      : this((Delegate) function, state, Task.InternalCurrentIfAttached(creationOptions), cancellationToken, creationOptions, InternalTaskOptions.None, (TaskScheduler) null)
    {
    }


    #nullable disable
    internal Task(
      Func<TResult> valueSelector,
      Task parent,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
      : base((Delegate) valueSelector, (object) null, parent, cancellationToken, creationOptions, internalOptions, scheduler)
    {
    }

    internal Task(
      Delegate valueSelector,
      object state,
      Task parent,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
      : base(valueSelector, state, parent, cancellationToken, creationOptions, internalOptions, scheduler)
    {
    }

    internal static Task<TResult> StartNew(
      Task parent,
      Func<TResult> function,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
    {
      if (function == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.function);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      Task<TResult> task = new Task<TResult>(function, parent, cancellationToken, creationOptions, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler);
      task.ScheduleAndStart(false);
      return task;
    }

    internal static Task<TResult> StartNew(
      Task parent,
      Func<object, TResult> function,
      object state,
      CancellationToken cancellationToken,
      TaskCreationOptions creationOptions,
      InternalTaskOptions internalOptions,
      TaskScheduler scheduler)
    {
      if (function == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.function);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      Task<TResult> task = new Task<TResult>((Delegate) function, state, parent, cancellationToken, creationOptions, internalOptions | InternalTaskOptions.QueuedByRuntime, scheduler);
      task.ScheduleAndStart(false);
      return task;
    }


    #nullable enable
    private string DebuggerDisplayResultDescription => !this.IsCompletedSuccessfully ? SR.TaskT_DebuggerNoResult : this.m_result?.ToString() ?? "";

    private string DebuggerDisplayMethodDescription => this.m_action?.Method.ToString() ?? "{null}";


    #nullable disable
    internal bool TrySetResult(TResult result)
    {
      bool flag = false;
      if (this.AtomicStateUpdate(67108864, 90177536))
      {
        this.m_result = result;
        Interlocked.Exchange(ref this.m_stateFlags, this.m_stateFlags | 16777216);
        Task.ContingentProperties contingentProperties = this.m_contingentProperties;
        if (contingentProperties != null)
        {
          this.NotifyParentIfPotentiallyAttachedTask();
          contingentProperties.SetCompleted();
        }
        this.FinishContinuations();
        flag = true;
      }
      return flag;
    }

    internal void DangerousSetResult(TResult result)
    {
      if (this.m_contingentProperties?.m_parent != null)
      {
        this.TrySetResult(result);
      }
      else
      {
        this.m_result = result;
        this.m_stateFlags |= 16777216;
      }
    }


    #nullable enable
    /// <summary>Gets the result value of this <see cref="T:System.Threading.Tasks.Task`1" />.</summary>
    /// <exception cref="T:System.AggregateException">The task was canceled. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains a <see cref="T:System.Threading.Tasks.TaskCanceledException" /> object.
    /// 
    /// -or-
    /// 
    /// An exception was thrown during the execution of the task. The <see cref="P:System.AggregateException.InnerExceptions" /> collection contains information about the exception or exceptions.</exception>
    /// <returns>The result value of this <see cref="T:System.Threading.Tasks.Task`1" />, which is of the same type as the task's type parameter.</returns>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public TResult Result => !this.IsWaitNotificationEnabledOrNotRanToCompletion ? this.m_result : this.GetResultCore(true);

    internal TResult ResultOnSuccess => this.m_result;


    #nullable disable
    internal TResult GetResultCore(bool waitCompletionNotification)
    {
      if (!this.IsCompleted)
        this.InternalWait(-1, new CancellationToken());
      if (waitCompletionNotification)
        this.NotifyDebuggerOfWaitCompletionIfNecessary();
      if (!this.IsCompletedSuccessfully)
        this.ThrowIfExceptional(true);
      return this.m_result;
    }


    #nullable enable
    /// <summary>Gets a factory method for creating and configuring <see cref="T:System.Threading.Tasks.Task`1" /> instances.</summary>
    /// <returns>A factory object that can create a variety of <see cref="T:System.Threading.Tasks.Task`1" /> objects.</returns>
    public static TaskFactory<TResult> Factory => Volatile.Read<TaskFactory<TResult>>(ref Task<TResult>.s_Factory) ?? Interlocked.CompareExchange<TaskFactory<TResult>>(ref Task<TResult>.s_Factory, new TaskFactory<TResult>(), (TaskFactory<TResult>) null) ?? Task<TResult>.s_Factory;

    internal override void InnerInvoke()
    {
      if (this.m_action is Func<TResult> action1)
      {
        this.m_result = action1();
      }
      else
      {
        if (!(this.m_action is Func<object, TResult> action))
          return;
        this.m_result = action(this.m_stateObject);
      }
    }

    /// <summary>Gets an awaiter used to await this <see cref="T:System.Threading.Tasks.Task`1" />.</summary>
    /// <returns>An awaiter instance.</returns>
    public TaskAwaiter<TResult> GetAwaiter() => new TaskAwaiter<TResult>(this);

    /// <summary>Configures an awaiter used to await this <see cref="T:System.Threading.Tasks.Task`1" />.</summary>
    /// <param name="continueOnCapturedContext">true to attempt to marshal the continuation back to the original context captured; otherwise, false.</param>
    /// <returns>An object used to await this task.</returns>
    public ConfiguredTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext) => new ConfiguredTaskAwaitable<TResult>(this, continueOnCapturedContext);

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.Task`1" /> that will complete when this <see cref="T:System.Threading.Tasks.Task`1" /> completes or when the specified <see cref="T:System.Threading.CancellationToken" /> has cancellation requested.</summary>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to monitor for a cancellation request.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task`1" /> representing the asynchronous wait. It may or may not be the same instance as the current instance.</returns>
    public Task<TResult> WaitAsync(CancellationToken cancellationToken) => this.WaitAsync(uint.MaxValue, cancellationToken);

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.Task`1" /> that will complete when this <see cref="T:System.Threading.Tasks.Task`1" /> completes or when the specified timeout expires.</summary>
    /// <param name="timeout">The timeout after which the <see cref="T:System.Threading.Tasks.Task" /> should be faulted with a <see cref="T:System.TimeoutException" /> if it hasn't otherwise completed.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task`1" /> representing the asynchronous wait. It may or may not be the same instance as the current instance.</returns>
    public Task<TResult> WaitAsync(TimeSpan timeout) => this.WaitAsync(Task.ValidateTimeout(timeout, ExceptionArgument.timeout), new CancellationToken());

    /// <summary>Gets a <see cref="T:System.Threading.Tasks.Task`1" /> that will complete when this <see cref="T:System.Threading.Tasks.Task`1" /> completes, when the specified timeout expires, or when the specified <see cref="T:System.Threading.CancellationToken" /> has cancellation requested.</summary>
    /// <param name="timeout">The timeout after which the <see cref="T:System.Threading.Tasks.Task" /> should be faulted with a <see cref="T:System.TimeoutException" /> if it hasn't otherwise completed.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to monitor for a cancellation request.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task`1" /> representing the asynchronous wait. It may or may not be the same instance as the current instance.</returns>
    public Task<TResult> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken) => this.WaitAsync(Task.ValidateTimeout(timeout, ExceptionArgument.timeout), cancellationToken);


    #nullable disable
    private Task<TResult> WaitAsync(uint millisecondsTimeout, CancellationToken cancellationToken)
    {
      if (this.IsCompleted || !cancellationToken.CanBeCanceled && millisecondsTimeout == uint.MaxValue)
        return this;
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled<TResult>(cancellationToken);
      return millisecondsTimeout == 0U ? Task.FromException<TResult>((System.Exception) new TimeoutException()) : (Task<TResult>) new Task.CancellationPromise<TResult>((Task) this, millisecondsTimeout, cancellationToken);
    }


    #nullable enable
    /// <summary>Creates a continuation that executes asynchronously when the target task completes.</summary>
    /// <param name="continuationAction">An action to run when the antecedent <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation task.</returns>
    public Task ContinueWith(Action<Task<TResult>> continuationAction) => this.ContinueWith(continuationAction, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);

    /// <summary>Creates a cancelable continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate is passed the completed task as an argument.</param>
    /// <param name="cancellationToken">The cancellation token that is passed to the new continuation task.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation task.</returns>
    public Task ContinueWith(
      Action<Task<TResult>> continuationAction,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith(continuationAction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(Action<Task<TResult>> continuationAction, TaskScheduler scheduler) => this.ContinueWith(continuationAction, scheduler, new CancellationToken(), TaskContinuationOptions.None);

    /// <summary>Creates a continuation that executes according the condition specified in <paramref name="continuationOptions" />.</summary>
    /// <param name="continuationAction">An action to according the condition specified in <paramref name="continuationOptions" />. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task<TResult>> continuationAction,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith(continuationAction, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that executes according the condition specified in <paramref name="continuationOptions" />.</summary>
    /// <param name="continuationAction">An action to run according the condition specified in <paramref name="continuationOptions" />. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task<TResult>> continuationAction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith(continuationAction, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    internal Task ContinueWith(
      Action<Task<TResult>> continuationAction,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationAction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task continuationTask = (Task) new ContinuationTaskFromResultTask<TResult>(this, (Delegate) continuationAction, (object) null, creationOptions, internalOptions);
      this.ContinueWithCore(continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }


    #nullable enable
    /// <summary>Creates a continuation that is passed state information and that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate is   passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(Action<Task<TResult>, object?> continuationAction, object? state) => this.ContinueWith(continuationAction, state, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task<TResult>, object?> continuationAction,
      object? state,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith(continuationAction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task<TResult>, object?> continuationAction,
      object? state,
      TaskScheduler scheduler)
    {
      return this.ContinueWith(continuationAction, state, scheduler, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such  as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationAction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task<TResult>, object?> continuationAction,
      object? state,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith(continuationAction, state, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationAction">An action to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation action.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new continuation task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as  well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its  execution.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task" />.</returns>
    public Task ContinueWith(
      Action<Task<TResult>, object?> continuationAction,
      object? state,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith(continuationAction, state, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    internal Task ContinueWith(
      Action<Task<TResult>, object> continuationAction,
      object state,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationAction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationAction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task continuationTask = (Task) new ContinuationTaskFromResultTask<TResult>(this, (Delegate) continuationAction, state, creationOptions, internalOptions);
      this.ContinueWithCore(continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }


    #nullable enable
    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, TNewResult> continuationFunction)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, TNewResult> continuationFunction,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes asynchronously when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, TNewResult> continuationFunction,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, scheduler, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes according the condition specified in <paramref name="continuationOptions" />.</summary>
    /// <param name="continuationFunction">A function to run according the condition specified in <paramref name="continuationOptions" />.
    /// 
    /// When run, the delegate will be passed the completed task as an argument.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, TNewResult> continuationFunction,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that executes according the condition specified in <paramref name="continuationOptions" />.</summary>
    /// <param name="continuationFunction">A function to run according the condition specified in <paramref name="continuationOptions" />.
    /// 
    /// When run, the delegate will be passed as an argument this completed task.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.Tasks.Task`1" /> has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, TNewResult> continuationFunction,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    internal Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, TNewResult> continuationFunction,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task<TNewResult> continuationTask = (Task<TNewResult>) new ContinuationResultTaskFromResultTask<TResult, TNewResult>(this, (Delegate) continuationFunction, (object) null, creationOptions, internalOptions);
      this.ContinueWithCore((Task) continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }


    #nullable enable
    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, object?, TNewResult> continuationFunction,
      object? state)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, state, TaskScheduler.Current, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, object?, TNewResult> continuationFunction,
      object? state,
      CancellationToken cancellationToken)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, state, TaskScheduler.Current, cancellationToken, TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, object?, TNewResult> continuationFunction,
      object? state,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, state, scheduler, new CancellationToken(), TaskContinuationOptions.None);
    }

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuationFunction" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, object?, TNewResult> continuationFunction,
      object? state,
      TaskContinuationOptions continuationOptions)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, state, TaskScheduler.Current, new CancellationToken(), continuationOptions);
    }

    /// <summary>Creates a continuation that executes when the target <see cref="T:System.Threading.Tasks.Task`1" /> completes.</summary>
    /// <param name="continuationFunction">A function to run when the <see cref="T:System.Threading.Tasks.Task`1" /> completes. When run, the delegate will be  passed the completed task and the caller-supplied state object as arguments.</param>
    /// <param name="state">An object representing data to be used by the continuation function.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that will be assigned to the new task.</param>
    /// <param name="continuationOptions">Options for when the continuation is scheduled and how it behaves. This includes criteria, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled" />, as well as execution options, such as <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</param>
    /// <param name="scheduler">The <see cref="T:System.Threading.Tasks.TaskScheduler" /> to associate with the continuation task and to use for its execution.</param>
    /// <typeparam name="TNewResult">The type of the result produced by the continuation.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="scheduler" /> argument is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The  <paramref name="continuationOptions" /> argument specifies an invalid value for <see cref="T:System.Threading.Tasks.TaskContinuationOptions" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The provided <see cref="T:System.Threading.CancellationToken" /> has already been disposed.</exception>
    /// <returns>A new continuation <see cref="T:System.Threading.Tasks.Task`1" />.</returns>
    public Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, object?, TNewResult> continuationFunction,
      object? state,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions,
      TaskScheduler scheduler)
    {
      return this.ContinueWith<TNewResult>(continuationFunction, state, scheduler, cancellationToken, continuationOptions);
    }


    #nullable disable
    internal Task<TNewResult> ContinueWith<TNewResult>(
      Func<Task<TResult>, object, TNewResult> continuationFunction,
      object state,
      TaskScheduler scheduler,
      CancellationToken cancellationToken,
      TaskContinuationOptions continuationOptions)
    {
      if (continuationFunction == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.continuationFunction);
      if (scheduler == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.scheduler);
      TaskCreationOptions creationOptions;
      InternalTaskOptions internalOptions;
      Task.CreationOptionsFromContinuationOptions(continuationOptions, out creationOptions, out internalOptions);
      Task<TNewResult> continuationTask = (Task<TNewResult>) new ContinuationResultTaskFromResultTask<TResult, TNewResult>(this, (Delegate) continuationFunction, state, creationOptions, internalOptions);
      this.ContinueWithCore((Task) continuationTask, scheduler, cancellationToken, continuationOptions);
      return continuationTask;
    }

    internal static class TaskWhenAnyCast
    {
      internal static readonly Func<Task<Task>, Task<TResult>> Value = (Func<Task<Task>, Task<TResult>>) (completed => (Task<TResult>) completed.Result);
    }
  }
}
