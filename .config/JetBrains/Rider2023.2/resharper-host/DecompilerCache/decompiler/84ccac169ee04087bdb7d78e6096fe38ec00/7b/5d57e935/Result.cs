// Decompiled with JetBrains decompiler
// Type: FluentResults.Result
// Assembly: FluentResults, Version=3.15.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 84CCAC16-9EE0-4087-BDB7-D78E6096FE38
// Assembly location: /home/phetoush/.nuget/packages/fluentresults/3.15.2/lib/netstandard2.1/FluentResults.dll
// XML documentation location: /home/phetoush/.nuget/packages/fluentresults/3.15.2/lib/netstandard2.1/FluentResults.xml

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentResults
{
  public class Result : ResultBase<Result>
  {
    internal static ResultSettings Settings { get; private set; }

    static Result() => Result.Settings = new ResultSettingsBuilder().Build();

    /// <summary>Setup global settings like logging</summary>
    public static void Setup(Action<ResultSettingsBuilder> setupFunc)
    {
      ResultSettingsBuilder resultSettingsBuilder = new ResultSettingsBuilder();
      setupFunc(resultSettingsBuilder);
      Result.Settings = resultSettingsBuilder.Build();
    }

    /// <summary>Creates a success result</summary>
    public static Result Ok() => new Result();

    /// <summary>Creates a failed result with the given error</summary>
    public static Result Fail(IError error)
    {
      Result result = new Result();
      result.WithError(error);
      return result;
    }

    /// <summary>
    /// Creates a failed result with the given error message. Internally an error object from the error factory is created.
    /// </summary>
    public static Result Fail(string errorMessage)
    {
      Result result = new Result();
      result.WithError(Result.Settings.ErrorFactory(errorMessage));
      return result;
    }

    /// <summary>
    /// Creates a failed result with the given error messages. Internally a list of error objects from the error factory is created
    /// </summary>
    public static Result Fail(IEnumerable<string> errorMessages)
    {
      if (errorMessages == null)
        throw new ArgumentNullException(nameof (errorMessages), "The list of error messages cannot be null");
      Result result = new Result();
      result.WithErrors(errorMessages.Select<string, IError>(Result.Settings.ErrorFactory));
      return result;
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    public static Result Fail(IEnumerable<IError> errors)
    {
      if (errors == null)
        throw new ArgumentNullException(nameof (errors), "The list of errors cannot be null");
      Result result = new Result();
      result.WithErrors(errors);
      return result;
    }

    /// <summary>Creates a success result with the given value</summary>
    public static Result<TValue> Ok<TValue>(TValue value)
    {
      Result<TValue> result = new Result<TValue>();
      result.WithValue(value);
      return result;
    }

    /// <summary>Creates a failed result with the given error</summary>
    public static Result<TValue> Fail<TValue>(IError error)
    {
      Result<TValue> result = new Result<TValue>();
      result.WithError(error);
      return result;
    }

    /// <summary>
    /// Creates a failed result with the given error message. Internally an error object from the error factory is created.
    /// </summary>
    public static Result<TValue> Fail<TValue>(string errorMessage)
    {
      Result<TValue> result = new Result<TValue>();
      result.WithError(Result.Settings.ErrorFactory(errorMessage));
      return result;
    }

    /// <summary>
    /// Creates a failed result with the given error messages. Internally a list of error objects from the error factory is created.
    /// </summary>
    public static Result<TValue> Fail<TValue>(IEnumerable<string> errorMessages)
    {
      if (errorMessages == null)
        throw new ArgumentNullException(nameof (errorMessages), "The list of error messages cannot be null");
      Result<TValue> result = new Result<TValue>();
      result.WithErrors(errorMessages.Select<string, IError>(Result.Settings.ErrorFactory));
      return result;
    }

    /// <summary>Creates a failed result with the given errors.</summary>
    public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors)
    {
      if (errors == null)
        throw new ArgumentNullException(nameof (errors), "The list of errors cannot be null");
      Result<TValue> result = new Result<TValue>();
      result.WithErrors(errors);
      return result;
    }

    /// <summary>
    /// Merge multiple result objects to one result object together
    /// </summary>
    public static Result Merge(params ResultBase[] results) => ResultHelper.Merge((IEnumerable<ResultBase>) results);

    /// <summary>
    /// Merge multiple result objects to one result object together. Return one result with a list of merged values.
    /// </summary>
    public static Result<IEnumerable<TValue>> Merge<TValue>(params Result<TValue>[] results) => ResultHelper.MergeWithValue<TValue>((IEnumerable<Result<TValue>>) results);

    /// <summary>
    /// Create a success/failed result depending on the parameter isSuccess
    /// </summary>
    public static Result OkIf(bool isSuccess, IError error) => isSuccess ? Result.Ok() : Result.Fail(error);

    /// <summary>
    /// Create a success/failed result depending on the parameter isSuccess
    /// </summary>
    public static Result OkIf(bool isSuccess, string error) => isSuccess ? Result.Ok() : Result.Fail(error);

    /// <summary>
    /// Create a success/failed result depending on the parameter isSuccess
    /// </summary>
    /// <remarks>Error is lazily evaluated.</remarks>
    public static Result OkIf(bool isSuccess, Func<IError> errorFactory) => isSuccess ? Result.Ok() : Result.Fail(errorFactory());

    /// <summary>
    /// Create a success/failed result depending on the parameter isSuccess
    /// </summary>
    /// <remarks>Error is lazily evaluated.</remarks>
    public static Result OkIf(bool isSuccess, Func<string> errorMessageFactory) => isSuccess ? Result.Ok() : Result.Fail(errorMessageFactory());

    /// <summary>
    /// Create a success/failed result depending on the parameter isFailure
    /// </summary>
    public static Result FailIf(bool isFailure, IError error) => isFailure ? Result.Fail(error) : Result.Ok();

    /// <summary>
    /// Create a success/failed result depending on the parameter isFailure
    /// </summary>
    public static Result FailIf(bool isFailure, string error) => isFailure ? Result.Fail(error) : Result.Ok();

    /// <summary>
    /// Create a success/failed result depending on the parameter isFailure
    /// </summary>
    /// <remarks>Error is lazily evaluated.</remarks>
    public static Result FailIf(bool isFailure, Func<IError> errorFactory) => isFailure ? Result.Fail(errorFactory()) : Result.Ok();

    /// <summary>
    /// Create a success/failed result depending on the parameter isFailure
    /// </summary>
    /// <remarks>Error is lazily evaluated.</remarks>
    public static Result FailIf(bool isFailure, Func<string> errorMessageFactory) => isFailure ? Result.Fail(errorMessageFactory()) : Result.Ok();

    /// <summary>
    /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
    /// </summary>
    public static Result Try(Action action, Func<Exception, IError> catchHandler = null)
    {
      catchHandler = catchHandler ?? Result.Settings.DefaultTryCatchHandler;
      try
      {
        action();
        return Result.Ok();
      }
      catch (Exception ex)
      {
        return Result.Fail(catchHandler(ex));
      }
    }

    /// <summary>
    /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
    /// </summary>
    public static async Task<Result> Try(Func<Task> action, Func<Exception, IError> catchHandler = null)
    {
      catchHandler = catchHandler ?? Result.Settings.DefaultTryCatchHandler;
      try
      {
        await action();
        return Result.Ok();
      }
      catch (Exception ex)
      {
        return Result.Fail(catchHandler(ex));
      }
    }

    /// <summary>
    /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
    /// </summary>
    public static async ValueTask<Result> Try(
      Func<ValueTask> action,
      Func<Exception, IError> catchHandler = null)
    {
      catchHandler = catchHandler ?? Result.Settings.DefaultTryCatchHandler;
      try
      {
        await action();
        return Result.Ok();
      }
      catch (Exception ex)
      {
        return Result.Fail(catchHandler(ex));
      }
    }

    /// <summary>
    /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
    /// </summary>
    public static Result<T> Try<T>(Func<T> action, Func<Exception, IError> catchHandler = null)
    {
      catchHandler = catchHandler ?? Result.Settings.DefaultTryCatchHandler;
      try
      {
        return Result.Ok<T>(action());
      }
      catch (Exception ex)
      {
        return (Result<T>) Result.Fail(catchHandler(ex));
      }
    }

    /// <summary>
    /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
    /// </summary>
    public static async Task<Result<T>> Try<T>(
      Func<Task<T>> action,
      Func<Exception, IError> catchHandler = null)
    {
      catchHandler = catchHandler ?? Result.Settings.DefaultTryCatchHandler;
      try
      {
        T obj = await action();
        return Result.Ok<T>(obj);
      }
      catch (Exception ex)
      {
        return (Result<T>) Result.Fail(catchHandler(ex));
      }
    }

    /// <summary>
    /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
    /// </summary>
    public static async ValueTask<Result<T>> Try<T>(
      Func<ValueTask<T>> action,
      Func<Exception, IError> catchHandler = null)
    {
      catchHandler = catchHandler ?? Result.Settings.DefaultTryCatchHandler;
      try
      {
        T obj = await action();
        return Result.Ok<T>(obj);
      }
      catch (Exception ex)
      {
        return (Result<T>) Result.Fail(catchHandler(ex));
      }
    }

    /// <summary>Map all errors of the result via errorMapper</summary>
    /// <param name="errorMapper"></param>
    /// <returns></returns>
    public Result MapErrors(Func<IError, IError> errorMapper) => this.IsSuccess ? this : new Result().WithErrors(this.Errors.Select<IError, IError>(errorMapper)).WithSuccesses((IEnumerable<ISuccess>) this.Successes);

    /// <summary>Map all successes of the result via successMapper</summary>
    /// <param name="successMapper"></param>
    /// <returns></returns>
    public Result MapSuccesses(Func<ISuccess, ISuccess> successMapper) => new Result().WithErrors((IEnumerable<IError>) this.Errors).WithSuccesses(this.Successes.Select<ISuccess, ISuccess>(successMapper));

    public Result<TNewValue> ToResult<TNewValue>(TNewValue newValue = null) => new Result<TNewValue>().WithValue(this.IsFailed ? default (TNewValue) : newValue).WithReasons((IEnumerable<IReason>) this.Reasons);

    /// <summary>Convert result to result with value that may fail.</summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="bind">Transformation that may fail.</param>
    public Result<TNewValue> Bind<TNewValue>(Func<Result<TNewValue>> bind)
    {
      Result<TNewValue> result1 = new Result<TNewValue>();
      result1.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result<TNewValue> result2 = bind();
        result1.WithValue(result2.ValueOrDefault);
        result1.WithReasons((IEnumerable<IReason>) result2.Reasons);
      }
      return result1;
    }

    /// <summary>
    /// Convert result to result with value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="bind">Transformation that may fail.</param>
    public async Task<Result<TNewValue>> Bind<TNewValue>(Func<Task<Result<TNewValue>>> bind)
    {
      Result<TNewValue> result = new Result<TNewValue>();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result<TNewValue> converted = await bind();
        result.WithValue(converted.ValueOrDefault);
        result.WithReasons((IEnumerable<IReason>) converted.Reasons);
        converted = (Result<TNewValue>) null;
      }
      Result<TNewValue> result1 = result;
      result = (Result<TNewValue>) null;
      return result1;
    }

    /// <summary>
    /// Convert result to result with value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="bind">Transformation that may fail.</param>
    public async ValueTask<Result<TNewValue>> Bind<TNewValue>(
      Func<ValueTask<Result<TNewValue>>> bind)
    {
      Result<TNewValue> result = new Result<TNewValue>();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result<TNewValue> converted = await bind();
        result.WithValue(converted.ValueOrDefault);
        result.WithReasons((IEnumerable<IReason>) converted.Reasons);
        converted = (Result<TNewValue>) null;
      }
      Result<TNewValue> result1 = result;
      result = (Result<TNewValue>) null;
      return result1;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="T:FluentResults.Result" />.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="action">Action that may fail.</param>
    public Result Bind(Func<Result> action)
    {
      Result result1 = new Result();
      result1.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result result2 = action();
        result1.WithReasons((IEnumerable<IReason>) result2.Reasons);
      }
      return result1;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="T:FluentResults.Result" /> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="action">Action that may fail.</param>
    public async Task<Result> Bind(Func<Task<Result>> action)
    {
      Result result = new Result();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result converted = await action();
        result.WithReasons((IEnumerable<IReason>) converted.Reasons);
        converted = (Result) null;
      }
      Result result1 = result;
      result = (Result) null;
      return result1;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="T:FluentResults.Result" /> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="action">Action that may fail.</param>
    public async ValueTask<Result> Bind(Func<ValueTask<Result>> action)
    {
      Result result = new Result();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result converted = await action();
        result.WithReasons((IEnumerable<IReason>) converted.Reasons);
        converted = (Result) null;
      }
      Result result1 = result;
      result = (Result) null;
      return result1;
    }

    public static implicit operator Result(Error error) => Result.Fail((IError) error);

    public static implicit operator Result(List<Error> errors) => Result.Fail((IEnumerable<IError>) errors);
  }
}
