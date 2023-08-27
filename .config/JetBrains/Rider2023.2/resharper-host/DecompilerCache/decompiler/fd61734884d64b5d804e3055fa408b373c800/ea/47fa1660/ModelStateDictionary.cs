// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary
// Assembly: Microsoft.AspNetCore.Mvc.Abstractions, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: FD617348-84D6-4B5D-804E-3055FA408B37
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.AspNetCore.Mvc.Abstractions.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.AspNetCore.Mvc.Abstractions.xml

using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;


#nullable enable
namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
  /// <summary>
  /// Represents the state of an attempt to bind values from an HTTP Request to an action method, which includes
  /// validation information.
  /// </summary>
  public class ModelStateDictionary : 
    IReadOnlyDictionary<string, ModelStateEntry?>,
    IEnumerable<KeyValuePair<string, ModelStateEntry?>>,
    IEnumerable,
    IReadOnlyCollection<KeyValuePair<string, ModelStateEntry?>>
  {
    /// <summary>
    /// The default value for <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.MaxAllowedErrors" /> of <c>200</c>.
    /// </summary>
    public static readonly int DefaultMaxAllowedErrors = 200;
    internal const int DefaultMaxRecursionDepth = 32;
    private const char DelimiterDot = '.';
    private const char DelimiterOpen = '[';

    #nullable disable
    private readonly ModelStateDictionary.ModelStateNode _root;
    private int _maxAllowedErrors;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> class.
    /// </summary>
    public ModelStateDictionary()
      : this(ModelStateDictionary.DefaultMaxAllowedErrors)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> class.
    /// </summary>
    public ModelStateDictionary(int maxAllowedErrors)
      : this(maxAllowedErrors, 32, 32)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> class.
    /// </summary>
    private ModelStateDictionary(int maxAllowedErrors, int maxValidationDepth, int maxStateDepth)
    {
      this.MaxAllowedErrors = maxAllowedErrors;
      this.MaxValidationDepth = new int?(maxValidationDepth);
      this.MaxStateDepth = new int?(maxStateDepth);
      this._root = new ModelStateDictionary.ModelStateNode(new StringSegment(string.Empty))
      {
        Key = string.Empty
      };
    }


    #nullable enable
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> class by using values that are copied
    /// from the specified <paramref name="dictionary" />.
    /// </summary>
    /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> to copy values from.</param>
    public ModelStateDictionary(ModelStateDictionary dictionary)
    {
      int maxAllowedErrors = dictionary != null ? dictionary.MaxAllowedErrors : ModelStateDictionary.DefaultMaxAllowedErrors;
      int? nullable = (int?) dictionary?.MaxValidationDepth;
      int maxValidationDepth = nullable ?? 32;
      nullable = (int?) dictionary?.MaxStateDepth;
      int maxStateDepth = nullable ?? 32;
      // ISSUE: explicit constructor call
      this.\u002Ector(maxAllowedErrors, maxValidationDepth, maxStateDepth);
      if (dictionary == null)
        throw new ArgumentNullException(nameof (dictionary));
      this.Merge(dictionary);
    }

    /// <summary>
    /// Root entry for the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.
    /// </summary>
    public ModelStateEntry Root => (ModelStateEntry) this._root;

    /// <summary>
    /// Gets or sets the maximum allowed model state errors in this instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.
    /// Defaults to <c>200</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> tracks the number of model errors added by calls to
    /// <see cref="M:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.AddModelError(System.String,System.Exception,Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata)" /> or
    /// <see cref="M:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.TryAddModelError(System.String,System.Exception,Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata)" />.
    /// Once the value of <c>MaxAllowedErrors - 1</c> is reached, if another attempt is made to add an error,
    /// the error message will be ignored and a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.TooManyModelErrorsException" /> will be added.
    /// </para>
    /// <para>
    /// Errors added via modifying <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> directly do not count towards this limit.
    /// </para>
    /// </remarks>
    public int MaxAllowedErrors
    {
      get => this._maxAllowedErrors;
      set => this._maxAllowedErrors = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    /// <summary>
    /// Gets a value indicating whether or not the maximum number of errors have been
    /// recorded.
    /// </summary>
    /// <remarks>
    /// Returns <c>true</c> if a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.TooManyModelErrorsException" /> has been recorded;
    /// otherwise <c>false</c>.
    /// </remarks>
    public bool HasReachedMaxErrors => this.ErrorCount >= this.MaxAllowedErrors;

    /// <summary>
    /// Gets the number of errors added to this instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> via
    /// <see cref="M:AddModelError" /> or <see cref="M:TryAddModelError" />.
    /// </summary>
    public int ErrorCount { get; private set; }

    /// <inheritdoc />
    public int Count { get; private set; }

    /// <summary>Gets the key sequence.</summary>
    public ModelStateDictionary.KeyEnumerable Keys => new ModelStateDictionary.KeyEnumerable(this);

    IEnumerable<string> IReadOnlyDictionary<
    #nullable disable
    string, ModelStateEntry>.Keys => (IEnumerable<string>) this.Keys;

    /// <summary>Gets the value sequence.</summary>
    public ModelStateDictionary.ValueEnumerable Values => new ModelStateDictionary.ValueEnumerable(this);


    #nullable enable
    IEnumerable<ModelStateEntry> IReadOnlyDictionary<
    #nullable disable
    string, ModelStateEntry>.Values => (IEnumerable<ModelStateEntry>) this.Values;

    /// <summary>
    /// Gets a value that indicates whether any model state values in this model state dictionary is invalid or not validated.
    /// </summary>
    public bool IsValid
    {
      get
      {
        ModelValidationState validationState = this.ValidationState;
        return validationState == ModelValidationState.Valid || validationState == ModelValidationState.Skipped;
      }
    }

    /// <inheritdoc />
    public ModelValidationState ValidationState => this.GetValidity(this._root, 0) ?? ModelValidationState.Valid;


    #nullable enable
    /// <inheritdoc />
    public ModelStateEntry? this[string key]
    {
      get
      {
        if (key == null)
          throw new ArgumentNullException(nameof (key));
        ModelStateEntry modelStateEntry;
        this.TryGetValue(key, out modelStateEntry);
        return modelStateEntry;
      }
    }

    private bool HasRecordedMaxModelError { get; set; }

    internal int? MaxValidationDepth { get; set; }

    internal int? MaxStateDepth { get; set; }

    /// <summary>
    /// Adds the specified <paramref name="exception" /> to the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.Errors" /> instance
    /// that is associated with the specified <paramref name="key" />. If the maximum number of allowed
    /// errors has already been recorded, ensures that a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.TooManyModelErrorsException" /> exception is
    /// recorded instead.
    /// </summary>
    /// <remarks>
    /// This method allows adding the <paramref name="exception" /> to the current <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />
    /// when <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata" /> is not available or the exact <paramref name="exception" />
    /// must be maintained for later use (even if it is for example a <see cref="T:System.FormatException" />).
    /// Where <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata" /> is available, use <see cref="M:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.AddModelError(System.String,System.Exception,Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata)" /> instead.
    /// </remarks>
    /// <param name="key">The key of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> to add errors to.</param>
    /// <param name="exception">The <see cref="T:System.Exception" /> to add.</param>
    /// <returns>
    /// <c>True</c> if the given error was added, <c>false</c> if the error was ignored.
    /// See <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.MaxAllowedErrors" />.
    /// </returns>
    public bool TryAddModelException(string key, Exception exception)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      if ((exception is InputFormatterException || exception is ValueProviderException) && !string.IsNullOrEmpty(exception.Message))
        return this.TryAddModelError(key, exception.Message);
      if (this.ErrorCount >= this.MaxAllowedErrors - 1)
      {
        this.EnsureMaxErrorsReachedRecorded();
        return false;
      }
      this.AddModelErrorCore(key, exception);
      return true;
    }

    /// <summary>
    /// Adds the specified <paramref name="exception" /> to the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.Errors" /> instance
    /// that is associated with the specified <paramref name="key" />. If the maximum number of allowed
    /// errors has already been recorded, ensures that a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.TooManyModelErrorsException" /> exception is
    /// recorded instead.
    /// </summary>
    /// <param name="key">The key of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> to add errors to.</param>
    /// <param name="exception">The <see cref="T:System.Exception" /> to add. Some exception types will be replaced with
    /// a descriptive error message.</param>
    /// <param name="metadata">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata" /> associated with the model.</param>
    public void AddModelError(string key, Exception exception, ModelMetadata metadata)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      if (metadata == null)
        throw new ArgumentNullException(nameof (metadata));
      this.TryAddModelError(key, exception, metadata);
    }

    /// <summary>
    /// Attempts to add the specified <paramref name="exception" /> to the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.Errors" />
    /// instance that is associated with the specified <paramref name="key" />. If the maximum number of allowed
    /// errors has already been recorded, ensures that a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.TooManyModelErrorsException" /> exception is
    /// recorded instead.
    /// </summary>
    /// <param name="key">The key of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> to add errors to.</param>
    /// <param name="exception">The <see cref="T:System.Exception" /> to add. Some exception types will be replaced with
    /// a descriptive error message.</param>
    /// <param name="metadata">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelMetadata" /> associated with the model.</param>
    /// <returns>
    /// <c>True</c> if the given error was added, <c>false</c> if the error was ignored.
    /// See <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.MaxAllowedErrors" />.
    /// </returns>
    public bool TryAddModelError(string key, Exception exception, ModelMetadata metadata)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      if (metadata == null)
        throw new ArgumentNullException(nameof (metadata));
      if (this.ErrorCount >= this.MaxAllowedErrors - 1)
      {
        this.EnsureMaxErrorsReachedRecorded();
        return false;
      }
      switch (exception)
      {
        case FormatException _:
        case OverflowException _:
          ModelStateEntry modelStateEntry;
          this.TryGetValue(key, out modelStateEntry);
          ModelBindingMessageProvider bindingMessageProvider = metadata.ModelBindingMessageProvider;
          string str = metadata.DisplayName ?? metadata.PropertyName;
          string errorMessage = modelStateEntry != null || str != null ? (modelStateEntry != null ? (str != null ? bindingMessageProvider.AttemptedValueIsInvalidAccessor(modelStateEntry.AttemptedValue, str) : bindingMessageProvider.NonPropertyAttemptedValueIsInvalidAccessor(modelStateEntry.AttemptedValue)) : bindingMessageProvider.UnknownValueIsInvalidAccessor(str)) : bindingMessageProvider.NonPropertyUnknownValueIsInvalidAccessor();
          return this.TryAddModelError(key, errorMessage);
        case InputFormatterException _:
        case ValueProviderException _:
          if (!string.IsNullOrEmpty(exception.Message))
            return this.TryAddModelError(key, exception.Message);
          break;
      }
      this.AddModelErrorCore(key, exception);
      return true;
    }

    /// <summary>
    /// Adds the specified <paramref name="errorMessage" /> to the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.Errors" /> instance
    /// that is associated with the specified <paramref name="key" />. If the maximum number of allowed
    /// errors has already been recorded, ensures that a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.TooManyModelErrorsException" /> exception is
    /// recorded instead.
    /// </summary>
    /// <param name="key">The key of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> to add errors to.</param>
    /// <param name="errorMessage">The error message to add.</param>
    public void AddModelError(string key, string errorMessage)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (errorMessage == null)
        throw new ArgumentNullException(nameof (errorMessage));
      this.TryAddModelError(key, errorMessage);
    }

    /// <summary>
    /// Attempts to add the specified <paramref name="errorMessage" /> to the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.Errors" />
    /// instance that is associated with the specified <paramref name="key" />. If the maximum number of allowed
    /// errors has already been recorded, ensures that a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.TooManyModelErrorsException" /> exception is
    /// recorded instead.
    /// </summary>
    /// <param name="key">The key of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> to add errors to.</param>
    /// <param name="errorMessage">The error message to add.</param>
    /// <returns>
    /// <c>True</c> if the given error was added, <c>false</c> if the error was ignored.
    /// See <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.MaxAllowedErrors" />.
    /// </returns>
    public bool TryAddModelError(string key, string errorMessage)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (errorMessage == null)
        throw new ArgumentNullException(nameof (errorMessage));
      if (this.ErrorCount >= this.MaxAllowedErrors - 1)
      {
        this.EnsureMaxErrorsReachedRecorded();
        return false;
      }
      ModelStateDictionary.ModelStateNode orAddNode = this.GetOrAddNode(key);
      this.Count += !orAddNode.IsContainerNode ? 0 : 1;
      orAddNode.ValidationState = ModelValidationState.Invalid;
      orAddNode.MarkNonContainerNode();
      orAddNode.Errors.Add(errorMessage);
      ++this.ErrorCount;
      return true;
    }

    /// <summary>
    /// Returns the aggregate <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState" /> for items starting with the
    /// specified <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key to look up model state errors for.</param>
    /// <returns>Returns <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Unvalidated" /> if no entries are found for the specified
    /// key, <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid" /> if at least one instance is found with one or more model
    /// state errors; <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid" /> otherwise.</returns>
    public ModelValidationState GetFieldValidationState(string key) => key != null ? this.GetValidity(this.GetNode(key), 0).GetValueOrDefault() : throw new ArgumentNullException(nameof (key));

    /// <summary>
    /// Returns <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState" /> for the <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key to look up model state errors for.</param>
    /// <returns>Returns <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Unvalidated" /> if no entry is found for the specified
    /// key, <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid" /> if an instance is found with one or more model
    /// state errors; <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid" /> otherwise.</returns>
    public ModelValidationState GetValidationState(string key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      ModelStateEntry modelStateEntry;
      return this.TryGetValue(key, out modelStateEntry) ? modelStateEntry.ValidationState : ModelValidationState.Unvalidated;
    }

    /// <summary>
    /// Marks the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.ValidationState" /> for the entry with the specified
    /// <paramref name="key" /> as <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid" />.
    /// </summary>
    /// <param name="key">The key of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> to mark as valid.</param>
    public void MarkFieldValid(string key)
    {
      ModelStateDictionary.ModelStateNode modelStateNode = key != null ? this.GetOrAddNode(key) : throw new ArgumentNullException(nameof (key));
      if (modelStateNode.ValidationState == ModelValidationState.Invalid)
        throw new InvalidOperationException(Resources.Validation_InvalidFieldCannotBeReset);
      this.Count += !modelStateNode.IsContainerNode ? 0 : 1;
      modelStateNode.MarkNonContainerNode();
      modelStateNode.ValidationState = ModelValidationState.Valid;
    }

    /// <summary>
    /// Marks the <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.ValidationState" /> for the entry with the specified <paramref name="key" />
    /// as <see cref="F:Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped" />.
    /// </summary>
    /// <param name="key">The key of the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> to mark as skipped.</param>
    public void MarkFieldSkipped(string key)
    {
      ModelStateDictionary.ModelStateNode modelStateNode = key != null ? this.GetOrAddNode(key) : throw new ArgumentNullException(nameof (key));
      if (modelStateNode.ValidationState == ModelValidationState.Invalid)
        throw new InvalidOperationException(Resources.Validation_InvalidFieldCannotBeReset_ToSkipped);
      this.Count += !modelStateNode.IsContainerNode ? 0 : 1;
      modelStateNode.MarkNonContainerNode();
      modelStateNode.ValidationState = ModelValidationState.Skipped;
    }

    /// <summary>
    /// Copies the values from the specified <paramref name="dictionary" /> into this instance, overwriting
    /// existing values if keys are the same.
    /// </summary>
    /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> to copy values from.</param>
    public void Merge(ModelStateDictionary dictionary)
    {
      if (dictionary == null)
        return;
      foreach (KeyValuePair<string, ModelStateEntry> keyValuePair in dictionary)
      {
        ModelStateDictionary.ModelStateNode orAddNode = this.GetOrAddNode(keyValuePair.Key);
        this.Count += !orAddNode.IsContainerNode ? 0 : 1;
        this.ErrorCount += keyValuePair.Value.Errors.Count - orAddNode.Errors.Count;
        orAddNode.Copy(keyValuePair.Value);
        orAddNode.MarkNonContainerNode();
      }
    }

    /// <summary>
    /// Sets the of <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.RawValue" /> and <see cref="P:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry.AttemptedValue" /> for
    /// the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> with the specified <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key for the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> entry.</param>
    /// <param name="rawValue">The raw value for the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> entry.</param>
    /// <param name="attemptedValue">
    /// The values of <paramref name="rawValue" /> in a comma-separated <see cref="T:System.String" />.
    /// </param>
    public void SetModelValue(string key, object? rawValue, string? attemptedValue)
    {
      ModelStateDictionary.ModelStateNode modelStateNode = key != null ? this.GetOrAddNode(key) : throw new ArgumentNullException(nameof (key));
      this.Count += !modelStateNode.IsContainerNode ? 0 : 1;
      modelStateNode.RawValue = rawValue;
      modelStateNode.AttemptedValue = attemptedValue;
      modelStateNode.MarkNonContainerNode();
    }

    /// <summary>
    /// Sets the value for the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> with the specified <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key for the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> entry</param>
    /// <param name="valueProviderResult">
    /// A <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ValueProviderResult" /> with data for the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> entry.
    /// </param>
    public void SetModelValue(string key, ValueProviderResult valueProviderResult)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      object rawValue = !(valueProviderResult == ValueProviderResult.None) ? (valueProviderResult.Length != 1 ? (object) valueProviderResult.Values.ToArray() : (object) valueProviderResult.Values[0]) : (object) null;
      this.SetModelValue(key, rawValue, valueProviderResult.ToString());
    }

    /// <summary>
    /// Clears <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> entries that match the key that is passed as parameter.
    /// </summary>
    /// <param name="key">The key of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> to clear.</param>
    public void ClearValidationState(string key)
    {
      foreach (KeyValuePair<string, ModelStateEntry> keyValuePair in this.FindKeysWithPrefix(key ?? string.Empty))
      {
        keyValuePair.Value.Errors.Clear();
        keyValuePair.Value.ValidationState = ModelValidationState.Unvalidated;
      }
    }


    #nullable disable
    private ModelStateDictionary.ModelStateNode GetNode(string key)
    {
      ModelStateDictionary.ModelStateNode node = this._root;
      if (key.Length > 0)
      {
        ModelStateDictionary.MatchResult currentMatch = new ModelStateDictionary.MatchResult();
        do
        {
          StringSegment next = ModelStateDictionary.FindNext(key, ref currentMatch);
          node = node.GetNode(next);
        }
        while (node != null && currentMatch.Type != ModelStateDictionary.Delimiter.None);
      }
      return node;
    }

    private ModelStateDictionary.ModelStateNode GetOrAddNode(string key)
    {
      ModelStateDictionary.ModelStateNode orAddNode = this._root;
      if (key.Length > 0)
      {
        int num1 = 0;
        ModelStateDictionary.MatchResult currentMatch = new ModelStateDictionary.MatchResult();
        do
        {
          int? maxStateDepth = this.MaxStateDepth;
          if (maxStateDepth.HasValue)
          {
            int num2 = num1;
            maxStateDepth = this.MaxStateDepth;
            int valueOrDefault = maxStateDepth.GetValueOrDefault();
            if (num2 >= valueOrDefault & maxStateDepth.HasValue)
              throw new InvalidOperationException(Resources.FormatModelStateDictionary_MaxModelStateDepth((object) this.MaxStateDepth));
          }
          StringSegment next = ModelStateDictionary.FindNext(key, ref currentMatch);
          orAddNode = orAddNode.GetOrAddNode(next);
          ++num1;
        }
        while (currentMatch.Type != ModelStateDictionary.Delimiter.None);
        if (orAddNode.Key == null)
          orAddNode.Key = key;
      }
      return orAddNode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static StringSegment FindNext(
      string key,
      ref ModelStateDictionary.MatchResult currentMatch)
    {
      int index = currentMatch.Index;
      ModelStateDictionary.Delimiter delimiter = ModelStateDictionary.Delimiter.None;
      for (; index < key.Length; ++index)
      {
        switch (key[index])
        {
          case '.':
            delimiter = ModelStateDictionary.Delimiter.Dot;
            goto label_6;
          case '[':
            delimiter = ModelStateDictionary.Delimiter.OpenBracket;
            goto label_6;
          default:
            continue;
        }
      }
label_6:
      int offset = currentMatch.Type == ModelStateDictionary.Delimiter.OpenBracket ? currentMatch.Index - 1 : currentMatch.Index;
      currentMatch.Type = delimiter;
      currentMatch.Index = index + 1;
      return new StringSegment(key, offset, index - offset);
    }

    private ModelValidationState? GetValidity(
      ModelStateDictionary.ModelStateNode node,
      int currentDepth)
    {
      if (node != null)
      {
        if (this.MaxValidationDepth.HasValue)
        {
          int num = currentDepth;
          int? maxValidationDepth = this.MaxValidationDepth;
          int valueOrDefault = maxValidationDepth.GetValueOrDefault();
          if (num >= valueOrDefault & maxValidationDepth.HasValue)
            goto label_3;
        }
        ModelValidationState? validity1 = new ModelValidationState?();
        if (!node.IsContainerNode)
        {
          validity1 = new ModelValidationState?(ModelValidationState.Valid);
          if (node.ValidationState == ModelValidationState.Unvalidated)
            return new ModelValidationState?(ModelValidationState.Unvalidated);
          if (node.ValidationState == ModelValidationState.Invalid)
            validity1 = new ModelValidationState?(node.ValidationState);
        }
        if (node.ChildNodes != null)
        {
          ++currentDepth;
          for (int index = 0; index < node.ChildNodes.Count; ++index)
          {
            ModelValidationState? validity2 = this.GetValidity(node.ChildNodes[index], currentDepth);
            ModelValidationState? nullable = validity2;
            ModelValidationState modelValidationState1 = ModelValidationState.Unvalidated;
            if (nullable.GetValueOrDefault() == modelValidationState1 & nullable.HasValue)
              return validity2;
            if (validity1.HasValue)
            {
              nullable = validity2;
              ModelValidationState modelValidationState2 = ModelValidationState.Invalid;
              if (!(nullable.GetValueOrDefault() == modelValidationState2 & nullable.HasValue))
                continue;
            }
            validity1 = validity2;
          }
        }
        return validity1;
      }
label_3:
      return new ModelValidationState?();
    }

    private void EnsureMaxErrorsReachedRecorded()
    {
      if (this.HasRecordedMaxModelError)
        return;
      TooManyModelErrorsException modelErrorsException = new TooManyModelErrorsException(Resources.ModelStateDictionary_MaxModelStateErrors);
      this.AddModelErrorCore(string.Empty, (Exception) modelErrorsException);
      this.HasRecordedMaxModelError = true;
    }

    private void AddModelErrorCore(string key, Exception exception)
    {
      ModelStateDictionary.ModelStateNode orAddNode = this.GetOrAddNode(key);
      this.Count += !orAddNode.IsContainerNode ? 0 : 1;
      orAddNode.ValidationState = ModelValidationState.Invalid;
      orAddNode.MarkNonContainerNode();
      orAddNode.Errors.Add(exception);
      ++this.ErrorCount;
    }

    /// <summary>
    /// Removes all keys and values from this instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.
    /// </summary>
    public void Clear()
    {
      this.Count = 0;
      this.HasRecordedMaxModelError = false;
      this.ErrorCount = 0;
      this._root.Reset();
      this._root.ChildNodes?.Clear();
    }


    #nullable enable
    /// <inheritdoc />
    public bool ContainsKey(string key)
    {
      ModelStateDictionary.ModelStateNode modelStateNode = key != null ? this.GetNode(key) : throw new ArgumentNullException(nameof (key));
      return modelStateNode != null && !modelStateNode.IsContainerNode;
    }

    /// <summary>
    /// Removes the <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" /> with the specified <paramref name="key" />.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if the element is successfully removed; otherwise <c>false</c>. This method also
    /// returns <c>false</c> if key was not found.</returns>
    public bool Remove(string key)
    {
      ModelStateDictionary.ModelStateNode modelStateNode = key != null ? this.GetNode(key) : throw new ArgumentNullException(nameof (key));
      if (modelStateNode == null || modelStateNode.IsContainerNode)
        return false;
      --this.Count;
      this.ErrorCount -= modelStateNode.Errors.Count;
      modelStateNode.Reset();
      return true;
    }

    /// <inheritdoc />
    public bool TryGetValue(string key, [NotNullWhen(true)] out ModelStateEntry? value)
    {
      ModelStateDictionary.ModelStateNode modelStateNode = key != null ? this.GetNode(key) : throw new ArgumentNullException(nameof (key));
      if (modelStateNode != null && !modelStateNode.IsContainerNode)
      {
        value = (ModelStateEntry) modelStateNode;
        return true;
      }
      value = (ModelStateEntry) null;
      return false;
    }

    /// <summary>
    /// Returns an enumerator that iterates through this instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.
    /// </summary>
    /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.Enumerator" />.</returns>
    public ModelStateDictionary.Enumerator GetEnumerator() => new ModelStateDictionary.Enumerator(this, string.Empty);


    #nullable disable
    IEnumerator<KeyValuePair<string, ModelStateEntry>> IEnumerable<KeyValuePair<string, ModelStateEntry>>.GetEnumerator() => (IEnumerator<KeyValuePair<string, ModelStateEntry>>) this.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();


    #nullable enable
    /// <summary>
    /// <para>
    /// This API supports the MVC's infrastructure and is not intended to be used
    /// directly from your code. This API may change or be removed in future releases.
    /// </para>
    /// </summary>
    public static bool StartsWithPrefix(string prefix, string key)
    {
      if (prefix == null)
        throw new ArgumentNullException(nameof (prefix));
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (prefix.Length == 0)
        return true;
      if (prefix.Length > key.Length || !key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        return false;
      if (key.Length == prefix.Length)
        return true;
      switch (key[prefix.Length])
      {
        case '.':
        case '[':
          return true;
        default:
          return false;
      }
    }

    /// <summary>
    /// Gets a <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.PrefixEnumerable" /> that iterates over this instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />
    /// using the specified <paramref name="prefix" />.
    /// </summary>
    /// <param name="prefix">The prefix.</param>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.PrefixEnumerable" />.</returns>
    public ModelStateDictionary.PrefixEnumerable FindKeysWithPrefix(string prefix) => prefix != null ? new ModelStateDictionary.PrefixEnumerable(this, prefix) : throw new ArgumentNullException(nameof (prefix));


    #nullable disable
    private struct MatchResult
    {
      public ModelStateDictionary.Delimiter Type;
      public int Index;
    }

    private enum Delimiter
    {
      None,
      Dot,
      OpenBracket,
    }

    [DebuggerDisplay("SubKey={SubKey}, Key={Key}, ValidationState={ValidationState}")]
    private sealed class ModelStateNode : ModelStateEntry
    {
      private bool _isContainerNode = true;

      public ModelStateNode(StringSegment subKey) => this.SubKey = subKey;

      public List<ModelStateDictionary.ModelStateNode> ChildNodes { get; set; }

      public override IReadOnlyList<ModelStateEntry> Children => (IReadOnlyList<ModelStateEntry>) this.ChildNodes;

      public string Key { get; set; }

      public StringSegment SubKey { get; }

      public override bool IsContainerNode => this._isContainerNode;

      public void MarkNonContainerNode() => this._isContainerNode = false;

      public void Copy(ModelStateEntry entry)
      {
        this.RawValue = entry.RawValue;
        this.AttemptedValue = entry.AttemptedValue;
        this.Errors.Clear();
        for (int index = 0; index < entry.Errors.Count; ++index)
          this.Errors.Add(entry.Errors[index]);
        this.ValidationState = entry.ValidationState;
      }

      public void Reset()
      {
        this._isContainerNode = true;
        this.RawValue = (object) null;
        this.AttemptedValue = (string) null;
        this.ValidationState = ModelValidationState.Unvalidated;
        this.Errors.Clear();
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public ModelStateDictionary.ModelStateNode GetNode(StringSegment subKey)
      {
        ModelStateDictionary.ModelStateNode node = (ModelStateDictionary.ModelStateNode) null;
        if (subKey.Length == 0)
          node = this;
        else if (this.ChildNodes != null)
        {
          int index = this.BinarySearch(subKey);
          if (index >= 0)
            node = this.ChildNodes[index];
        }
        return node;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public ModelStateDictionary.ModelStateNode GetOrAddNode(StringSegment subKey)
      {
        ModelStateDictionary.ModelStateNode orAddNode;
        if (subKey.Length == 0)
          orAddNode = this;
        else if (this.ChildNodes == null)
        {
          this.ChildNodes = new List<ModelStateDictionary.ModelStateNode>(1);
          orAddNode = new ModelStateDictionary.ModelStateNode(subKey);
          this.ChildNodes.Add(orAddNode);
        }
        else
        {
          int index = this.BinarySearch(subKey);
          if (index >= 0)
          {
            orAddNode = this.ChildNodes[index];
          }
          else
          {
            orAddNode = new ModelStateDictionary.ModelStateNode(subKey);
            this.ChildNodes.Insert(~index, orAddNode);
          }
        }
        return orAddNode;
      }

      public override ModelStateEntry GetModelStateForProperty(string propertyName) => (ModelStateEntry) this.GetNode(new StringSegment(propertyName));

      private int BinarySearch(StringSegment searchKey)
      {
        int num1 = 0;
        int num2 = this.ChildNodes.Count - 1;
        while (num1 <= num2)
        {
          int index = num1 + (num2 - num1) / 2;
          StringSegment subKey = this.ChildNodes[index].SubKey;
          int num3 = subKey.Length - searchKey.Length;
          if (num3 == 0)
            num3 = string.Compare(subKey.Buffer, subKey.Offset, searchKey.Buffer, searchKey.Offset, searchKey.Length, StringComparison.OrdinalIgnoreCase);
          if (num3 == 0)
            return index;
          if (num3 < 0)
            num1 = index + 1;
          else
            num2 = index - 1;
        }
        return ~num1;
      }
    }


    #nullable enable
    /// <summary>
    /// Enumerates over <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" /> to provide entries that start with the
    /// specified prefix.
    /// </summary>
    public readonly struct PrefixEnumerable : 
      IEnumerable<KeyValuePair<string, ModelStateEntry>>,
      IEnumerable
    {

      #nullable disable
      private readonly ModelStateDictionary _dictionary;
      private readonly string _prefix;


      #nullable enable
      /// <summary>
      /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.PrefixEnumerable" />.
      /// </summary>
      /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.</param>
      /// <param name="prefix">The prefix.</param>
      public PrefixEnumerable(ModelStateDictionary dictionary, string prefix)
      {
        if (dictionary == null)
          throw new ArgumentNullException(nameof (dictionary));
        if (prefix == null)
          throw new ArgumentNullException(nameof (prefix));
        this._dictionary = dictionary;
        this._prefix = prefix;
      }

      /// <inheritdoc />
      public ModelStateDictionary.Enumerator GetEnumerator() => new ModelStateDictionary.Enumerator(this._dictionary, this._prefix);


      #nullable disable
      IEnumerator<KeyValuePair<string, ModelStateEntry>> IEnumerable<KeyValuePair<string, ModelStateEntry>>.GetEnumerator() => (IEnumerator<KeyValuePair<string, ModelStateEntry>>) this.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }


    #nullable enable
    /// <summary>
    /// An <see cref="T:System.Collections.Generic.IEnumerator`1" /> for <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.PrefixEnumerable" />.
    /// </summary>
    public struct Enumerator : 
      IEnumerator<KeyValuePair<string, ModelStateEntry>>,
      IEnumerator,
      IDisposable
    {

      #nullable disable
      private readonly ModelStateDictionary.ModelStateNode _rootNode;
      private ModelStateDictionary.ModelStateNode _modelStateNode;
      private List<ModelStateDictionary.ModelStateNode> _nodes;
      private int _index;
      private bool _visitedRoot;


      #nullable enable
      /// <summary>
      /// Intializes a new instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.Enumerator" />.
      /// </summary>
      /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.</param>
      /// <param name="prefix">The prefix.</param>
      public Enumerator(ModelStateDictionary dictionary, string prefix)
      {
        if (dictionary == null)
          throw new ArgumentNullException(nameof (dictionary));
        if (prefix == null)
          throw new ArgumentNullException(nameof (prefix));
        this._index = -1;
        this._rootNode = dictionary.GetNode(prefix);
        this._modelStateNode = (ModelStateDictionary.ModelStateNode) null;
        this._nodes = (List<ModelStateDictionary.ModelStateNode>) null;
        this._visitedRoot = false;
      }

      /// <inheritdoc />
      public KeyValuePair<string, ModelStateEntry> Current => new KeyValuePair<string, ModelStateEntry>(this._modelStateNode.Key, (ModelStateEntry) this._modelStateNode);

      object IEnumerator.Current => (object) this.Current;

      /// <inheritdoc />
      public void Dispose()
      {
      }

      /// <inheritdoc />
      public bool MoveNext()
      {
        if (this._rootNode == null)
          return false;
        if (!this._visitedRoot)
        {
          this._visitedRoot = true;
          List<ModelStateDictionary.ModelStateNode> childNodes = this._rootNode.ChildNodes;
          // ISSUE: explicit non-virtual call
          if ((childNodes != null ? (__nonvirtual (childNodes.Count) > 0 ? 1 : 0) : 0) != 0)
            this._nodes = new List<ModelStateDictionary.ModelStateNode>()
            {
              this._rootNode
            };
          if (!this._rootNode.IsContainerNode)
          {
            this._modelStateNode = this._rootNode;
            return true;
          }
        }
        if (this._nodes == null)
          return false;
        while (this._nodes.Count > 0)
        {
          ModelStateDictionary.ModelStateNode node = this._nodes[0];
          if (this._index == node.ChildNodes.Count - 1)
          {
            this._nodes.RemoveAt(0);
            this._index = -1;
          }
          else
          {
            ++this._index;
            ModelStateDictionary.ModelStateNode childNode = node.ChildNodes[this._index];
            List<ModelStateDictionary.ModelStateNode> childNodes = childNode.ChildNodes;
            // ISSUE: explicit non-virtual call
            if ((childNodes != null ? (__nonvirtual (childNodes.Count) > 0 ? 1 : 0) : 0) != 0)
              this._nodes.Add(childNode);
            if (!childNode.IsContainerNode)
            {
              this._modelStateNode = childNode;
              return true;
            }
          }
        }
        return false;
      }

      /// <inheritdoc />
      public void Reset()
      {
        this._index = -1;
        this._nodes?.Clear();
        this._visitedRoot = false;
        this._modelStateNode = (ModelStateDictionary.ModelStateNode) null;
      }
    }

    /// <summary>
    /// A <see cref="T:System.Collections.Generic.IEnumerable`1" /> for keys in <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.
    /// </summary>
    public readonly struct KeyEnumerable : IEnumerable<string>, IEnumerable
    {

      #nullable disable
      private readonly ModelStateDictionary _dictionary;


      #nullable enable
      /// <summary>
      /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.KeyEnumerable" />.
      /// </summary>
      /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.</param>
      public KeyEnumerable(ModelStateDictionary dictionary) => this._dictionary = dictionary;

      /// <inheritdoc />
      public ModelStateDictionary.KeyEnumerator GetEnumerator() => new ModelStateDictionary.KeyEnumerator(this._dictionary, string.Empty);


      #nullable disable
      IEnumerator<string> IEnumerable<string>.GetEnumerator() => (IEnumerator<string>) this.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }


    #nullable enable
    /// <summary>
    /// An <see cref="T:System.Collections.Generic.IEnumerator`1" /> for keys in <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.
    /// </summary>
    public struct KeyEnumerator : IEnumerator<string>, IEnumerator, IDisposable
    {
      private ModelStateDictionary.Enumerator _prefixEnumerator;

      /// <summary>
      /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.KeyEnumerable" />.
      /// </summary>
      /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.</param>
      /// <param name="prefix">The prefix.</param>
      public KeyEnumerator(ModelStateDictionary dictionary, string prefix)
      {
        this._prefixEnumerator = new ModelStateDictionary.Enumerator(dictionary, prefix);
        this.Current = (string) null;
      }

      /// <inheritdoc />
      public string Current { get; private set; }

      object IEnumerator.Current => (object) this.Current;

      /// <inheritdoc />
      public void Dispose() => this._prefixEnumerator.Dispose();

      /// <inheritdoc />
      public bool MoveNext()
      {
        int num = this._prefixEnumerator.MoveNext() ? 1 : 0;
        if (num != 0)
        {
          this.Current = this._prefixEnumerator.Current.Key;
          return num != 0;
        }
        this.Current = (string) null;
        return num != 0;
      }

      /// <inheritdoc />
      public void Reset()
      {
        this._prefixEnumerator.Reset();
        this.Current = (string) null;
      }
    }

    /// <summary>
    /// An <see cref="T:System.Collections.IEnumerable" /> for <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" />.
    /// </summary>
    public readonly struct ValueEnumerable : IEnumerable<ModelStateEntry>, IEnumerable
    {

      #nullable disable
      private readonly ModelStateDictionary _dictionary;


      #nullable enable
      /// <summary>
      /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.ValueEnumerable" />.
      /// </summary>
      /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.</param>
      public ValueEnumerable(ModelStateDictionary dictionary) => this._dictionary = dictionary;

      /// <inheritdoc />
      public ModelStateDictionary.ValueEnumerator GetEnumerator() => new ModelStateDictionary.ValueEnumerator(this._dictionary, string.Empty);


      #nullable disable
      IEnumerator<ModelStateEntry> IEnumerable<ModelStateEntry>.GetEnumerator() => (IEnumerator<ModelStateEntry>) this.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }


    #nullable enable
    /// <summary>
    /// An enumerator for <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry" />.
    /// </summary>
    public struct ValueEnumerator : IEnumerator<ModelStateEntry>, IEnumerator, IDisposable
    {
      private ModelStateDictionary.Enumerator _prefixEnumerator;

      /// <summary>
      /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary.ValueEnumerator" />.
      /// </summary>
      /// <param name="dictionary">The <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />.</param>
      /// <param name="prefix">The prefix to enumerate.</param>
      public ValueEnumerator(ModelStateDictionary dictionary, string prefix)
      {
        this._prefixEnumerator = new ModelStateDictionary.Enumerator(dictionary, prefix);
        this.Current = (ModelStateEntry) null;
      }

      /// <inheritdoc />
      public ModelStateEntry Current { get; private set; }

      object IEnumerator.Current => (object) this.Current;

      /// <inheritdoc />
      public void Dispose() => this._prefixEnumerator.Dispose();

      /// <inheritdoc />
      public bool MoveNext()
      {
        int num = this._prefixEnumerator.MoveNext() ? 1 : 0;
        if (num != 0)
        {
          this.Current = this._prefixEnumerator.Current.Value;
          return num != 0;
        }
        this.Current = (ModelStateEntry) null;
        return num != 0;
      }

      /// <inheritdoc />
      public void Reset()
      {
        this._prefixEnumerator.Reset();
        this.Current = (ModelStateEntry) null;
      }
    }
  }
}
