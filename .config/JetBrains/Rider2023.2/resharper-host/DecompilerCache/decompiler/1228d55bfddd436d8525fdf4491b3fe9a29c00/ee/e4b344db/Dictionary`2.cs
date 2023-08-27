// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.Dictionary`2
// Assembly: System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 1228D55B-FDDD-436D-8525-FDF4491B3FE9
// Assembly location: /usr/lib/dotnet/shared/Microsoft.NETCore.App/7.0.9/System.Private.CoreLib.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.NETCore.App.Ref/7.0.9/ref/net7.0/System.Collections.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Collections.Generic
{
  /// <summary>Represents a collection of keys and values.</summary>
  /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
  /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
  [DebuggerTypeProxy(typeof (IDictionaryDebugView<,>))]
  [DebuggerDisplay("Count = {Count}")]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class Dictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable,
    IDictionary,
    ICollection,
    IReadOnlyDictionary<TKey, TValue>,
    IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
    ISerializable,
    IDeserializationCallback
    where TKey : notnull
  {

    #nullable disable
    private int[] _buckets;
    private Dictionary<TKey, TValue>.Entry[] _entries;
    private ulong _fastModMultiplier;
    private int _count;
    private int _freeList;
    private int _freeCount;
    private int _version;
    private IEqualityComparer<TKey> _comparer;
    private Dictionary<TKey, TValue>.KeyCollection _keys;
    private Dictionary<TKey, TValue>.ValueCollection _values;

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.</summary>
    public Dictionary()
      : this(0, (IEqualityComparer<TKey>) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.</summary>
    /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than 0.</exception>
    public Dictionary(int capacity)
      : this(capacity, (IEqualityComparer<TKey>) null)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
    public Dictionary(IEqualityComparer<TKey>? comparer)
      : this(0, comparer)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
    /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than 0.</exception>
    public Dictionary(int capacity, IEqualityComparer<TKey>? comparer)
    {
      if (capacity < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
      if (capacity > 0)
        this.Initialize(capacity);
      if (comparer != null && comparer != EqualityComparer<TKey>.Default)
        this._comparer = comparer;
      if (!(typeof (TKey) == typeof (string)))
        return;
      IEqualityComparer<string> stringComparer = NonRandomizedStringEqualityComparer.GetStringComparer((object) this._comparer);
      if (stringComparer == null)
        return;
      this._comparer = (IEqualityComparer<TKey>) stringComparer;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the default equality comparer for the key type.</summary>
    /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="dictionary" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
    public Dictionary(IDictionary<TKey, TValue> dictionary)
      : this(dictionary, (IEqualityComparer<TKey>) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
    /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="dictionary" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="dictionary" /> contains one or more duplicate keys.</exception>
    public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer)
      : this(dictionary != null ? dictionary.Count : 0, comparer)
    {
      if (dictionary == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
      this.AddRange((IEnumerable<KeyValuePair<TKey, TValue>>) dictionary);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IEnumerable`1" />.</summary>
    /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" />  whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="collection" /> contains one or more duplicated keys.</exception>
    public Dictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
      : this(collection, (IEqualityComparer<TKey>) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IEnumerable`1" /> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.</summary>
    /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1" /> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or <see langword="null" /> to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="collection" /> contains one or more duplicated keys.</exception>
    public Dictionary(
      IEnumerable<KeyValuePair<TKey, TValue>> collection,
      IEqualityComparer<TKey>? comparer)
      : this(collection is ICollection<KeyValuePair<TKey, TValue>> keyValuePairs ? keyValuePairs.Count : 0, comparer)
    {
      if (collection == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
      this.AddRange(collection);
    }


    #nullable disable
    private void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
      if (collection.GetType() == typeof (Dictionary<TKey, TValue>))
      {
        Dictionary<TKey, TValue> dictionary = (Dictionary<TKey, TValue>) collection;
        if (dictionary.Count == 0)
          return;
        Dictionary<TKey, TValue>.Entry[] entries = dictionary._entries;
        if (dictionary._comparer == this._comparer)
        {
          this.CopyEntries(entries, dictionary._count);
        }
        else
        {
          int count = dictionary._count;
          for (int index = 0; index < count; ++index)
          {
            if (entries[index].next >= -1)
              this.Add(entries[index].key, entries[index].value);
          }
        }
      }
      else
      {
        foreach (KeyValuePair<TKey, TValue> keyValuePair in collection)
          this.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class with serialized data.</summary>
    /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    protected Dictionary(SerializationInfo info, StreamingContext context) => HashHelpers.SerializationInfoTable.Add((object) this, info);

    /// <summary>Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> that is used to determine equality of keys for the dictionary.</summary>
    /// <returns>The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> generic interface implementation that is used to determine equality of keys for the current <see cref="T:System.Collections.Generic.Dictionary`2" /> and to provide hash values for the keys.</returns>
    public IEqualityComparer<TKey> Comparer => typeof (TKey) == typeof (string) ? (IEqualityComparer<TKey>) IInternalStringEqualityComparer.GetUnderlyingEqualityComparer((IEqualityComparer<string>) this._comparer) : this._comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;

    /// <summary>Gets the number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
    /// <returns>The number of key/value pairs contained in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
    public int Count => this._count - this._freeCount;

    /// <summary>Gets a collection containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
    /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> containing the keys in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
    public Dictionary<
    #nullable disable
    TKey, TValue>.KeyCollection Keys => this._keys ?? (this._keys = new Dictionary<TKey, TValue>.KeyCollection(this));


    #nullable enable
    /// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> of type <paramref name="TKey" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
    ICollection<TKey> IDictionary<
    #nullable disable
    TKey, TValue>.Keys => (ICollection<TKey>) this.Keys;


    #nullable enable
    /// <summary>Gets a collection containing the keys of the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" />.</summary>
    /// <returns>A collection containing the keys of the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" />.</returns>
    IEnumerable<TKey> IReadOnlyDictionary<
    #nullable disable
    TKey, TValue>.Keys => (IEnumerable<TKey>) this.Keys;


    #nullable enable
    /// <summary>Gets a collection containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
    /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> containing the values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
    public Dictionary<
    #nullable disable
    TKey, TValue>.ValueCollection Values => this._values ?? (this._values = new Dictionary<TKey, TValue>.ValueCollection(this));


    #nullable enable
    /// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> of type <paramref name="TValue" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
    ICollection<TValue> IDictionary<
    #nullable disable
    TKey, TValue>.Values => (ICollection<TValue>) this.Values;


    #nullable enable
    /// <summary>Gets a collection containing the values of the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" />.</summary>
    /// <returns>A collection containing the values of the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" />.</returns>
    IEnumerable<TValue> IReadOnlyDictionary<
    #nullable disable
    TKey, TValue>.Values => (IEnumerable<TValue>) this.Values;


    #nullable enable
    /// <summary>Gets or sets the value associated with the specified key.</summary>
    /// <param name="key">The key of the value to get or set.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> does not exist in the collection.</exception>
    /// <returns>The value associated with the specified key. If the specified key is not found, a get operation throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />, and a set operation creates a new element with the specified key.</returns>
    public TValue this[TKey key]
    {
      get
      {
        ref TValue local = ref this.FindValue(key);
        if (!Unsafe.IsNullRef<TValue>(ref local))
          return local;
        ThrowHelper.ThrowKeyNotFoundException<TKey>(key);
        return default (TValue);
      }
      set => this.TryInsert(key, value, InsertionBehavior.OverwriteExisting);
    }

    /// <summary>Adds the specified key and value to the dictionary.</summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add. The value can be <see langword="null" /> for reference types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
    public void Add(TKey key, TValue value) => this.TryInsert(key, value, InsertionBehavior.ThrowOnExisting);


    #nullable disable
    /// <summary>Adds the specified value to the <see cref="T:System.Collections.Generic.ICollection`1" /> with the specified key.</summary>
    /// <param name="keyValuePair">The <see cref="T:System.Collections.Generic.KeyValuePair`2" /> structure representing the key and value to add to the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The key of <paramref name="keyValuePair" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair) => this.Add(keyValuePair.Key, keyValuePair.Value);

    /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific key and value.</summary>
    /// <param name="keyValuePair">The <see cref="T:System.Collections.Generic.KeyValuePair`2" /> structure to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="keyValuePair" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
    {
      ref TValue local = ref this.FindValue(keyValuePair.Key);
      return !Unsafe.IsNullRef<TValue>(ref local) && EqualityComparer<TValue>.Default.Equals(local, keyValuePair.Value);
    }

    /// <summary>Removes a key and value from the dictionary.</summary>
    /// <param name="keyValuePair">The <see cref="T:System.Collections.Generic.KeyValuePair`2" /> structure representing the key and value to remove from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <returns>
    /// <see langword="true" /> if the key and value represented by <paramref name="keyValuePair" /> is successfully found and removed; otherwise, <see langword="false" />. This method returns <see langword="false" /> if <paramref name="keyValuePair" /> is not found in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
    {
      ref TValue local = ref this.FindValue(keyValuePair.Key);
      if (Unsafe.IsNullRef<TValue>(ref local) || !EqualityComparer<TValue>.Default.Equals(local, keyValuePair.Value))
        return false;
      this.Remove(keyValuePair.Key);
      return true;
    }

    /// <summary>Removes all keys and values from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
    public void Clear()
    {
      int count = this._count;
      if (count <= 0)
        return;
      Array.Clear((Array) this._buckets);
      this._count = 0;
      this._freeList = -1;
      this._freeCount = 0;
      Array.Clear((Array) this._entries, 0, count);
    }


    #nullable enable
    /// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains the specified key.</summary>
    /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
    public bool ContainsKey(TKey key) => !Unsafe.IsNullRef<TValue>(ref this.FindValue(key));

    /// <summary>Determines whether the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains a specific value.</summary>
    /// <param name="value">The value to locate in the <see cref="T:System.Collections.Generic.Dictionary`2" />. The value can be <see langword="null" /> for reference types.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified value; otherwise, <see langword="false" />.</returns>
    public bool ContainsValue(TValue value)
    {
      Dictionary<TKey, TValue>.Entry[] entries = this._entries;
      if ((object) value == null)
      {
        for (int index = 0; index < this._count; ++index)
        {
          if (entries[index].next >= -1 && (object) entries[index].value == null)
            return true;
        }
      }
      else if (typeof (TValue).IsValueType)
      {
        for (int index = 0; index < this._count; ++index)
        {
          if (entries[index].next >= -1 && EqualityComparer<TValue>.Default.Equals(entries[index].value, value))
            return true;
        }
      }
      else
      {
        EqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;
        for (int index = 0; index < this._count; ++index)
        {
          if (entries[index].next >= -1 && equalityComparer.Equals(entries[index].value, value))
            return true;
        }
      }
      return false;
    }


    #nullable disable
    private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if ((uint) index > (uint) array.Length)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (array.Length - index < this.Count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
      int count = this._count;
      Dictionary<TKey, TValue>.Entry[] entries = this._entries;
      for (int index1 = 0; index1 < count; ++index1)
      {
        if (entries[index1].next >= -1)
          array[index++] = new KeyValuePair<TKey, TValue>(entries[index1].key, entries[index1].value);
      }
    }


    #nullable enable
    /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
    /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" /> structure for the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
    public Dictionary<
    #nullable disable
    TKey, TValue>.Enumerator GetEnumerator() => new Dictionary<TKey, TValue>.Enumerator(this, 2);

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => (IEnumerator<KeyValuePair<TKey, TValue>>) new Dictionary<TKey, TValue>.Enumerator(this, 2);


    #nullable enable
    /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</summary>
    /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
    /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" /> instance.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
      info.AddValue("Version", this._version);
      info.AddValue("Comparer", (object) this.Comparer, typeof (IEqualityComparer<TKey>));
      info.AddValue("HashSize", this._buckets == null ? 0 : this._buckets.Length);
      if (this._buckets == null)
        return;
      KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.Count];
      this.CopyTo(array, 0);
      info.AddValue("KeyValuePairs", (object) array, typeof (KeyValuePair<TKey, TValue>[]));
    }


    #nullable disable
    internal ref TValue FindValue(TKey key)
    {
      if ((object) key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      Unsafe.NullRef<Dictionary<TKey, TValue>.Entry>();
      // ISSUE: variable of a reference type
      TValue& local1;
      if (this._buckets != null)
      {
        IEqualityComparer<TKey> comparer = this._comparer;
        // ISSUE: variable of a reference type
        Dictionary<TKey, TValue>.Entry& local2;
        if (comparer == null)
        {
          uint hashCode = (uint) key.GetHashCode();
          int num1 = this.GetBucket(hashCode);
          Dictionary<TKey, TValue>.Entry[] entries = this._entries;
          uint num2 = 0;
          if (typeof (TKey).IsValueType)
          {
            int index = num1 - 1;
            while ((uint) index < (uint) entries.Length)
            {
              local2 = ref entries[index];
              if ((int) local2.hashCode != (int) hashCode || !EqualityComparer<TKey>.Default.Equals(local2.key, key))
              {
                index = local2.next;
                ++num2;
                if (num2 > (uint) entries.Length)
                  goto label_17;
              }
              else
                goto label_18;
            }
            goto label_20;
          }
          else
          {
            EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
            int index = num1 - 1;
            while ((uint) index < (uint) entries.Length)
            {
              local2 = ref entries[index];
              if ((int) local2.hashCode != (int) hashCode || !equalityComparer.Equals(local2.key, key))
              {
                index = local2.next;
                ++num2;
                if (num2 > (uint) entries.Length)
                  goto label_17;
              }
              else
                goto label_18;
            }
            goto label_20;
          }
        }
        else
        {
          uint hashCode = (uint) comparer.GetHashCode(key);
          int num3 = this.GetBucket(hashCode);
          Dictionary<TKey, TValue>.Entry[] entries = this._entries;
          uint num4 = 0;
          int index = num3 - 1;
          while ((uint) index < (uint) entries.Length)
          {
            local2 = ref entries[index];
            if ((int) local2.hashCode != (int) hashCode || !comparer.Equals(local2.key, key))
            {
              index = local2.next;
              ++num4;
              if (num4 > (uint) entries.Length)
                goto label_17;
            }
            else
              goto label_18;
          }
          goto label_20;
        }
label_17:
        ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
label_18:
        local1 = ref local2.value;
      }
      else
        goto label_20;
label_19:
      return ref local1;
label_20:
      local1 = ref Unsafe.NullRef<TValue>();
      goto label_19;
    }

    private int Initialize(int capacity)
    {
      int prime = HashHelpers.GetPrime(capacity);
      int[] numArray = new int[prime];
      Dictionary<TKey, TValue>.Entry[] entryArray = new Dictionary<TKey, TValue>.Entry[prime];
      this._freeList = -1;
      this._fastModMultiplier = HashHelpers.GetFastModMultiplier((uint) prime);
      this._buckets = numArray;
      this._entries = entryArray;
      return prime;
    }

    private bool TryInsert(TKey key, TValue value, InsertionBehavior behavior)
    {
      if ((object) key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      if (this._buckets == null)
        this.Initialize(0);
      Dictionary<TKey, TValue>.Entry[] entries = this._entries;
      IEqualityComparer<TKey> comparer = this._comparer;
      uint hashCode = comparer == null ? (uint) key.GetHashCode() : (uint) comparer.GetHashCode(key);
      uint num = 0;
      ref int local1 = ref this.GetBucket(hashCode);
      int index1 = local1 - 1;
      if (comparer == null)
      {
        if (typeof (TKey).IsValueType)
        {
          while ((uint) index1 < (uint) entries.Length)
          {
            if ((int) entries[index1].hashCode == (int) hashCode && EqualityComparer<TKey>.Default.Equals(entries[index1].key, key))
            {
              switch (behavior)
              {
                case InsertionBehavior.OverwriteExisting:
                  entries[index1].value = value;
                  return true;
                case InsertionBehavior.ThrowOnExisting:
                  ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException<TKey>(key);
                  break;
              }
              return false;
            }
            index1 = entries[index1].next;
            ++num;
            if (num > (uint) entries.Length)
              ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
          }
        }
        else
        {
          EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
          while ((uint) index1 < (uint) entries.Length)
          {
            if ((int) entries[index1].hashCode == (int) hashCode && equalityComparer.Equals(entries[index1].key, key))
            {
              switch (behavior)
              {
                case InsertionBehavior.OverwriteExisting:
                  entries[index1].value = value;
                  return true;
                case InsertionBehavior.ThrowOnExisting:
                  ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException<TKey>(key);
                  break;
              }
              return false;
            }
            index1 = entries[index1].next;
            ++num;
            if (num > (uint) entries.Length)
              ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
          }
        }
      }
      else
      {
        while ((uint) index1 < (uint) entries.Length)
        {
          if ((int) entries[index1].hashCode == (int) hashCode && comparer.Equals(entries[index1].key, key))
          {
            switch (behavior)
            {
              case InsertionBehavior.OverwriteExisting:
                entries[index1].value = value;
                return true;
              case InsertionBehavior.ThrowOnExisting:
                ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException<TKey>(key);
                break;
            }
            return false;
          }
          index1 = entries[index1].next;
          ++num;
          if (num > (uint) entries.Length)
            ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
        }
      }
      int index2;
      if (this._freeCount > 0)
      {
        index2 = this._freeList;
        this._freeList = -3 - entries[this._freeList].next;
        --this._freeCount;
      }
      else
      {
        int count = this._count;
        if (count == entries.Length)
        {
          this.Resize();
          local1 = ref this.GetBucket(hashCode);
        }
        index2 = count;
        this._count = count + 1;
        entries = this._entries;
      }
      ref Dictionary<TKey, TValue>.Entry local2 = ref entries[index2];
      local2.hashCode = hashCode;
      local2.next = local1 - 1;
      local2.key = key;
      local2.value = value;
      local1 = index2 + 1;
      ++this._version;
      if (!typeof (TKey).IsValueType && num > 100U && comparer is NonRandomizedStringEqualityComparer)
        this.Resize(entries.Length, true);
      return true;
    }


    #nullable enable
    /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
    /// <param name="sender">The source of the deserialization event.</param>
    /// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object associated with the current <see cref="T:System.Collections.Generic.Dictionary`2" /> instance is invalid.</exception>
    public virtual void OnDeserialization(object? sender)
    {
      SerializationInfo serializationInfo;
      HashHelpers.SerializationInfoTable.TryGetValue((object) this, out serializationInfo);
      if (serializationInfo == null)
        return;
      int int32_1 = serializationInfo.GetInt32("Version");
      int int32_2 = serializationInfo.GetInt32("HashSize");
      this._comparer = (IEqualityComparer<TKey>) serializationInfo.GetValue("Comparer", typeof (IEqualityComparer<TKey>));
      if (int32_2 != 0)
      {
        this.Initialize(int32_2);
        KeyValuePair<TKey, TValue>[] keyValuePairArray = (KeyValuePair<TKey, TValue>[]) serializationInfo.GetValue("KeyValuePairs", typeof (KeyValuePair<TKey, TValue>[]));
        if (keyValuePairArray == null)
          ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingKeys);
        for (int index = 0; index < keyValuePairArray.Length; ++index)
        {
          if ((object) keyValuePairArray[index].Key == null)
            ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_NullKey);
          this.Add(keyValuePairArray[index].Key, keyValuePairArray[index].Value);
        }
      }
      else
        this._buckets = (int[]) null;
      this._version = int32_1;
      HashHelpers.SerializationInfoTable.Remove((object) this);
    }

    private void Resize() => this.Resize(HashHelpers.ExpandPrime(this._count), false);

    private void Resize(int newSize, bool forceNewHashCodes)
    {
      Dictionary<TKey, TValue>.Entry[] destinationArray = new Dictionary<TKey, TValue>.Entry[newSize];
      int count = this._count;
      Array.Copy((Array) this._entries, (Array) destinationArray, count);
      if (!typeof (TKey).IsValueType & forceNewHashCodes)
      {
        this._comparer = (IEqualityComparer<TKey>) ((NonRandomizedStringEqualityComparer) this._comparer).GetRandomizedEqualityComparer();
        for (int index = 0; index < count; ++index)
        {
          if (destinationArray[index].next >= -1)
            destinationArray[index].hashCode = (uint) this._comparer.GetHashCode(destinationArray[index].key);
        }
        if (this._comparer == EqualityComparer<TKey>.Default)
          this._comparer = (IEqualityComparer<TKey>) null;
      }
      this._buckets = new int[newSize];
      this._fastModMultiplier = HashHelpers.GetFastModMultiplier((uint) newSize);
      for (int index = 0; index < count; ++index)
      {
        if (destinationArray[index].next >= -1)
        {
          ref int local = ref this.GetBucket(destinationArray[index].hashCode);
          destinationArray[index].next = local - 1;
          local = index + 1;
        }
      }
      this._entries = destinationArray;
    }

    /// <summary>Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the element is successfully found and removed; otherwise, <see langword="false" />.  This method returns <see langword="false" /> if <paramref name="key" /> is not found in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
    public bool Remove(TKey key)
    {
      if ((object) key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      if (this._buckets != null)
      {
        uint num = 0;
        IEqualityComparer<TKey> comparer1 = this._comparer;
        uint hashCode = comparer1 != null ? (uint) comparer1.GetHashCode(key) : (uint) key.GetHashCode();
        ref int local1 = ref this.GetBucket(hashCode);
        Dictionary<TKey, TValue>.Entry[] entries = this._entries;
        int index1 = -1;
        int index2 = local1 - 1;
        while (index2 >= 0)
        {
          ref Dictionary<TKey, TValue>.Entry local2 = ref entries[index2];
          if ((int) local2.hashCode == (int) hashCode)
          {
            IEqualityComparer<TKey> comparer2 = this._comparer;
            if ((comparer2 != null ? (comparer2.Equals(local2.key, key) ? 1 : 0) : (EqualityComparer<TKey>.Default.Equals(local2.key, key) ? 1 : 0)) != 0)
            {
              if (index1 < 0)
                local1 = local2.next + 1;
              else
                entries[index1].next = local2.next;
              local2.next = -3 - this._freeList;
              if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
                local2.key = default (TKey);
              if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
                local2.value = default (TValue);
              this._freeList = index2;
              ++this._freeCount;
              return true;
            }
          }
          index1 = index2;
          index2 = local2.next;
          ++num;
          if (num > (uint) entries.Length)
            ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
        }
      }
      return false;
    }

    /// <summary>Removes the value with the specified key from the <see cref="T:System.Collections.Generic.Dictionary`2" />, and copies the element to the <paramref name="value" /> parameter.</summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <param name="value">The removed element.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the element is successfully found and removed; otherwise, <see langword="false" />.</returns>
    public bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
      if ((object) key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      if (this._buckets != null)
      {
        uint num = 0;
        IEqualityComparer<TKey> comparer1 = this._comparer;
        uint hashCode = comparer1 != null ? (uint) comparer1.GetHashCode(key) : (uint) key.GetHashCode();
        ref int local1 = ref this.GetBucket(hashCode);
        Dictionary<TKey, TValue>.Entry[] entries = this._entries;
        int index1 = -1;
        int index2 = local1 - 1;
        while (index2 >= 0)
        {
          ref Dictionary<TKey, TValue>.Entry local2 = ref entries[index2];
          if ((int) local2.hashCode == (int) hashCode)
          {
            IEqualityComparer<TKey> comparer2 = this._comparer;
            if ((comparer2 != null ? (comparer2.Equals(local2.key, key) ? 1 : 0) : (EqualityComparer<TKey>.Default.Equals(local2.key, key) ? 1 : 0)) != 0)
            {
              if (index1 < 0)
                local1 = local2.next + 1;
              else
                entries[index1].next = local2.next;
              value = local2.value;
              local2.next = -3 - this._freeList;
              if (RuntimeHelpers.IsReferenceOrContainsReferences<TKey>())
                local2.key = default (TKey);
              if (RuntimeHelpers.IsReferenceOrContainsReferences<TValue>())
                local2.value = default (TValue);
              this._freeList = index2;
              ++this._freeCount;
              return true;
            }
          }
          index1 = index2;
          index2 = local2.next;
          ++num;
          if (num > (uint) entries.Length)
            ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
        }
      }
      value = default (TValue);
      return false;
    }

    /// <summary>Gets the value associated with the specified key.</summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.Dictionary`2" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
      ref TValue local = ref this.FindValue(key);
      if (!Unsafe.IsNullRef<TValue>(ref local))
      {
        value = local;
        return true;
      }
      value = default (TValue);
      return false;
    }

    /// <summary>Attempts to add the specified key and value to the dictionary.</summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add. It can be <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the key/value pair was added to the dictionary successfully; otherwise, <see langword="false" />.</returns>
    public bool TryAdd(TKey key, TValue value) => this.TryInsert(key, value, InsertionBehavior.None);

    /// <summary>Gets a value that indicates whether the dictionary is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />. In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns <see langword="false" />.</returns>
    bool ICollection<KeyValuePair<
    #nullable disable
    TKey, TValue>>.IsReadOnly => false;

    /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an array of type <see cref="T:System.Collections.Generic.KeyValuePair`2" />, starting at the specified array index.</summary>
    /// <param name="array">The one-dimensional array of type <see cref="T:System.Collections.Generic.KeyValuePair`2" /> that is the destination of the <see cref="T:System.Collections.Generic.KeyValuePair`2" /> elements copied from the <see cref="T:System.Collections.Generic.ICollection`1" />. The array must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
      KeyValuePair<TKey, TValue>[] array,
      int index)
    {
      this.CopyTo(array, index);
    }

    /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an array, starting at the specified array index.</summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The array must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// <paramref name="array" /> does not have zero-based indexing.
    /// 
    /// -or-
    /// 
    /// The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// The type of the source <see cref="T:System.Collections.Generic.ICollection`1" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
    void ICollection.CopyTo(Array array, int index)
    {
      if (array == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
      if (array.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
      if (array.GetLowerBound(0) != 0)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
      if ((uint) index > (uint) array.Length)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (array.Length - index < this.Count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
      switch (array)
      {
        case KeyValuePair<TKey, TValue>[] array1:
          this.CopyTo(array1, index);
          break;
        case DictionaryEntry[] dictionaryEntryArray:
          Dictionary<TKey, TValue>.Entry[] entries1 = this._entries;
          for (int index1 = 0; index1 < this._count; ++index1)
          {
            if (entries1[index1].next >= -1)
            {
              int index2 = index++;
              DictionaryEntry dictionaryEntry = new DictionaryEntry((object) entries1[index1].key, (object) entries1[index1].value);
              dictionaryEntryArray[index2] = dictionaryEntry;
            }
          }
          break;
        case object[] objArray:
label_18:
          try
          {
            int count = this._count;
            Dictionary<TKey, TValue>.Entry[] entries2 = this._entries;
            for (int index3 = 0; index3 < count; ++index3)
            {
              if (entries2[index3].next >= -1)
              {
                int index4 = index++;
                // ISSUE: variable of a boxed type
                __Boxed<KeyValuePair<TKey, TValue>> local = (ValueType) new KeyValuePair<TKey, TValue>(entries2[index3].key, entries2[index3].value);
                objArray[index4] = (object) local;
              }
            }
            break;
          }
          catch (ArrayTypeMismatchException ex)
          {
            ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
            break;
          }
        default:
          ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
          goto label_18;
      }
    }

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Dictionary<TKey, TValue>.Enumerator(this, 2);

    /// <summary>Ensures that the dictionary can hold up to a specified number of entries without any further expansion of its backing storage.</summary>
    /// <param name="capacity">The number of entries.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than 0.</exception>
    /// <returns>The current capacity of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
    public int EnsureCapacity(int capacity)
    {
      if (capacity < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
      int length = this._entries == null ? 0 : this._entries.Length;
      if (length >= capacity)
        return length;
      ++this._version;
      if (this._buckets == null)
        return this.Initialize(capacity);
      int prime = HashHelpers.GetPrime(capacity);
      this.Resize(prime, false);
      return prime;
    }

    /// <summary>Sets the capacity of this dictionary to what it would be if it had been originally initialized with all its entries.</summary>
    public void TrimExcess() => this.TrimExcess(this.Count);

    /// <summary>Sets the capacity of this dictionary to hold up a specified number of entries without any further expansion of its backing storage.</summary>
    /// <param name="capacity">The new capacity.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
    public void TrimExcess(int capacity)
    {
      if (capacity < this.Count)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
      int prime = HashHelpers.GetPrime(capacity);
      Dictionary<TKey, TValue>.Entry[] entries = this._entries;
      int length = entries == null ? 0 : entries.Length;
      if (prime >= length)
        return;
      int count = this._count;
      ++this._version;
      this.Initialize(prime);
      this.CopyEntries(entries, count);
    }

    private void CopyEntries(Dictionary<TKey, TValue>.Entry[] entries, int count)
    {
      Dictionary<TKey, TValue>.Entry[] entries1 = this._entries;
      int index1 = 0;
      for (int index2 = 0; index2 < count; ++index2)
      {
        uint hashCode = entries[index2].hashCode;
        if (entries[index2].next >= -1)
        {
          ref Dictionary<TKey, TValue>.Entry local1 = ref entries1[index1];
          local1 = entries[index2];
          ref int local2 = ref this.GetBucket(hashCode);
          local1.next = local2 - 1;
          local2 = index1 + 1;
          ++index1;
        }
      }
      this._count = index1;
      this._freeCount = 0;
    }

    /// <summary>Gets a value that indicates whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
    /// <returns>
    /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns <see langword="false" />.</returns>
    bool ICollection.IsSynchronized => false;


    #nullable enable
    /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
    /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
    object ICollection.SyncRoot => (object) this;

    /// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.IDictionary" /> has a fixed size.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IDictionary" /> has a fixed size; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns <see langword="false" />.</returns>
    bool IDictionary.IsFixedSize => false;

    /// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.IDictionary" /> is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IDictionary" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2" />, this property always returns <see langword="false" />.</returns>
    bool IDictionary.IsReadOnly => false;

    /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</summary>
    /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the keys of the <see cref="T:System.Collections.IDictionary" />.</returns>
    ICollection IDictionary.Keys => (ICollection) this.Keys;

    /// <summary>Gets an <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</summary>
    /// <returns>An <see cref="T:System.Collections.ICollection" /> containing the values in the <see cref="T:System.Collections.IDictionary" />.</returns>
    ICollection IDictionary.Values => (ICollection) this.Values;

    /// <summary>Gets or sets the value with the specified key.</summary>
    /// <param name="key">The key of the value to get.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">A value is being assigned, and <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.
    /// 
    /// -or-
    /// 
    /// A value is being assigned, and <paramref name="value" /> is of a type that is not assignable to the value type <paramref name="TValue" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
    /// <returns>The value associated with the specified key, or <see langword="null" /> if <paramref name="key" /> is not in the dictionary or <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</returns>
    object? IDictionary.this[
    #nullable disable
    object key]
    {
      get
      {
        if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
        {
          ref TValue local = ref this.FindValue((TKey) key);
          if (!Unsafe.IsNullRef<TValue>(ref local))
            return (object) local;
        }
        return (object) null;
      }
      set
      {
        if (key == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
        ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
        try
        {
          TKey key1 = (TKey) key;
          try
          {
            this[key1] = (TValue) value;
          }
          catch (InvalidCastException ex)
          {
            ThrowHelper.ThrowWrongValueTypeArgumentException<object>(value, typeof (TValue));
          }
        }
        catch (InvalidCastException ex)
        {
          ThrowHelper.ThrowWrongKeyTypeArgumentException<object>(key, typeof (TKey));
        }
      }
    }

    private static bool IsCompatibleKey(object key)
    {
      if (key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      return key is TKey;
    }

    /// <summary>Adds the specified key and value to the dictionary.</summary>
    /// <param name="key">The object to use as the key.</param>
    /// <param name="value">The object to use as the value.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="key" /> is of a type that is not assignable to the key type <paramref name="TKey" /> of the <see cref="T:System.Collections.Generic.Dictionary`2" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is of a type that is not assignable to <paramref name="TValue" />, the type of values in the <see cref="T:System.Collections.Generic.Dictionary`2" />.
    /// 
    /// -or-
    /// 
    /// A value with the same key already exists in the <see cref="T:System.Collections.Generic.Dictionary`2" />.</exception>
    void IDictionary.Add(object key, object value)
    {
      if (key == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
      try
      {
        TKey key1 = (TKey) key;
        try
        {
          this.Add(key1, (TValue) value);
        }
        catch (InvalidCastException ex)
        {
          ThrowHelper.ThrowWrongValueTypeArgumentException<object>(value, typeof (TValue));
        }
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongKeyTypeArgumentException<object>(key, typeof (TKey));
      }
    }

    /// <summary>Determines whether the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key.</summary>
    /// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IDictionary" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
    bool IDictionary.Contains(object key) => Dictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey) key);

    /// <summary>Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</summary>
    /// <returns>An <see cref="T:System.Collections.IDictionaryEnumerator" /> for the <see cref="T:System.Collections.IDictionary" />.</returns>
    IDictionaryEnumerator IDictionary.GetEnumerator() => (IDictionaryEnumerator) new Dictionary<TKey, TValue>.Enumerator(this, 1);

    /// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" />.</summary>
    /// <param name="key">The key of the element to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="key" /> is <see langword="null" />.</exception>
    void IDictionary.Remove(object key)
    {
      if (!Dictionary<TKey, TValue>.IsCompatibleKey(key))
        return;
      this.Remove((TKey) key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ref int GetBucket(uint hashCode)
    {
      int[] buckets = this._buckets;
      return ref buckets[(int) HashHelpers.FastMod(hashCode, (uint) buckets.Length, this._fastModMultiplier)];
    }

    internal static class CollectionsMarshalHelper
    {
      public static ref TValue GetValueRefOrAddDefault(
        Dictionary<TKey, TValue> dictionary,
        TKey key,
        out bool exists)
      {
        if ((object) key == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
        if (dictionary._buckets == null)
          dictionary.Initialize(0);
        Dictionary<TKey, TValue>.Entry[] entries = dictionary._entries;
        IEqualityComparer<TKey> comparer = dictionary._comparer;
        uint hashCode = comparer == null ? (uint) key.GetHashCode() : (uint) comparer.GetHashCode(key);
        uint num = 0;
        ref int local1 = ref dictionary.GetBucket(hashCode);
        int index1 = local1 - 1;
        if (comparer == null)
        {
          if (typeof (TKey).IsValueType)
          {
            while ((uint) index1 < (uint) entries.Length)
            {
              if ((int) entries[index1].hashCode == (int) hashCode && EqualityComparer<TKey>.Default.Equals(entries[index1].key, key))
              {
                exists = true;
                return ref entries[index1].value;
              }
              index1 = entries[index1].next;
              ++num;
              if (num > (uint) entries.Length)
                ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
            }
          }
          else
          {
            EqualityComparer<TKey> equalityComparer = EqualityComparer<TKey>.Default;
            while ((uint) index1 < (uint) entries.Length)
            {
              if ((int) entries[index1].hashCode == (int) hashCode && equalityComparer.Equals(entries[index1].key, key))
              {
                exists = true;
                return ref entries[index1].value;
              }
              index1 = entries[index1].next;
              ++num;
              if (num > (uint) entries.Length)
                ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
            }
          }
        }
        else
        {
          while ((uint) index1 < (uint) entries.Length)
          {
            if ((int) entries[index1].hashCode == (int) hashCode && comparer.Equals(entries[index1].key, key))
            {
              exists = true;
              return ref entries[index1].value;
            }
            index1 = entries[index1].next;
            ++num;
            if (num > (uint) entries.Length)
              ThrowHelper.ThrowInvalidOperationException_ConcurrentOperationsNotSupported();
          }
        }
        int index2;
        if (dictionary._freeCount > 0)
        {
          index2 = dictionary._freeList;
          dictionary._freeList = -3 - entries[dictionary._freeList].next;
          --dictionary._freeCount;
        }
        else
        {
          int count = dictionary._count;
          if (count == entries.Length)
          {
            dictionary.Resize();
            local1 = ref dictionary.GetBucket(hashCode);
          }
          index2 = count;
          dictionary._count = count + 1;
          entries = dictionary._entries;
        }
        ref Dictionary<TKey, TValue>.Entry local2 = ref entries[index2];
        local2.hashCode = hashCode;
        local2.next = local1 - 1;
        local2.key = key;
        local2.value = default (TValue);
        local1 = index2 + 1;
        ++dictionary._version;
        if (!typeof (TKey).IsValueType && num > 100U && comparer is NonRandomizedStringEqualityComparer)
        {
          dictionary.Resize(entries.Length, true);
          exists = false;
          return ref dictionary.FindValue(key);
        }
        exists = false;
        return ref local2.value;
      }
    }

    private struct Entry
    {
      public uint hashCode;
      public int next;
      public TKey key;
      public TValue value;
    }


    #nullable enable
    /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
    /// <typeparam name="TKey" />
    /// <typeparam name="TValue" />
    public struct Enumerator : 
      IEnumerator<KeyValuePair<TKey, TValue>>,
      IDisposable,
      IEnumerator,
      IDictionaryEnumerator
    {

      #nullable disable
      private readonly Dictionary<TKey, TValue> _dictionary;
      private readonly int _version;
      private int _index;
      private KeyValuePair<TKey, TValue> _current;
      private readonly int _getEnumeratorRetType;

      internal Enumerator(Dictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
      {
        this._dictionary = dictionary;
        this._version = dictionary._version;
        this._index = 0;
        this._getEnumeratorRetType = getEnumeratorRetType;
        this._current = new KeyValuePair<TKey, TValue>();
      }

      /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
      /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
      /// <returns>
      /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
      public bool MoveNext()
      {
        if (this._version != this._dictionary._version)
          ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
        while ((uint) this._index < (uint) this._dictionary._count)
        {
          ref Dictionary<TKey, TValue>.Entry local = ref this._dictionary._entries[this._index++];
          if (local.next >= -1)
          {
            this._current = new KeyValuePair<TKey, TValue>(local.key, local.value);
            return true;
          }
        }
        this._index = this._dictionary._count + 1;
        this._current = new KeyValuePair<TKey, TValue>();
        return false;
      }


      #nullable enable
      /// <summary>Gets the element at the current position of the enumerator.</summary>
      /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2" /> at the current position of the enumerator.</returns>
      public KeyValuePair<TKey, TValue> Current => this._current;

      /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.Enumerator" />.</summary>
      public void Dispose()
      {
      }

      /// <summary>Gets the element at the current position of the enumerator.</summary>
      /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
      /// <returns>The element in the collection at the current position of the enumerator, as an <see cref="T:System.Object" />.</returns>
      object? IEnumerator.Current
      {
        get
        {
          if (this._index == 0 || this._index == this._dictionary._count + 1)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
          return this._getEnumeratorRetType == 1 ? (object) new DictionaryEntry((object) this._current.Key, (object) this._current.Value) : (object) new KeyValuePair<TKey, TValue>(this._current.Key, this._current.Value);
        }
      }

      /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
      /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
      void IEnumerator.Reset()
      {
        if (this._version != this._dictionary._version)
          ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
        this._index = 0;
        this._current = new KeyValuePair<TKey, TValue>();
      }

      /// <summary>Gets the element at the current position of the enumerator.</summary>
      /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
      /// <returns>The element in the dictionary at the current position of the enumerator, as a <see cref="T:System.Collections.DictionaryEntry" />.</returns>
      DictionaryEntry IDictionaryEnumerator.Entry
      {
        get
        {
          if (this._index == 0 || this._index == this._dictionary._count + 1)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
          return new DictionaryEntry((object) this._current.Key, (object) this._current.Value);
        }
      }

      /// <summary>Gets the key of the element at the current position of the enumerator.</summary>
      /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
      /// <returns>The key of the element in the dictionary at the current position of the enumerator.</returns>
      object IDictionaryEnumerator.Key
      {
        get
        {
          if (this._index == 0 || this._index == this._dictionary._count + 1)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
          return (object) this._current.Key;
        }
      }

      /// <summary>Gets the value of the element at the current position of the enumerator.</summary>
      /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
      /// <returns>The value of the element in the dictionary at the current position of the enumerator.</returns>
      object? IDictionaryEnumerator.Value
      {
        get
        {
          if (this._index == 0 || this._index == this._dictionary._count + 1)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
          return (object) this._current.Value;
        }
      }
    }

    /// <summary>Represents the collection of keys in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited.</summary>
    /// <typeparam name="TKey" />
    /// <typeparam name="TValue" />
    [DebuggerTypeProxy(typeof (DictionaryKeyCollectionDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class KeyCollection : 
      ICollection<TKey>,
      IEnumerable<TKey>,
      IEnumerable,
      ICollection,
      IReadOnlyCollection<TKey>
    {

      #nullable disable
      private readonly Dictionary<TKey, TValue> _dictionary;


      #nullable enable
      /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> class that reflects the keys in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
      /// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose keys are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// <paramref name="dictionary" /> is <see langword="null" />.</exception>
      public KeyCollection(Dictionary<TKey, TValue> dictionary)
      {
        if (dictionary == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
        this._dictionary = dictionary;
      }

      /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
      /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</returns>
      public Dictionary<
      #nullable disable
      TKey, TValue>.KeyCollection.Enumerator GetEnumerator() => new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);


      #nullable enable
      /// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
      /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// <paramref name="array" /> is <see langword="null" />.</exception>
      /// <exception cref="T:System.ArgumentOutOfRangeException">
      /// <paramref name="index" /> is less than zero.</exception>
      /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
      public void CopyTo(TKey[] array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if (index < 0 || index > array.Length)
          ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
        if (array.Length - index < this._dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        int count = this._dictionary._count;
        Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
        for (int index1 = 0; index1 < count; ++index1)
        {
          if (entries[index1].next >= -1)
            array[index++] = entries[index1].key;
        }
      }

      /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
      /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.
      /// 
      /// Retrieving the value of this property is an O(1) operation.</returns>
      public int Count => this._dictionary.Count;

      /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
      /// <returns>
      /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns <see langword="true" />.</returns>
      bool ICollection<
      #nullable disable
      TKey>.IsReadOnly => true;

      /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
      /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
      /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
      void ICollection<TKey>.Add(TKey item) => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);

      /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
      /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
      void ICollection<TKey>.Clear() => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);

      /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
      /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
      /// <returns>
      /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
      bool ICollection<TKey>.Contains(TKey item) => this._dictionary.ContainsKey(item);

      /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
      /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
      /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
      /// <returns>
      /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if item was not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
      bool ICollection<TKey>.Remove(TKey item)
      {
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
        return false;
      }

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
      IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => (IEnumerator<TKey>) new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this._dictionary);

      /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
      /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// <paramref name="array" /> is <see langword="null" />.</exception>
      /// <exception cref="T:System.ArgumentOutOfRangeException">
      /// <paramref name="index" /> is less than zero.</exception>
      /// <exception cref="T:System.ArgumentException">
      ///        <paramref name="array" /> is multidimensional.
      /// 
      /// -or-
      /// 
      /// <paramref name="array" /> does not have zero-based indexing.
      /// 
      /// -or-
      /// 
      /// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.
      /// 
      /// -or-
      /// 
      /// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
      void ICollection.CopyTo(Array array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if (array.Rank != 1)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
        if (array.GetLowerBound(0) != 0)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
        if ((uint) index > (uint) array.Length)
          ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
        if (array.Length - index < this._dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        switch (array)
        {
          case TKey[] array1:
            this.CopyTo(array1, index);
            break;
          case object[] objArray:
label_13:
            int count = this._dictionary._count;
            Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
            try
            {
              for (int index1 = 0; index1 < count; ++index1)
              {
                if (entries[index1].next >= -1)
                {
                  int index2 = index++;
                  // ISSUE: variable of a boxed type
                  __Boxed<TKey> key = (object) entries[index1].key;
                  objArray[index2] = (object) key;
                }
              }
              break;
            }
            catch (ArrayTypeMismatchException ex)
            {
              ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
              break;
            }
          default:
            ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
            goto label_13;
        }
      }

      /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
      /// <returns>
      /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns <see langword="false" />.</returns>
      bool ICollection.IsSynchronized => false;


      #nullable enable
      /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
      /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />, this property always returns the current instance.</returns>
      object ICollection.SyncRoot => ((ICollection) this._dictionary).SyncRoot;

      /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
      /// <typeparam name="TKey" />
      /// <typeparam name="TValue" />
      public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
      {

        #nullable disable
        private readonly Dictionary<TKey, TValue> _dictionary;
        private int _index;
        private readonly int _version;
        private TKey _currentKey;

        internal Enumerator(Dictionary<TKey, TValue> dictionary)
        {
          this._dictionary = dictionary;
          this._version = dictionary._version;
          this._index = 0;
          this._currentKey = default (TKey);
        }

        /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection.Enumerator" />.</summary>
        public void Dispose()
        {
        }

        /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" />.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        /// <returns>
        /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
          if (this._version != this._dictionary._version)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
          while ((uint) this._index < (uint) this._dictionary._count)
          {
            ref Dictionary<TKey, TValue>.Entry local = ref this._dictionary._entries[this._index++];
            if (local.next >= -1)
            {
              this._currentKey = local.key;
              return true;
            }
          }
          this._index = this._dictionary._count + 1;
          this._currentKey = default (TKey);
          return false;
        }


        #nullable enable
        /// <summary>Gets the element at the current position of the enumerator.</summary>
        /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.KeyCollection" /> at the current position of the enumerator.</returns>
        public TKey Current => this._currentKey;

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        object? IEnumerator.Current
        {
          get
          {
            if (this._index == 0 || this._index == this._dictionary._count + 1)
              ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
            return (object) this._currentKey;
          }
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        void IEnumerator.Reset()
        {
          if (this._version != this._dictionary._version)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
          this._index = 0;
          this._currentKey = default (TKey);
        }
      }
    }

    /// <summary>Represents the collection of values in a <see cref="T:System.Collections.Generic.Dictionary`2" />. This class cannot be inherited.</summary>
    /// <typeparam name="TKey" />
    /// <typeparam name="TValue" />
    [DebuggerTypeProxy(typeof (DictionaryValueCollectionDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ValueCollection : 
      ICollection<TValue>,
      IEnumerable<TValue>,
      IEnumerable,
      ICollection,
      IReadOnlyCollection<TValue>
    {

      #nullable disable
      private readonly Dictionary<TKey, TValue> _dictionary;


      #nullable enable
      /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> class that reflects the values in the specified <see cref="T:System.Collections.Generic.Dictionary`2" />.</summary>
      /// <param name="dictionary">The <see cref="T:System.Collections.Generic.Dictionary`2" /> whose values are reflected in the new <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// <paramref name="dictionary" /> is <see langword="null" />.</exception>
      public ValueCollection(Dictionary<TKey, TValue> dictionary)
      {
        if (dictionary == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
        this._dictionary = dictionary;
      }

      /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
      /// <returns>A <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" /> for the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
      public Dictionary<
      #nullable disable
      TKey, TValue>.ValueCollection.Enumerator GetEnumerator() => new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this._dictionary);


      #nullable enable
      /// <summary>Copies the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
      /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// <paramref name="array" /> is <see langword="null" />.</exception>
      /// <exception cref="T:System.ArgumentOutOfRangeException">
      /// <paramref name="index" /> is less than zero.</exception>
      /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
      public void CopyTo(TValue[] array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if ((long) (uint) index > (long) array.Length)
          ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
        if (array.Length - index < this._dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        int count = this._dictionary._count;
        Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
        for (int index1 = 0; index1 < count; ++index1)
        {
          if (entries[index1].next >= -1)
            array[index++] = entries[index1].value;
        }
      }

      /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
      /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</returns>
      public int Count => this._dictionary.Count;

      /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
      /// <returns>
      /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns <see langword="true" />.</returns>
      bool ICollection<
      #nullable disable
      TValue>.IsReadOnly => true;

      /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
      /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
      /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
      void ICollection<TValue>.Add(TValue item) => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);

      /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />. This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
      /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
      /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
      /// <returns>
      /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> was not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
      bool ICollection<TValue>.Remove(TValue item)
      {
        ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
        return false;
      }

      /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
      /// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
      void ICollection<TValue>.Clear() => ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);

      /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
      /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
      /// <returns>
      /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
      bool ICollection<TValue>.Contains(TValue item) => this._dictionary.ContainsValue(item);

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
      IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => (IEnumerator<TValue>) new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this._dictionary);

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this._dictionary);

      /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
      /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
      /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// <paramref name="array" /> is <see langword="null" />.</exception>
      /// <exception cref="T:System.ArgumentOutOfRangeException">
      /// <paramref name="index" /> is less than zero.</exception>
      /// <exception cref="T:System.ArgumentException">
      ///        <paramref name="array" /> is multidimensional.
      /// 
      /// -or-
      /// 
      /// <paramref name="array" /> does not have zero-based indexing.
      /// 
      /// -or-
      /// 
      /// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.
      /// 
      /// -or-
      /// 
      /// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
      void ICollection.CopyTo(Array array, int index)
      {
        if (array == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
        if (array.Rank != 1)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
        if (array.GetLowerBound(0) != 0)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
        if ((uint) index > (uint) array.Length)
          ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
        if (array.Length - index < this._dictionary.Count)
          ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
        switch (array)
        {
          case TValue[] array1:
            this.CopyTo(array1, index);
            break;
          case object[] objArray:
label_13:
            int count = this._dictionary._count;
            Dictionary<TKey, TValue>.Entry[] entries = this._dictionary._entries;
            try
            {
              for (int index1 = 0; index1 < count; ++index1)
              {
                if (entries[index1].next >= -1)
                {
                  int index2 = index++;
                  // ISSUE: variable of a boxed type
                  __Boxed<TValue> local = (object) entries[index1].value;
                  objArray[index2] = (object) local;
                }
              }
              break;
            }
            catch (ArrayTypeMismatchException ex)
            {
              ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
              break;
            }
          default:
            ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
            goto label_13;
        }
      }

      /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
      /// <returns>
      /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns <see langword="false" />.</returns>
      bool ICollection.IsSynchronized => false;


      #nullable enable
      /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
      /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />, this property always returns the current instance.</returns>
      object ICollection.SyncRoot => ((ICollection) this._dictionary).SyncRoot;

      /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
      /// <typeparam name="TKey" />
      /// <typeparam name="TValue" />
      public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
      {

        #nullable disable
        private readonly Dictionary<TKey, TValue> _dictionary;
        private int _index;
        private readonly int _version;
        private TValue _currentValue;

        internal Enumerator(Dictionary<TKey, TValue> dictionary)
        {
          this._dictionary = dictionary;
          this._version = dictionary._version;
          this._index = 0;
          this._currentValue = default (TValue);
        }

        /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection.Enumerator" />.</summary>
        public void Dispose()
        {
        }

        /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" />.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        /// <returns>
        /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
          if (this._version != this._dictionary._version)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
          while ((uint) this._index < (uint) this._dictionary._count)
          {
            ref Dictionary<TKey, TValue>.Entry local = ref this._dictionary._entries[this._index++];
            if (local.next >= -1)
            {
              this._currentValue = local.value;
              return true;
            }
          }
          this._index = this._dictionary._count + 1;
          this._currentValue = default (TValue);
          return false;
        }


        #nullable enable
        /// <summary>Gets the element at the current position of the enumerator.</summary>
        /// <returns>The element in the <see cref="T:System.Collections.Generic.Dictionary`2.ValueCollection" /> at the current position of the enumerator.</returns>
        public TValue Current => this._currentValue;

        /// <summary>Gets the element at the current position of the enumerator.</summary>
        /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
        /// <returns>The element in the collection at the current position of the enumerator.</returns>
        object? IEnumerator.Current
        {
          get
          {
            if (this._index == 0 || this._index == this._dictionary._count + 1)
              ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
            return (object) this._currentValue;
          }
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        void IEnumerator.Reset()
        {
          if (this._version != this._dictionary._version)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
          this._index = 0;
          this._currentValue = default (TValue);
        }
      }
    }
  }
}
