// Decompiled with JetBrains decompiler
// Type: System.Guid
// Assembly: System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 1228D55B-FDDD-436D-8525-FDF4491B3FE9
// Assembly location: /usr/lib/dotnet/shared/Microsoft.NETCore.App/7.0.9/System.Private.CoreLib.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.NETCore.App.Ref/7.0.9/ref/net7.0/System.Runtime.xml

using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Represents a globally unique identifier (GUID).</summary>
  [NonVersionable]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Guid : 
    ISpanFormattable,
    IFormattable,
    IComparable,
    IComparable<Guid>,
    IEquatable<Guid>,
    ISpanParsable<Guid>,
    IParsable<Guid>
  {
    /// <summary>A read-only instance of the <see cref="T:System.Guid" /> structure whose value is all zeros.</summary>
    public static readonly Guid Empty;
    private readonly int _a;
    private readonly short _b;
    private readonly short _c;
    private readonly byte _d;
    private readonly byte _e;
    private readonly byte _f;
    private readonly byte _g;
    private readonly byte _h;
    private readonly byte _i;
    private readonly byte _j;
    private readonly byte _k;

    /// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> structure by using the specified array of bytes.</summary>
    /// <param name="b">A 16-element byte array containing values with which to initialize the GUID.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="b" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="b" /> is not 16 bytes long.</exception>
    public Guid(byte[] b)
      : this(new ReadOnlySpan<byte>(b ?? throw new ArgumentNullException(nameof (b))))
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> structure by using the value represented by the specified read-only span of bytes.</summary>
    /// <param name="b">A read-only span containing the bytes representing the GUID. The span must be exactly 16 bytes long.</param>
    /// <exception cref="T:System.ArgumentException">The span must be exactly 16 bytes long.</exception>
    public Guid(ReadOnlySpan<byte> b)
    {
      if (b.Length != 16)
        throw new ArgumentException(SR.Format(SR.Arg_GuidArrayCtor, (object) "16"), nameof (b));
      int num = BitConverter.IsLittleEndian ? 1 : 0;
      this = MemoryMarshal.Read<Guid>(b);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> structure by using the specified unsigned integers and bytes.</summary>
    /// <param name="a">The first 4 bytes of the GUID.</param>
    /// <param name="b">The next 2 bytes of the GUID.</param>
    /// <param name="c">The next 2 bytes of the GUID.</param>
    /// <param name="d">The next byte of the GUID.</param>
    /// <param name="e">The next byte of the GUID.</param>
    /// <param name="f">The next byte of the GUID.</param>
    /// <param name="g">The next byte of the GUID.</param>
    /// <param name="h">The next byte of the GUID.</param>
    /// <param name="i">The next byte of the GUID.</param>
    /// <param name="j">The next byte of the GUID.</param>
    /// <param name="k">The next byte of the GUID.</param>
    [CLSCompliant(false)]
    public Guid(
      uint a,
      ushort b,
      ushort c,
      byte d,
      byte e,
      byte f,
      byte g,
      byte h,
      byte i,
      byte j,
      byte k)
    {
      this._a = (int) a;
      this._b = (short) b;
      this._c = (short) c;
      this._d = d;
      this._e = e;
      this._f = f;
      this._g = g;
      this._h = h;
      this._i = i;
      this._j = j;
      this._k = k;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> structure by using the specified integers and byte array.</summary>
    /// <param name="a">The first 4 bytes of the GUID.</param>
    /// <param name="b">The next 2 bytes of the GUID.</param>
    /// <param name="c">The next 2 bytes of the GUID.</param>
    /// <param name="d">The remaining 8 bytes of the GUID.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="d" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="d" /> is not 8 bytes long.</exception>
    public Guid(int a, short b, short c, byte[] d)
    {
      ArgumentNullException.ThrowIfNull((object) d, nameof (d));
      if (d.Length != 8)
        throw new ArgumentException(SR.Format(SR.Arg_GuidArrayCtor, (object) "8"), nameof (d));
      this._a = a;
      this._b = b;
      this._c = c;
      this._k = d[7];
      this._d = d[0];
      this._e = d[1];
      this._f = d[2];
      this._g = d[3];
      this._h = d[4];
      this._i = d[5];
      this._j = d[6];
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> structure by using the specified integers and bytes.</summary>
    /// <param name="a">The first 4 bytes of the GUID.</param>
    /// <param name="b">The next 2 bytes of the GUID.</param>
    /// <param name="c">The next 2 bytes of the GUID.</param>
    /// <param name="d">The next byte of the GUID.</param>
    /// <param name="e">The next byte of the GUID.</param>
    /// <param name="f">The next byte of the GUID.</param>
    /// <param name="g">The next byte of the GUID.</param>
    /// <param name="h">The next byte of the GUID.</param>
    /// <param name="i">The next byte of the GUID.</param>
    /// <param name="j">The next byte of the GUID.</param>
    /// <param name="k">The next byte of the GUID.</param>
    public Guid(
      int a,
      short b,
      short c,
      byte d,
      byte e,
      byte f,
      byte g,
      byte h,
      byte i,
      byte j,
      byte k)
    {
      this._a = a;
      this._b = b;
      this._c = c;
      this._d = d;
      this._e = e;
      this._f = f;
      this._g = g;
      this._h = h;
      this._i = i;
      this._j = j;
      this._k = k;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> structure by using the value represented by the specified string.</summary>
    /// <param name="g">A string that contains a GUID in one of the following formats ("d" represents a hexadecimal digit whose case is ignored):
    /// 
    /// 32 contiguous hexadecimal digits:
    /// 
    /// dddddddddddddddddddddddddddddddd
    /// 
    /// -or-
    /// 
    /// Groups of 8, 4, 4, 4, and 12 hexadecimal digits with hyphens between the groups. The entire GUID can optionally be enclosed in matching braces or parentheses:
    /// 
    /// dddddddd-dddd-dddd-dddd-dddddddddddd
    /// 
    /// -or-
    /// 
    /// {dddddddd-dddd-dddd-dddd-dddddddddddd}
    /// 
    /// -or-
    /// 
    /// (dddddddd-dddd-dddd-dddd-dddddddddddd)
    /// 
    /// -or-
    /// 
    /// Groups of 8, 4, and 4 hexadecimal digits, and a subset of eight groups of 2 hexadecimal digits, with each group prefixed by "0x" or "0X", and separated by commas. The entire GUID, as well as the subset, is enclosed in matching braces:
    /// 
    /// {0xdddddddd, 0xdddd, 0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}
    /// 
    /// All braces, commas, and "0x" prefixes are required. All embedded spaces are ignored. All leading zeros in a group are ignored.
    /// 
    /// The hexadecimal digits shown in a group are the maximum number of meaningful hexadecimal digits that can appear in that group. You can specify from 1 to the number of hexadecimal digits shown for a group. The specified digits are assumed to be the low-order digits of the group.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="g" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The format of <paramref name="g" /> is invalid.</exception>
    /// <exception cref="T:System.OverflowException">The format of <paramref name="g" /> is invalid.</exception>
    public Guid(string g)
    {
      ArgumentNullException.ThrowIfNull((object) g, nameof (g));
      Guid.GuidResult result = new Guid.GuidResult(Guid.GuidParseThrowStyle.All);
      Guid.TryParseGuid((ReadOnlySpan<char>) g, ref result);
      this = result.ToGuid();
    }

    /// <summary>Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid" /> structure.</summary>
    /// <param name="input">The string to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input" /> is not in a recognized format.</exception>
    /// <returns>A structure that contains the value that was parsed.</returns>
    public static Guid Parse(string input)
    {
      ArgumentNullException.ThrowIfNull((object) input, nameof (input));
      return Guid.Parse((ReadOnlySpan<char>) input);
    }

    /// <summary>Converts a read-only character span that represents a GUID to the equivalent <see cref="T:System.Guid" /> structure.</summary>
    /// <param name="input">A read-only span containing the bytes representing a GUID.</param>
    /// <exception cref="T:System.FormatException">
    ///         <paramref name="input" /> is not in a recognized format.
    /// 
    /// -or-
    /// 
    /// After trimming, the length of the read-only character span is 0.</exception>
    /// <returns>A structure that contains the value that was parsed.</returns>
    public static Guid Parse(ReadOnlySpan<char> input)
    {
      Guid.GuidResult result = new Guid.GuidResult(Guid.GuidParseThrowStyle.AllButOverflow);
      Guid.TryParseGuid(input, ref result);
      return result.ToGuid();
    }

    /// <summary>Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid" /> structure.</summary>
    /// <param name="input">A string containing the GUID to convert.</param>
    /// <param name="result">When this method returns, contains the parsed value. If the method returns <see langword="true" />, <paramref name="result" /> contains a valid <see cref="T:System.Guid" />. If the method returns <see langword="false" />, <paramref name="result" /> equals <see cref="F:System.Guid.Empty" />.</param>
    /// <returns>
    /// <see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? input, out Guid result)
    {
      if (input != null)
        return Guid.TryParse((ReadOnlySpan<char>) input, out result);
      result = new Guid();
      return false;
    }

    /// <summary>Converts the specified read-only span of characters containing the representation of a GUID to the equivalent <see cref="T:System.Guid" /> structure.</summary>
    /// <param name="input">A span containing the characters representing the GUID to convert.</param>
    /// <param name="result">When this method returns, contains the parsed value. If the method returns <see langword="true" />, <paramref name="result" /> contains a valid <see cref="T:System.Guid" />. If the method returns <see langword="false" />, <paramref name="result" /> equals <see cref="F:System.Guid.Empty" />.</param>
    /// <returns>
    /// <see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> input, out Guid result)
    {
      Guid.GuidResult result1 = new Guid.GuidResult(Guid.GuidParseThrowStyle.None);
      if (Guid.TryParseGuid(input, ref result1))
      {
        result = result1.ToGuid();
        return true;
      }
      result = new Guid();
      return false;
    }

    /// <summary>Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid" /> structure, provided that the string is in the specified format.</summary>
    /// <param name="input">The GUID to convert.</param>
    /// <param name="format">One of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> or <paramref name="format" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="input" /> is not in the format specified by <paramref name="format" />.</exception>
    /// <returns>A structure that contains the value that was parsed.</returns>
    public static Guid ParseExact(string input, [StringSyntax("GuidFormat")] string format)
    {
      ArgumentNullException.ThrowIfNull((object) input, nameof (input));
      ArgumentNullException.ThrowIfNull((object) format, nameof (format));
      return Guid.ParseExact((ReadOnlySpan<char>) input, (ReadOnlySpan<char>) format);
    }

    /// <summary>Converts the character span representation of a GUID to the equivalent <see cref="T:System.Guid" /> structure, provided that the string is in the specified format.</summary>
    /// <param name="input">A read-only span containing the characters representing the GUID to convert.</param>
    /// <param name="format">A read-only span of characters representing one of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <returns>A structure that contains the value that was parsed.</returns>
    public static Guid ParseExact(ReadOnlySpan<char> input, [StringSyntax("GuidFormat")] ReadOnlySpan<char> format)
    {
      if (format.Length != 1)
        throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
      input = input.Trim();
      Guid.GuidResult result = new Guid.GuidResult(Guid.GuidParseThrowStyle.AllButOverflow);
      bool flag;
      switch ((char) ((uint) format[0] | 32U))
      {
        case 'b':
          flag = Guid.TryParseExactB(input, ref result);
          break;
        case 'd':
          flag = Guid.TryParseExactD(input, ref result);
          break;
        case 'n':
          flag = Guid.TryParseExactN(input, ref result);
          break;
        case 'p':
          flag = Guid.TryParseExactP(input, ref result);
          break;
        case 'x':
          flag = Guid.TryParseExactX(input, ref result);
          break;
        default:
          throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
      }
      return result.ToGuid();
    }

    /// <summary>Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid" /> structure, provided that the string is in the specified format.</summary>
    /// <param name="input">The GUID to convert.</param>
    /// <param name="format">One of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <param name="result">When this method returns, contains the parsed value. If the method returns <see langword="true" />, <paramref name="result" /> contains a valid <see cref="T:System.Guid" />. If the method returns <see langword="false" />, <paramref name="result" /> equals <see cref="F:System.Guid.Empty" />.</param>
    /// <returns>
    /// <see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact([NotNullWhen(true)] string? input, [NotNullWhen(true), StringSyntax("GuidFormat")] string? format, out Guid result)
    {
      if (input != null)
        return Guid.TryParseExact((ReadOnlySpan<char>) input, (ReadOnlySpan<char>) format, out result);
      result = new Guid();
      return false;
    }

    /// <summary>Converts span of characters representing the GUID to the equivalent <see cref="T:System.Guid" /> structure, provided that the string is in the specified format.</summary>
    /// <param name="input">A read-only span containing the characters representing the GUID to convert.</param>
    /// <param name="format">A read-only span containing a character representing one of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <param name="result">When this method returns, contains the parsed value. If the method returns <see langword="true" />, <paramref name="result" /> contains a valid <see cref="T:System.Guid" />. If the method returns <see langword="false" />, <paramref name="result" /> equals <see cref="F:System.Guid.Empty" />.</param>
    /// <returns>
    /// <see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryParseExact(
      ReadOnlySpan<char> input,
      [StringSyntax("GuidFormat")] ReadOnlySpan<char> format,
      out Guid result)
    {
      if (format.Length != 1)
      {
        result = new Guid();
        return false;
      }
      input = input.Trim();
      Guid.GuidResult result1 = new Guid.GuidResult(Guid.GuidParseThrowStyle.None);
      bool flag = false;
      switch ((char) ((uint) format[0] | 32U))
      {
        case 'b':
          flag = Guid.TryParseExactB(input, ref result1);
          break;
        case 'd':
          flag = Guid.TryParseExactD(input, ref result1);
          break;
        case 'n':
          flag = Guid.TryParseExactN(input, ref result1);
          break;
        case 'p':
          flag = Guid.TryParseExactP(input, ref result1);
          break;
        case 'x':
          flag = Guid.TryParseExactX(input, ref result1);
          break;
      }
      if (flag)
      {
        result = result1.ToGuid();
        return true;
      }
      result = new Guid();
      return false;
    }


    #nullable disable
    private static bool TryParseGuid(ReadOnlySpan<char> guidString, ref Guid.GuidResult result)
    {
      guidString = guidString.Trim();
      if (guidString.Length == 0)
      {
        result.SetFailure(false, "Format_GuidUnrecognized");
        return false;
      }
      bool guid;
      switch (guidString[0])
      {
        case '(':
          guid = Guid.TryParseExactP(guidString, ref result);
          break;
        case '{':
          guid = guidString.Contains<char>('-') ? Guid.TryParseExactB(guidString, ref result) : Guid.TryParseExactX(guidString, ref result);
          break;
        default:
          guid = guidString.Contains<char>('-') ? Guid.TryParseExactD(guidString, ref result) : Guid.TryParseExactN(guidString, ref result);
          break;
      }
      return guid;
    }

    private static bool TryParseExactB(ReadOnlySpan<char> guidString, ref Guid.GuidResult result)
    {
      if (guidString.Length == 38 && guidString[0] == '{' && guidString[37] == '}')
        return Guid.TryParseExactD(guidString.Slice(1, 36), ref result);
      result.SetFailure(false, "Format_GuidInvLen");
      return false;
    }

    private static bool TryParseExactD(ReadOnlySpan<char> guidString, ref Guid.GuidResult result)
    {
      if (guidString.Length != 36 || guidString[8] != '-' || guidString[13] != '-' || guidString[18] != '-' || guidString[23] != '-')
      {
        result.SetFailure(false, guidString.Length != 36 ? "Format_GuidInvLen" : "Format_GuidDashes");
        return false;
      }
      Span<byte> span = MemoryMarshal.AsBytes<Guid.GuidResult>(new Span<Guid.GuidResult>(ref result));
      int invalidIfNegative = 0;
      span[0] = Guid.DecodeByte((UIntPtr) guidString[6], (UIntPtr) guidString[7], ref invalidIfNegative);
      span[1] = Guid.DecodeByte((UIntPtr) guidString[4], (UIntPtr) guidString[5], ref invalidIfNegative);
      span[2] = Guid.DecodeByte((UIntPtr) guidString[2], (UIntPtr) guidString[3], ref invalidIfNegative);
      span[3] = Guid.DecodeByte((UIntPtr) guidString[0], (UIntPtr) guidString[1], ref invalidIfNegative);
      span[4] = Guid.DecodeByte((UIntPtr) guidString[11], (UIntPtr) guidString[12], ref invalidIfNegative);
      span[5] = Guid.DecodeByte((UIntPtr) guidString[9], (UIntPtr) guidString[10], ref invalidIfNegative);
      span[6] = Guid.DecodeByte((UIntPtr) guidString[16], (UIntPtr) guidString[17], ref invalidIfNegative);
      span[7] = Guid.DecodeByte((UIntPtr) guidString[14], (UIntPtr) guidString[15], ref invalidIfNegative);
      span[8] = Guid.DecodeByte((UIntPtr) guidString[19], (UIntPtr) guidString[20], ref invalidIfNegative);
      span[9] = Guid.DecodeByte((UIntPtr) guidString[21], (UIntPtr) guidString[22], ref invalidIfNegative);
      span[10] = Guid.DecodeByte((UIntPtr) guidString[24], (UIntPtr) guidString[25], ref invalidIfNegative);
      span[11] = Guid.DecodeByte((UIntPtr) guidString[26], (UIntPtr) guidString[27], ref invalidIfNegative);
      span[12] = Guid.DecodeByte((UIntPtr) guidString[28], (UIntPtr) guidString[29], ref invalidIfNegative);
      span[13] = Guid.DecodeByte((UIntPtr) guidString[30], (UIntPtr) guidString[31], ref invalidIfNegative);
      span[14] = Guid.DecodeByte((UIntPtr) guidString[32], (UIntPtr) guidString[33], ref invalidIfNegative);
      span[15] = Guid.DecodeByte((UIntPtr) guidString[34], (UIntPtr) guidString[35], ref invalidIfNegative);
      if (invalidIfNegative >= 0)
      {
        if (BitConverter.IsLittleEndian)
          ;
        return true;
      }
      if (guidString.IndexOfAny<char>('X', 'x', '+') >= 0 && TryCompatParsing(guidString, ref result))
        return true;
      result.SetFailure(false, "Format_GuidInvalidChar");
      return false;

      static bool TryCompatParsing(ReadOnlySpan<char> guidString, ref Guid.GuidResult result)
      {
        uint result1;
        if (Guid.TryParseHex(guidString.Slice(0, 8), out result._a) && Guid.TryParseHex(guidString.Slice(9, 4), out result1))
        {
          result._b = (ushort) result1;
          if (Guid.TryParseHex(guidString.Slice(14, 4), out result1))
          {
            result._c = (ushort) result1;
            if (Guid.TryParseHex(guidString.Slice(19, 4), out result1))
            {
              ref Guid.GuidResult local1 = ref result;
              if (BitConverter.IsLittleEndian)
                ;
              int num1 = (int) BinaryPrimitives.ReverseEndianness((ushort) result1);
              local1._de = (ushort) num1;
              if (Guid.TryParseHex(guidString.Slice(24, 4), out result1))
              {
                ref Guid.GuidResult local2 = ref result;
                if (BitConverter.IsLittleEndian)
                  ;
                int num2 = (int) BinaryPrimitives.ReverseEndianness((ushort) result1);
                local2._fg = (ushort) num2;
                if (Number.TryParseUInt32HexNumberStyle(guidString.Slice(28, 8), NumberStyles.AllowHexSpecifier, out result1) == Number.ParsingStatus.OK)
                {
                  ref Guid.GuidResult local3 = ref result;
                  if (BitConverter.IsLittleEndian)
                    ;
                  int num3 = (int) BinaryPrimitives.ReverseEndianness(result1);
                  local3._hijk = (uint) num3;
                  return true;
                }
              }
            }
          }
        }
        return false;
      }
    }

    private static bool TryParseExactN(ReadOnlySpan<char> guidString, ref Guid.GuidResult result)
    {
      if (guidString.Length != 32)
      {
        result.SetFailure(false, "Format_GuidInvLen");
        return false;
      }
      Span<byte> span = MemoryMarshal.AsBytes<Guid.GuidResult>(new Span<Guid.GuidResult>(ref result));
      int invalidIfNegative = 0;
      span[0] = Guid.DecodeByte((UIntPtr) guidString[6], (UIntPtr) guidString[7], ref invalidIfNegative);
      span[1] = Guid.DecodeByte((UIntPtr) guidString[4], (UIntPtr) guidString[5], ref invalidIfNegative);
      span[2] = Guid.DecodeByte((UIntPtr) guidString[2], (UIntPtr) guidString[3], ref invalidIfNegative);
      span[3] = Guid.DecodeByte((UIntPtr) guidString[0], (UIntPtr) guidString[1], ref invalidIfNegative);
      span[4] = Guid.DecodeByte((UIntPtr) guidString[10], (UIntPtr) guidString[11], ref invalidIfNegative);
      span[5] = Guid.DecodeByte((UIntPtr) guidString[8], (UIntPtr) guidString[9], ref invalidIfNegative);
      span[6] = Guid.DecodeByte((UIntPtr) guidString[14], (UIntPtr) guidString[15], ref invalidIfNegative);
      span[7] = Guid.DecodeByte((UIntPtr) guidString[12], (UIntPtr) guidString[13], ref invalidIfNegative);
      span[8] = Guid.DecodeByte((UIntPtr) guidString[16], (UIntPtr) guidString[17], ref invalidIfNegative);
      span[9] = Guid.DecodeByte((UIntPtr) guidString[18], (UIntPtr) guidString[19], ref invalidIfNegative);
      span[10] = Guid.DecodeByte((UIntPtr) guidString[20], (UIntPtr) guidString[21], ref invalidIfNegative);
      span[11] = Guid.DecodeByte((UIntPtr) guidString[22], (UIntPtr) guidString[23], ref invalidIfNegative);
      span[12] = Guid.DecodeByte((UIntPtr) guidString[24], (UIntPtr) guidString[25], ref invalidIfNegative);
      span[13] = Guid.DecodeByte((UIntPtr) guidString[26], (UIntPtr) guidString[27], ref invalidIfNegative);
      span[14] = Guid.DecodeByte((UIntPtr) guidString[28], (UIntPtr) guidString[29], ref invalidIfNegative);
      span[15] = Guid.DecodeByte((UIntPtr) guidString[30], (UIntPtr) guidString[31], ref invalidIfNegative);
      if (invalidIfNegative >= 0)
      {
        if (BitConverter.IsLittleEndian)
          ;
        return true;
      }
      result.SetFailure(false, "Format_GuidInvalidChar");
      return false;
    }

    private static bool TryParseExactP(ReadOnlySpan<char> guidString, ref Guid.GuidResult result)
    {
      if (guidString.Length == 38 && guidString[0] == '(' && guidString[37] == ')')
        return Guid.TryParseExactD(guidString.Slice(1, 36), ref result);
      result.SetFailure(false, "Format_GuidInvLen");
      return false;
    }

    private static bool TryParseExactX(ReadOnlySpan<char> guidString, ref Guid.GuidResult result)
    {
      guidString = Guid.EatAllWhitespace(guidString);
      if (guidString.Length == 0 || guidString[0] != '{')
      {
        result.SetFailure(false, "Format_GuidBrace");
        return false;
      }
      if (!Guid.IsHexPrefix(guidString, 1))
      {
        result.SetFailure(false, "Format_GuidHexPrefix");
        return false;
      }
      int start1 = 3;
      int length1 = guidString.Slice(start1).IndexOf<char>(',');
      if (length1 <= 0)
      {
        result.SetFailure(false, "Format_GuidComma");
        return false;
      }
      bool overflow = false;
      if (!Guid.TryParseHex(guidString.Slice(start1, length1), out result._a, ref overflow) | overflow)
      {
        result.SetFailure(overflow, overflow ? "Overflow_UInt32" : "Format_GuidInvalidChar");
        return false;
      }
      if (!Guid.IsHexPrefix(guidString, start1 + length1 + 1))
      {
        result.SetFailure(false, "Format_GuidHexPrefix");
        return false;
      }
      int start2 = start1 + length1 + 3;
      int length2 = guidString.Slice(start2).IndexOf<char>(',');
      if (length2 <= 0)
      {
        result.SetFailure(false, "Format_GuidComma");
        return false;
      }
      if (!Guid.TryParseHex(guidString.Slice(start2, length2), out result._b, ref overflow) | overflow)
      {
        result.SetFailure(overflow, overflow ? "Overflow_UInt32" : "Format_GuidInvalidChar");
        return false;
      }
      if (!Guid.IsHexPrefix(guidString, start2 + length2 + 1))
      {
        result.SetFailure(false, "Format_GuidHexPrefix");
        return false;
      }
      int start3 = start2 + length2 + 3;
      int length3 = guidString.Slice(start3).IndexOf<char>(',');
      if (length3 <= 0)
      {
        result.SetFailure(false, "Format_GuidComma");
        return false;
      }
      if (!Guid.TryParseHex(guidString.Slice(start3, length3), out result._c, ref overflow) | overflow)
      {
        result.SetFailure(overflow, overflow ? "Overflow_UInt32" : "Format_GuidInvalidChar");
        return false;
      }
      if ((uint) guidString.Length <= (uint) (start3 + length3 + 1) || guidString[start3 + length3 + 1] != '{')
      {
        result.SetFailure(false, "Format_GuidBrace");
        return false;
      }
      int length4 = length3 + 1;
      for (int elementOffset = 0; elementOffset < 8; ++elementOffset)
      {
        if (!Guid.IsHexPrefix(guidString, start3 + length4 + 1))
        {
          result.SetFailure(false, "Format_GuidHexPrefix");
          return false;
        }
        start3 = start3 + length4 + 3;
        if (elementOffset < 7)
        {
          length4 = guidString.Slice(start3).IndexOf<char>(',');
          if (length4 <= 0)
          {
            result.SetFailure(false, "Format_GuidComma");
            return false;
          }
        }
        else
        {
          length4 = guidString.Slice(start3).IndexOf<char>('}');
          if (length4 <= 0)
          {
            result.SetFailure(false, "Format_GuidBraceAfterLastNumber");
            return false;
          }
        }
        uint result1;
        if (!Guid.TryParseHex(guidString.Slice(start3, length4), out result1, ref overflow) | overflow || result1 > (uint) byte.MaxValue)
        {
          result.SetFailure(overflow, overflow ? "Overflow_UInt32" : (result1 > (uint) byte.MaxValue ? "Overflow_Byte" : "Format_GuidInvalidChar"));
          return false;
        }
        Unsafe.Add<byte>(ref result._d, elementOffset) = (byte) result1;
      }
      if (start3 + length4 + 1 >= guidString.Length || guidString[start3 + length4 + 1] != '}')
      {
        result.SetFailure(false, "Format_GuidEndBrace");
        return false;
      }
      if (start3 + length4 + 1 == guidString.Length - 1)
        return true;
      result.SetFailure(false, "Format_ExtraJunkAtEnd");
      return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte DecodeByte(UIntPtr ch1, UIntPtr ch2, ref int invalidIfNegative)
    {
      ReadOnlySpan<byte> charToHexLookup = HexConverter.CharToHexLookup;
      int num1 = -1;
      if (ch1 < (UIntPtr) charToHexLookup.Length)
        num1 = (int) (sbyte) Unsafe.Add<byte>(ref MemoryMarshal.GetReference<byte>(charToHexLookup), (IntPtr) ch1);
      int num2 = num1 << 4;
      int num3 = -1;
      if (ch2 < (UIntPtr) charToHexLookup.Length)
        num3 = (int) (sbyte) Unsafe.Add<byte>(ref MemoryMarshal.GetReference<byte>(charToHexLookup), (IntPtr) ch2);
      int num4 = num2 | num3;
      invalidIfNegative |= num4;
      return (byte) num4;
    }

    private static bool TryParseHex(
      ReadOnlySpan<char> guidString,
      out ushort result,
      ref bool overflow)
    {
      uint result1;
      bool hex = Guid.TryParseHex(guidString, out result1, ref overflow);
      result = (ushort) result1;
      return hex;
    }

    private static bool TryParseHex(ReadOnlySpan<char> guidString, out uint result)
    {
      bool overflow = false;
      return Guid.TryParseHex(guidString, out result, ref overflow);
    }

    private static bool TryParseHex(
      ReadOnlySpan<char> guidString,
      out uint result,
      ref bool overflow)
    {
      if (guidString.Length > 0)
      {
        if (guidString[0] == '+')
          guidString = guidString.Slice(1);
        if (guidString.Length > 1 && guidString[0] == '0' && ((int) guidString[1] | 32) == 120)
          guidString = guidString.Slice(2);
      }
      int index = 0;
      while (index < guidString.Length && guidString[index] == '0')
        ++index;
      int num1 = 0;
      uint num2 = 0;
      for (; index < guidString.Length; ++index)
      {
        int num3 = HexConverter.FromChar((int) guidString[index]);
        if (num3 == (int) byte.MaxValue)
        {
          if (num1 > 8)
            overflow = true;
          result = 0U;
          return false;
        }
        num2 = (uint) ((int) num2 * 16 + num3);
        ++num1;
      }
      if (num1 > 8)
        overflow = true;
      result = num2;
      return true;
    }

    private static ReadOnlySpan<char> EatAllWhitespace(ReadOnlySpan<char> str)
    {
      int num = 0;
      while (num < str.Length && !char.IsWhiteSpace(str[num]))
        ++num;
      if (num == str.Length)
        return str;
      char[] chArray = new char[str.Length];
      int length = 0;
      if (num > 0)
      {
        length = num;
        str.Slice(0, num).CopyTo((Span<char>) chArray);
      }
      for (; num < str.Length; ++num)
      {
        char c = str[num];
        if (!char.IsWhiteSpace(c))
          chArray[length++] = c;
      }
      return new ReadOnlySpan<char>(chArray, 0, length);
    }

    private static bool IsHexPrefix(ReadOnlySpan<char> str, int i) => i + 1 < str.Length && str[i] == '0' && ((int) str[i + 1] | 32) == 120;


    #nullable enable
    /// <summary>Returns a 16-element byte array that contains the value of this instance.</summary>
    /// <returns>A 16-element byte array.</returns>
    public byte[] ToByteArray()
    {
      byte[] destination = new byte[16];
      int num = BitConverter.IsLittleEndian ? 1 : 0;
      MemoryMarshal.TryWrite<Guid>((Span<byte>) destination, ref Unsafe.AsRef<Guid>(in this));
      return destination;
    }

    /// <summary>Tries to write the current GUID instance into a span of bytes.</summary>
    /// <param name="destination">When this method returns, the GUID as a span of bytes.</param>
    /// <returns>
    /// <see langword="true" /> if the GUID is successfully written to the specified span; <see langword="false" /> otherwise.</returns>
    public bool TryWriteBytes(Span<byte> destination)
    {
      int num = BitConverter.IsLittleEndian ? 1 : 0;
      return MemoryMarshal.TryWrite<Guid>(destination, ref Unsafe.AsRef<Guid>(in this));
    }

    /// <summary>Returns a string representation of the value of this instance in registry format.</summary>
    /// <returns>The value of this <see cref="T:System.Guid" />, formatted by using the "D" format specifier as follows:
    /// 
    /// <c>xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</c>
    /// 
    /// where the value of the GUID is represented as a series of lowercase hexadecimal digits in groups of 8, 4, 4, 4, and 12 digits and separated by hyphens. An example of a return value is "382c74c3-721d-4f34-80e5-57657b6cbc27". To convert the hexadecimal digits from a through f to uppercase, call the <see cref="M:System.String.ToUpper" /> method on the returned string.</returns>
    public override string ToString() => this.ToString("D", (IFormatProvider) null);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode()
    {
      ref int local = ref Unsafe.AsRef<int>(in this._a);
      return local ^ Unsafe.Add<int>(ref local, 1) ^ Unsafe.Add<int>(ref local, 2) ^ Unsafe.Add<int>(ref local, 3);
    }

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="o">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="o" /> is a <see cref="T:System.Guid" /> that has the same value as this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? o) => o is Guid right && Guid.EqualsCore(in this, in right);

    /// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Guid" /> object represent the same value.</summary>
    /// <param name="g">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="g" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(Guid g) => Guid.EqualsCore(in this, in g);


    #nullable disable
    private static bool EqualsCore(in Guid left, in Guid right)
    {
      if (Vector128.IsHardwareAccelerated)
        return Vector128.LoadUnsafe<byte>(ref Unsafe.As<Guid, byte>(ref Unsafe.AsRef<Guid>(in left))) == Vector128.LoadUnsafe<byte>(ref Unsafe.As<Guid, byte>(ref Unsafe.AsRef<Guid>(in right)));
      ref int local1 = ref Unsafe.AsRef<int>(in left._a);
      ref int local2 = ref Unsafe.AsRef<int>(in right._a);
      return local1 == local2 && Unsafe.Add<int>(ref local1, 1) == Unsafe.Add<int>(ref local2, 1) && Unsafe.Add<int>(ref local1, 2) == Unsafe.Add<int>(ref local2, 2) && Unsafe.Add<int>(ref local1, 3) == Unsafe.Add<int>(ref local2, 3);
    }

    private static int GetResult(uint me, uint them) => me >= them ? 1 : -1;


    #nullable enable
    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.Guid" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> A positive integer</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      if (!(value is Guid guid))
        throw new ArgumentException(SR.Arg_MustBeGuid, nameof (value));
      if (guid._a != this._a)
        return Guid.GetResult((uint) this._a, (uint) guid._a);
      if ((int) guid._b != (int) this._b)
        return Guid.GetResult((uint) this._b, (uint) guid._b);
      if ((int) guid._c != (int) this._c)
        return Guid.GetResult((uint) this._c, (uint) guid._c);
      if ((int) guid._d != (int) this._d)
        return Guid.GetResult((uint) this._d, (uint) guid._d);
      if ((int) guid._e != (int) this._e)
        return Guid.GetResult((uint) this._e, (uint) guid._e);
      if ((int) guid._f != (int) this._f)
        return Guid.GetResult((uint) this._f, (uint) guid._f);
      if ((int) guid._g != (int) this._g)
        return Guid.GetResult((uint) this._g, (uint) guid._g);
      if ((int) guid._h != (int) this._h)
        return Guid.GetResult((uint) this._h, (uint) guid._h);
      if ((int) guid._i != (int) this._i)
        return Guid.GetResult((uint) this._i, (uint) guid._i);
      if ((int) guid._j != (int) this._j)
        return Guid.GetResult((uint) this._j, (uint) guid._j);
      return (int) guid._k != (int) this._k ? Guid.GetResult((uint) this._k, (uint) guid._k) : 0;
    }

    /// <summary>Compares this instance to a specified <see cref="T:System.Guid" /> object and returns an indication of their relative values.</summary>
    /// <param name="value">An object to compare to this instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Description</description></listheader><item><term> A negative integer</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> A positive integer</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(Guid value)
    {
      if (value._a != this._a)
        return Guid.GetResult((uint) this._a, (uint) value._a);
      if ((int) value._b != (int) this._b)
        return Guid.GetResult((uint) this._b, (uint) value._b);
      if ((int) value._c != (int) this._c)
        return Guid.GetResult((uint) this._c, (uint) value._c);
      if ((int) value._d != (int) this._d)
        return Guid.GetResult((uint) this._d, (uint) value._d);
      if ((int) value._e != (int) this._e)
        return Guid.GetResult((uint) this._e, (uint) value._e);
      if ((int) value._f != (int) this._f)
        return Guid.GetResult((uint) this._f, (uint) value._f);
      if ((int) value._g != (int) this._g)
        return Guid.GetResult((uint) this._g, (uint) value._g);
      if ((int) value._h != (int) this._h)
        return Guid.GetResult((uint) this._h, (uint) value._h);
      if ((int) value._i != (int) this._i)
        return Guid.GetResult((uint) this._i, (uint) value._i);
      if ((int) value._j != (int) this._j)
        return Guid.GetResult((uint) this._j, (uint) value._j);
      return (int) value._k != (int) this._k ? Guid.GetResult((uint) this._k, (uint) value._k) : 0;
    }

    /// <summary>Indicates whether the values of two specified <see cref="T:System.Guid" /> objects are equal.</summary>
    /// <param name="a">The first object to compare.</param>
    /// <param name="b">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Guid a, Guid b) => Guid.EqualsCore(in a, in b);

    /// <summary>Indicates whether the values of two specified <see cref="T:System.Guid" /> objects are not equal.</summary>
    /// <param name="a">The first object to compare.</param>
    /// <param name="b">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Guid a, Guid b) => !Guid.EqualsCore(in a, in b);

    /// <summary>Returns a string representation of the value of this <see cref="T:System.Guid" /> instance, according to the provided format specifier.</summary>
    /// <param name="format">A single format specifier that indicates how to format the value of this <see cref="T:System.Guid" />. The <paramref name="format" /> parameter can be "N", "D", "B", "P", or "X". If <paramref name="format" /> is <see langword="null" /> or an empty string (""), "D" is used.</param>
    /// <exception cref="T:System.FormatException">The value of <paramref name="format" /> is not <see langword="null" />, an empty string (""), "N", "D", "B", "P", or "X".</exception>
    /// <returns>The value of this <see cref="T:System.Guid" />, represented as a series of lowercase hexadecimal digits in the specified format.</returns>
    public string ToString([StringSyntax("GuidFormat")] string? format) => this.ToString(format, (IFormatProvider) null);


    #nullable disable
    private static unsafe int HexsToChars(char* guidChars, int a, int b)
    {
      *guidChars = HexConverter.ToCharLower(a >> 4);
      guidChars[1] = HexConverter.ToCharLower(a);
      guidChars[2] = HexConverter.ToCharLower(b >> 4);
      guidChars[3] = HexConverter.ToCharLower(b);
      return 4;
    }

    private static unsafe int HexsToCharsHexOutput(char* guidChars, int a, int b)
    {
      *guidChars = '0';
      guidChars[1] = 'x';
      guidChars[2] = HexConverter.ToCharLower(a >> 4);
      guidChars[3] = HexConverter.ToCharLower(a);
      guidChars[4] = ',';
      guidChars[5] = '0';
      guidChars[6] = 'x';
      guidChars[7] = HexConverter.ToCharLower(b >> 4);
      guidChars[8] = HexConverter.ToCharLower(b);
      return 9;
    }


    #nullable enable
    /// <summary>Returns a string representation of the value of this instance of the <see cref="T:System.Guid" /> class, according to the provided format specifier and culture-specific format information.</summary>
    /// <param name="format">A single format specifier that indicates how to format the value of this <see cref="T:System.Guid" />. The <paramref name="format" /> parameter can be "N", "D", "B", "P", or "X". If <paramref name="format" /> is <see langword="null" /> or an empty string (""), "D" is used.</param>
    /// <param name="provider">(Reserved) An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">The value of <paramref name="format" /> is not <see langword="null" />, an empty string (""), "N", "D", "B", "P", or "X".</exception>
    /// <returns>The value of this <see cref="T:System.Guid" />, represented as a series of lowercase hexadecimal digits in the specified format.</returns>
    public string ToString([StringSyntax("GuidFormat")] string? format, IFormatProvider? provider)
    {
      if (string.IsNullOrEmpty(format))
        format = "D";
      if (format.Length != 1)
        throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
      int length;
      switch (format[0])
      {
        case 'B':
        case 'P':
        case 'b':
        case 'p':
          length = 38;
          break;
        case 'D':
        case 'd':
          length = 36;
          break;
        case 'N':
        case 'n':
          length = 32;
          break;
        case 'X':
        case 'x':
          length = 68;
          break;
        default:
          throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
      }
      string str = string.FastAllocateString(length);
      this.TryFormat(new Span<char>(ref str.GetRawStringData(), str.Length), out int _, (ReadOnlySpan<char>) format);
      return str;
    }

    /// <summary>Tries to format the current GUID instance into the provided character span.</summary>
    /// <param name="destination">The span in which to write the GUID as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, contains the number of characters written into the span.</param>
    /// <param name="format">A read-only span containing the character representing one of the following specifiers that indicates the exact format to use when interpreting <paramref name="input" />: "N", "D", "B", "P", or "X".</param>
    /// <returns>
    /// <see langword="true" /> if the formatting operation was successful; <see langword="false" /> otherwise.</returns>
    public unsafe bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      [StringSyntax("GuidFormat")] ReadOnlySpan<char> format = default (ReadOnlySpan<char>))
    {
      if (format.Length == 0)
        format = (ReadOnlySpan<char>) "D";
      if (format.Length != 1)
        throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
      bool flag1 = true;
      bool flag2 = false;
      int num1 = 0;
      int num2;
      switch (format[0])
      {
        case 'B':
        case 'b':
          num1 = 8192123;
          num2 = 38;
          break;
        case 'D':
        case 'd':
          num2 = 36;
          break;
        case 'N':
        case 'n':
          flag1 = false;
          num2 = 32;
          break;
        case 'P':
        case 'p':
          num1 = 2687016;
          num2 = 38;
          break;
        case 'X':
        case 'x':
          num1 = 8192123;
          flag1 = false;
          flag2 = true;
          num2 = 68;
          break;
        default:
          throw new FormatException(SR.Format_InvalidGuidFormatSpecification);
      }
      if (destination.Length < num2)
      {
        charsWritten = 0;
        return false;
      }
      fixed (char* chPtr1 = &MemoryMarshal.GetReference<char>(destination))
      {
        char* guidChars1 = chPtr1;
        if (num1 != 0)
          *guidChars1++ = (char) num1;
        char* chPtr2;
        if (flag2)
        {
          char* chPtr3 = guidChars1;
          char* chPtr4 = (char*) ((IntPtr) chPtr3 + 2);
          *chPtr3 = '0';
          char* chPtr5 = chPtr4;
          char* guidChars2 = (char*) ((IntPtr) chPtr5 + 2);
          *chPtr5 = 'x';
          char* guidChars3 = guidChars2 + Guid.HexsToChars(guidChars2, this._a >> 24, this._a >> 16);
          char* chPtr6 = guidChars3 + Guid.HexsToChars(guidChars3, this._a >> 8, this._a);
          char* chPtr7 = (char*) ((IntPtr) chPtr6 + 2);
          *chPtr6 = ',';
          char* chPtr8 = chPtr7;
          char* chPtr9 = (char*) ((IntPtr) chPtr8 + 2);
          *chPtr8 = '0';
          char* chPtr10 = chPtr9;
          char* guidChars4 = (char*) ((IntPtr) chPtr10 + 2);
          *chPtr10 = 'x';
          char* chPtr11 = guidChars4 + Guid.HexsToChars(guidChars4, (int) this._b >> 8, (int) this._b);
          char* chPtr12 = (char*) ((IntPtr) chPtr11 + 2);
          *chPtr11 = ',';
          char* chPtr13 = chPtr12;
          char* chPtr14 = (char*) ((IntPtr) chPtr13 + 2);
          *chPtr13 = '0';
          char* chPtr15 = chPtr14;
          char* guidChars5 = (char*) ((IntPtr) chPtr15 + 2);
          *chPtr15 = 'x';
          char* chPtr16 = guidChars5 + Guid.HexsToChars(guidChars5, (int) this._c >> 8, (int) this._c);
          char* chPtr17 = (char*) ((IntPtr) chPtr16 + 2);
          *chPtr16 = ',';
          char* chPtr18 = chPtr17;
          char* guidChars6 = (char*) ((IntPtr) chPtr18 + 2);
          *chPtr18 = '{';
          char* chPtr19 = guidChars6 + Guid.HexsToCharsHexOutput(guidChars6, (int) this._d, (int) this._e);
          char* guidChars7 = (char*) ((IntPtr) chPtr19 + 2);
          *chPtr19 = ',';
          char* chPtr20 = guidChars7 + Guid.HexsToCharsHexOutput(guidChars7, (int) this._f, (int) this._g);
          char* guidChars8 = (char*) ((IntPtr) chPtr20 + 2);
          *chPtr20 = ',';
          char* chPtr21 = guidChars8 + Guid.HexsToCharsHexOutput(guidChars8, (int) this._h, (int) this._i);
          char* guidChars9 = (char*) ((IntPtr) chPtr21 + 2);
          *chPtr21 = ',';
          char* chPtr22 = guidChars9 + Guid.HexsToCharsHexOutput(guidChars9, (int) this._j, (int) this._k);
          chPtr2 = (char*) ((IntPtr) chPtr22 + 2);
          *chPtr22 = '}';
        }
        else
        {
          char* guidChars10 = guidChars1 + Guid.HexsToChars(guidChars1, this._a >> 24, this._a >> 16);
          char* guidChars11 = guidChars10 + Guid.HexsToChars(guidChars10, this._a >> 8, this._a);
          if (flag1)
            *guidChars11++ = '-';
          char* guidChars12 = guidChars11 + Guid.HexsToChars(guidChars11, (int) this._b >> 8, (int) this._b);
          if (flag1)
            *guidChars12++ = '-';
          char* guidChars13 = guidChars12 + Guid.HexsToChars(guidChars12, (int) this._c >> 8, (int) this._c);
          if (flag1)
            *guidChars13++ = '-';
          char* guidChars14 = guidChars13 + Guid.HexsToChars(guidChars13, (int) this._d, (int) this._e);
          if (flag1)
            *guidChars14++ = '-';
          char* guidChars15 = guidChars14 + Guid.HexsToChars(guidChars14, (int) this._f, (int) this._g);
          char* guidChars16 = guidChars15 + Guid.HexsToChars(guidChars15, (int) this._h, (int) this._i);
          chPtr2 = guidChars16 + Guid.HexsToChars(guidChars16, (int) this._j, (int) this._k);
        }
        if (num1 != 0)
        {
          char* chPtr23 = chPtr2;
          char* chPtr24 = (char*) ((IntPtr) chPtr23 + 2);
          int num3 = (int) (ushort) (num1 >> 16);
          *chPtr23 = (char) num3;
        }
      }
      charsWritten = num2;
      return true;
    }


    #nullable disable
    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <param name="destination">The span in which to write this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, contains the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    bool ISpanFormattable.TryFormat(
      Span<char> destination,
      out int charsWritten,
      [StringSyntax("GuidFormat")] ReadOnlySpan<char> format,
      IFormatProvider provider)
    {
      return this.TryFormat(destination, out charsWritten, format);
    }

    /// <summary>Compares two values to determine which is less.</summary>
    /// <param name="left" />
    /// <param name="right" />
    /// <returns>
    /// <code data-dev-comment-type="langword">true</code> if <code data-dev-comment-type="paramref">left</code> is less than <code data-dev-comment-type="paramref">right</code>; otherwise, <code data-dev-comment-type="langword">false</code>.</returns>
    public static bool operator <(Guid left, Guid right)
    {
      if (left._a != right._a)
        return (uint) left._a < (uint) right._a;
      if ((int) left._b != (int) right._b)
        return (uint) left._b < (uint) right._b;
      if ((int) left._c != (int) right._c)
        return (uint) left._c < (uint) right._c;
      if ((int) left._d != (int) right._d)
        return (int) left._d < (int) right._d;
      if ((int) left._e != (int) right._e)
        return (int) left._e < (int) right._e;
      if ((int) left._f != (int) right._f)
        return (int) left._f < (int) right._f;
      if ((int) left._g != (int) right._g)
        return (int) left._g < (int) right._g;
      if ((int) left._h != (int) right._h)
        return (int) left._h < (int) right._h;
      if ((int) left._i != (int) right._i)
        return (int) left._i < (int) right._i;
      if ((int) left._j != (int) right._j)
        return (int) left._j < (int) right._j;
      return (int) left._k != (int) right._k && (int) left._k < (int) right._k;
    }

    /// <summary>Compares two values to determine which is less or equal.</summary>
    /// <param name="left" />
    /// <param name="right" />
    /// <returns>
    /// <code data-dev-comment-type="langword">true</code> if <code data-dev-comment-type="paramref">left</code> is less than or equal to <code data-dev-comment-type="paramref">right</code>; otherwise, <code data-dev-comment-type="langword">false</code>.</returns>
    public static bool operator <=(Guid left, Guid right)
    {
      if (left._a != right._a)
        return (uint) left._a < (uint) right._a;
      if ((int) left._b != (int) right._b)
        return (uint) left._b < (uint) right._b;
      if ((int) left._c != (int) right._c)
        return (uint) left._c < (uint) right._c;
      if ((int) left._d != (int) right._d)
        return (int) left._d < (int) right._d;
      if ((int) left._e != (int) right._e)
        return (int) left._e < (int) right._e;
      if ((int) left._f != (int) right._f)
        return (int) left._f < (int) right._f;
      if ((int) left._g != (int) right._g)
        return (int) left._g < (int) right._g;
      if ((int) left._h != (int) right._h)
        return (int) left._h < (int) right._h;
      if ((int) left._i != (int) right._i)
        return (int) left._i < (int) right._i;
      if ((int) left._j != (int) right._j)
        return (int) left._j < (int) right._j;
      return (int) left._k == (int) right._k || (int) left._k < (int) right._k;
    }

    /// <summary>Compares two values to determine which is greater.</summary>
    /// <param name="left" />
    /// <param name="right" />
    /// <returns>
    /// <code data-dev-comment-type="langword">true</code> if <code data-dev-comment-type="paramref">left</code> is greater than <code data-dev-comment-type="paramref">right</code>; otherwise, <code data-dev-comment-type="langword">false</code>.</returns>
    public static bool operator >(Guid left, Guid right)
    {
      if (left._a != right._a)
        return (uint) left._a > (uint) right._a;
      if ((int) left._b != (int) right._b)
        return (uint) left._b > (uint) right._b;
      if ((int) left._c != (int) right._c)
        return (uint) left._c > (uint) right._c;
      if ((int) left._d != (int) right._d)
        return (int) left._d > (int) right._d;
      if ((int) left._e != (int) right._e)
        return (int) left._e > (int) right._e;
      if ((int) left._f != (int) right._f)
        return (int) left._f > (int) right._f;
      if ((int) left._g != (int) right._g)
        return (int) left._g > (int) right._g;
      if ((int) left._h != (int) right._h)
        return (int) left._h > (int) right._h;
      if ((int) left._i != (int) right._i)
        return (int) left._i > (int) right._i;
      if ((int) left._j != (int) right._j)
        return (int) left._j > (int) right._j;
      return (int) left._k != (int) right._k && (int) left._k > (int) right._k;
    }

    /// <summary>Compares two values to determine which is greater or equal.</summary>
    /// <param name="left" />
    /// <param name="right" />
    /// <returns>
    /// <code data-dev-comment-type="langword">true</code> if <code data-dev-comment-type="paramref">left</code> is greater than or equal to <code data-dev-comment-type="paramref">right</code>; otherwise, <code data-dev-comment-type="langword">false</code>.</returns>
    public static bool operator >=(Guid left, Guid right)
    {
      if (left._a != right._a)
        return (uint) left._a > (uint) right._a;
      if ((int) left._b != (int) right._b)
        return (uint) left._b > (uint) right._b;
      if ((int) left._c != (int) right._c)
        return (uint) left._c > (uint) right._c;
      if ((int) left._d != (int) right._d)
        return (int) left._d > (int) right._d;
      if ((int) left._e != (int) right._e)
        return (int) left._e > (int) right._e;
      if ((int) left._f != (int) right._f)
        return (int) left._f > (int) right._f;
      if ((int) left._g != (int) right._g)
        return (int) left._g > (int) right._g;
      if ((int) left._h != (int) right._h)
        return (int) left._h > (int) right._h;
      if ((int) left._i != (int) right._i)
        return (int) left._i > (int) right._i;
      if ((int) left._j != (int) right._j)
        return (int) left._j > (int) right._j;
      return (int) left._k == (int) right._k || (int) left._k > (int) right._k;
    }


    #nullable enable
    /// <summary>Parses a string into a value.</summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <code data-dev-comment-type="paramref">s</code>.</param>
    /// <returns>The result of parsing <code data-dev-comment-type="paramref">s</code>.</returns>
    public static Guid Parse(string s, IFormatProvider? provider) => Guid.Parse(s);

    /// <summary>Tries to parse a string into a value.</summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <code data-dev-comment-type="paramref">s</code>.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <code data-dev-comment-type="paramref">s</code> or an undefined value on failure.</param>
    /// <returns>
    /// <code data-dev-comment-type="langword">true</code> if <code data-dev-comment-type="paramref">s</code> was successfully parsed; otherwise, <code data-dev-comment-type="langword">false</code>.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Guid result) => Guid.TryParse(s, out result);

    /// <summary>Parses a span of characters into a value.</summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <code data-dev-comment-type="paramref">s</code>.</param>
    /// <returns>The result of parsing <code data-dev-comment-type="paramref">s</code>.</returns>
    public static Guid Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Guid.Parse(s);

    /// <summary>Tries to parse a span of characters into a value.</summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <code data-dev-comment-type="paramref">s</code>.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <code data-dev-comment-type="paramref">s</code>, or an undefined value on failure.</param>
    /// <returns>
    /// <code data-dev-comment-type="langword">true</code> if <code data-dev-comment-type="paramref">s</code> was successfully parsed; otherwise, <code data-dev-comment-type="langword">false</code>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Guid result) => Guid.TryParse(s, out result);

    /// <summary>Initializes a new instance of the <see cref="T:System.Guid" /> structure.</summary>
    /// <returns>A new GUID object.</returns>
    public static unsafe Guid NewGuid()
    {
      Guid guid;
      Interop.GetCryptographicallySecureRandomBytes((byte*) &guid, sizeof (Guid));
      Unsafe.AsRef<short>(in guid._c) = (short) ((int) guid._c & -61441 | 16384);
      Unsafe.AsRef<byte>(in guid._d) = (byte) ((int) guid._d & -193 | 128);
      return guid;
    }


    #nullable disable
    private enum GuidParseThrowStyle : byte
    {
      None,
      All,
      AllButOverflow,
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct GuidResult
    {
      [FieldOffset(0)]
      internal uint _a;
      [FieldOffset(4)]
      internal uint _bc;
      [FieldOffset(4)]
      internal ushort _b;
      [FieldOffset(6)]
      internal ushort _c;
      [FieldOffset(8)]
      internal uint _defg;
      [FieldOffset(8)]
      internal ushort _de;
      [FieldOffset(8)]
      internal byte _d;
      [FieldOffset(10)]
      internal ushort _fg;
      [FieldOffset(12)]
      internal uint _hijk;
      [FieldOffset(16)]
      private readonly Guid.GuidParseThrowStyle _throwStyle;

      internal GuidResult(Guid.GuidParseThrowStyle canThrow)
        : this()
      {
        this._throwStyle = canThrow;
      }

      internal readonly void SetFailure(bool overflow, string failureMessageID)
      {
        if (this._throwStyle == Guid.GuidParseThrowStyle.None)
          return;
        if (!overflow)
          throw new FormatException(SR.GetResourceString(failureMessageID));
        if (this._throwStyle == Guid.GuidParseThrowStyle.All)
          throw new OverflowException(SR.GetResourceString(failureMessageID));
        throw new FormatException(SR.Format_GuidUnrecognized);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public readonly Guid ToGuid() => Unsafe.As<Guid.GuidResult, Guid>(ref Unsafe.AsRef<Guid.GuidResult>(in this));
    }
  }
}
