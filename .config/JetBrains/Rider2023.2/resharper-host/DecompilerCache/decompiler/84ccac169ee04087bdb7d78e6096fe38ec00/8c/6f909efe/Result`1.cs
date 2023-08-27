// Decompiled with JetBrains decompiler
// Type: FluentResults.Result`1
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
  public class Result<TValue> : ResultBase<Result<TValue>>, IResult<TValue>, IResultBase
  {
    private TValue _value;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public TValue ValueOrDefault => this._value;

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public TValue Value
    {
      get
      {
        this.ThrowIfFailed();
        return this._value;
      }
      private set
      {
        this.ThrowIfFailed();
        this._value = value;
      }
    }

    /// <summary>Set value</summary>
    public Result<TValue> WithValue(TValue value)
    {
      this.Value = value;
      return this;
    }

    /// <summary>Map all errors of the result via errorMapper</summary>
    /// <param name="errorMapper"></param>
    /// <returns></returns>
    public Result<TValue> MapErrors(Func<IError, IError> errorMapper) => this.IsSuccess ? this : new Result<TValue>().WithErrors(this.Errors.Select<IError, IError>(errorMapper)).WithSuccesses((IEnumerable<ISuccess>) this.Successes);

    /// <summary>Map all successes of the result via successMapper</summary>
    /// <param name="successMapper"></param>
    /// <returns></returns>
    public Result<TValue> MapSuccesses(Func<ISuccess, ISuccess> successMapper) => new Result<TValue>().WithValue(this.ValueOrDefault).WithErrors((IEnumerable<IError>) this.Errors).WithSuccesses(this.Successes.Select<ISuccess, ISuccess>(successMapper));

    /// <summary>Convert result with value to result without value</summary>
    public Result ToResult() => new Result().WithReasons((IEnumerable<IReason>) this.Reasons);

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue> valueConverter = null) => this.Map<TNewValue>(valueConverter);

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public Result<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> mapLogic)
    {
      if (this.IsSuccess && mapLogic == null)
        throw new ArgumentException("If result is success then valueConverter should not be null");
      return new Result<TNewValue>().WithValue(this.IsFailed ? default (TNewValue) : mapLogic(this.Value)).WithReasons((IEnumerable<IReason>) this.Reasons);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result
    ///     .Bind(GetWhichMayFail)
    ///     .Bind(ProcessWhichMayFail)
    ///     .Bind(FormattingWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="bind">Transformation that may fail.</param>
    public Result<TNewValue> Bind<TNewValue>(Func<TValue, Result<TNewValue>> bind)
    {
      Result<TNewValue> result1 = new Result<TNewValue>();
      result1.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result<TNewValue> result2 = bind(this.Value);
        result1.WithValue(result2.ValueOrDefault);
        result1.WithReasons((IEnumerable<IReason>) result2.Reasons);
      }
      return result1;
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="bind">Transformation that may fail.</param>
    public async Task<Result<TNewValue>> Bind<TNewValue>(Func<TValue, Task<Result<TNewValue>>> bind)
    {
      Result<TNewValue> result = new Result<TNewValue>();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result<TNewValue> converted = await bind(this.Value);
        result.WithValue(converted.ValueOrDefault);
        result.WithReasons((IEnumerable<IReason>) converted.Reasons);
        converted = (Result<TNewValue>) null;
      }
      Result<TNewValue> result1 = result;
      result = (Result<TNewValue>) null;
      return result1;
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="bind">Transformation that may fail.</param>
    public async ValueTask<Result<TNewValue>> Bind<TNewValue>(
      Func<TValue, ValueTask<Result<TNewValue>>> bind)
    {
      Result<TNewValue> result = new Result<TNewValue>();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result<TNewValue> converted = await bind(this.Value);
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
    public Result Bind(Func<TValue, Result> action)
    {
      Result result1 = new Result();
      result1.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result result2 = action(this.Value);
        result1.WithReasons((IEnumerable<IReason>) result2.Reasons);
      }
      return result1;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="T:FluentResults.Result" /> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="action">Action that may fail.</param>
    public async Task<Result> Bind(Func<TValue, Task<Result>> action)
    {
      Result result = new Result();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result converted = await action(this.Value);
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
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="action">Action that may fail.</param>
    public async ValueTask<Result> Bind(Func<TValue, ValueTask<Result>> action)
    {
      Result result = new Result();
      result.WithReasons((IEnumerable<IReason>) this.Reasons);
      if (this.IsSuccess)
      {
        Result converted = await action(this.Value);
        result.WithReasons((IEnumerable<IReason>) converted.Reasons);
        converted = (Result) null;
      }
      Result result1 = result;
      result = (Result) null;
      return result1;
    }

    public override string ToString() => base.ToString() + ", " + ((object) this.ValueOrDefault).ToLabelValueStringOrEmpty("Value");

    public static implicit operator Result<TValue>(Result result) => result.ToResult<TValue>();

    public static implicit operator Result<object>(Result<TValue> result) => result.ToResult<object>((Func<TValue, object>) (value => (object) value));

    public static implicit operator Result<TValue>(TValue value) => value is Result<TValue> result ? result : Result.Ok<TValue>(value);

    public static implicit operator Result<TValue>(Error error) => (Result<TValue>) Result.Fail((IError) error);

    public static implicit operator Result<TValue>(List<Error> errors) => (Result<TValue>) Result.Fail((IEnumerable<IError>) errors);

    /// <summary>Deconstruct Result</summary>
    /// <param name="isSuccess"></param>
    /// <param name="isFailed"></param>
    /// <param name="value"></param>
    public void Deconstruct(out bool isSuccess, out bool isFailed, out TValue value)
    {
      isSuccess = this.IsSuccess;
      isFailed = this.IsFailed;
      value = this.IsSuccess ? this.Value : default (TValue);
    }

    /// <summary>Deconstruct Result</summary>
    /// <param name="isSuccess"></param>
    /// <param name="isFailed"></param>
    /// <param name="value"></param>
    /// <param name="errors"></param>
    public void Deconstruct(
      out bool isSuccess,
      out bool isFailed,
      out TValue value,
      out List<IError> errors)
    {
      isSuccess = this.IsSuccess;
      isFailed = this.IsFailed;
      value = this.IsSuccess ? this.Value : default (TValue);
      errors = this.IsFailed ? this.Errors : (List<IError>) null;
    }

    private void ThrowIfFailed()
    {
      if (this.IsFailed)
        throw new InvalidOperationException("Result is in status failed. Value is not set. Having: " + ReasonFormat.ErrorReasonsToString((IReadOnlyCollection<IError>) this.Errors));
    }
  }
}
