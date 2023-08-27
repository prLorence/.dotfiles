// Decompiled with JetBrains decompiler
// Type: System.Security.Claims.Claim
// Assembly: System.Security.Claims, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 27111040-48EA-4A11-B4D7-221BECC91DB0
// Assembly location: /usr/lib/dotnet/shared/Microsoft.NETCore.App/7.0.9/System.Security.Claims.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.NETCore.App.Ref/7.0.9/ref/net7.0/System.Security.Claims.xml

using System.Collections.Generic;
using System.IO;


#nullable enable
namespace System.Security.Claims
{
  /// <summary>Represents a claim.</summary>
  public class Claim
  {

    #nullable disable
    private readonly byte[] _userSerializationData;
    private readonly string _issuer;
    private readonly string _originalIssuer;
    private Dictionary<string, string> _properties;
    private readonly ClaimsIdentity _subject;
    private readonly string _type;
    private readonly string _value;
    private readonly string _valueType;


    #nullable enable
    /// <summary>Initializes an instance of <see cref="T:System.Security.Claims.Claim" /> with the specified <see cref="T:System.IO.BinaryReader" />.</summary>
    /// <param name="reader">A <see cref="T:System.IO.BinaryReader" /> pointing to a <see cref="T:System.Security.Claims.Claim" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="reader" /> is <see langword="null" />.</exception>
    public Claim(BinaryReader reader)
      : this(reader, (ClaimsIdentity) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class with the specified reader and subject.</summary>
    /// <param name="reader">The binary reader.</param>
    /// <param name="subject">The subject that this claim describes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="reader" /> is <see langword="null" />.</exception>
    public Claim(BinaryReader reader, ClaimsIdentity? subject)
    {
      ArgumentNullException.ThrowIfNull((object) reader, nameof (reader));
      this._subject = subject;
      Claim.SerializationMask serializationMask = (Claim.SerializationMask) reader.ReadInt32();
      int num1 = 1;
      int num2 = reader.ReadInt32();
      this._value = reader.ReadString();
      if ((serializationMask & Claim.SerializationMask.NameClaimType) == Claim.SerializationMask.NameClaimType)
        this._type = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
      else if ((serializationMask & Claim.SerializationMask.RoleClaimType) == Claim.SerializationMask.RoleClaimType)
      {
        this._type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
      }
      else
      {
        this._type = reader.ReadString();
        ++num1;
      }
      if ((serializationMask & Claim.SerializationMask.StringType) == Claim.SerializationMask.StringType)
      {
        this._valueType = reader.ReadString();
        ++num1;
      }
      else
        this._valueType = "http://www.w3.org/2001/XMLSchema#string";
      if ((serializationMask & Claim.SerializationMask.Issuer) == Claim.SerializationMask.Issuer)
      {
        this._issuer = reader.ReadString();
        ++num1;
      }
      else
        this._issuer = "LOCAL AUTHORITY";
      if ((serializationMask & Claim.SerializationMask.OriginalIssuerEqualsIssuer) == Claim.SerializationMask.OriginalIssuerEqualsIssuer)
        this._originalIssuer = this._issuer;
      else if ((serializationMask & Claim.SerializationMask.OriginalIssuer) == Claim.SerializationMask.OriginalIssuer)
      {
        this._originalIssuer = reader.ReadString();
        ++num1;
      }
      else
        this._originalIssuer = "LOCAL AUTHORITY";
      if ((serializationMask & Claim.SerializationMask.HasProperties) == Claim.SerializationMask.HasProperties)
      {
        int num3 = reader.ReadInt32();
        ++num1;
        for (int index = 0; index < num3; ++index)
          this.Properties.Add(reader.ReadString(), reader.ReadString());
      }
      if ((serializationMask & Claim.SerializationMask.UserData) == Claim.SerializationMask.UserData)
      {
        int count = reader.ReadInt32();
        this._userSerializationData = reader.ReadBytes(count);
        ++num1;
      }
      for (int index = num1; index < num2; ++index)
        reader.ReadString();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class with the specified claim type, and value.</summary>
    /// <param name="type">The claim type.</param>
    /// <param name="value">The claim value.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    public Claim(string type, string value)
      : this(type, value, "http://www.w3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY", (ClaimsIdentity) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class with the specified claim type, value, and value type.</summary>
    /// <param name="type">The claim type.</param>
    /// <param name="value">The claim value.</param>
    /// <param name="valueType">The claim value type. If this parameter is <see langword="null" />, then <see cref="F:System.Security.Claims.ClaimValueTypes.String" /> is used.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    public Claim(string type, string value, string? valueType)
      : this(type, value, valueType, "LOCAL AUTHORITY", "LOCAL AUTHORITY", (ClaimsIdentity) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class with the specified claim type, value, value type, and issuer.</summary>
    /// <param name="type">The claim type.</param>
    /// <param name="value">The claim value.</param>
    /// <param name="valueType">The claim value type. If this parameter is <see langword="null" />, then <see cref="F:System.Security.Claims.ClaimValueTypes.String" /> is used.</param>
    /// <param name="issuer">The claim issuer. If this parameter is empty or <see langword="null" />, then <see cref="F:System.Security.Claims.ClaimsIdentity.DefaultIssuer" /> is used.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    public Claim(string type, string value, string? valueType, string? issuer)
      : this(type, value, valueType, issuer, issuer, (ClaimsIdentity) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class with the specified claim type, value, value type, issuer,  and original issuer.</summary>
    /// <param name="type">The claim type.</param>
    /// <param name="value">The claim value.</param>
    /// <param name="valueType">The claim value type. If this parameter is <see langword="null" />, then <see cref="F:System.Security.Claims.ClaimValueTypes.String" /> is used.</param>
    /// <param name="issuer">The claim issuer. If this parameter is empty or <see langword="null" />, then <see cref="F:System.Security.Claims.ClaimsIdentity.DefaultIssuer" /> is used.</param>
    /// <param name="originalIssuer">The original issuer of the claim. If this parameter is empty or <see langword="null" />, then the <see cref="P:System.Security.Claims.Claim.OriginalIssuer" /> property is set to the value of the <see cref="P:System.Security.Claims.Claim.Issuer" /> property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    public Claim(
      string type,
      string value,
      string? valueType,
      string? issuer,
      string? originalIssuer)
      : this(type, value, valueType, issuer, originalIssuer, (ClaimsIdentity) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class with the specified claim type, value, value type, issuer, original issuer and subject.</summary>
    /// <param name="type">The claim type.</param>
    /// <param name="value">The claim value.</param>
    /// <param name="valueType">The claim value type. If this parameter is <see langword="null" />, then <see cref="F:System.Security.Claims.ClaimValueTypes.String" /> is used.</param>
    /// <param name="issuer">The claim issuer. If this parameter is empty or <see langword="null" />, then <see cref="F:System.Security.Claims.ClaimsIdentity.DefaultIssuer" /> is used.</param>
    /// <param name="originalIssuer">The original issuer of the claim. If this parameter is empty or <see langword="null" />, then the <see cref="P:System.Security.Claims.Claim.OriginalIssuer" /> property is set to the value of the <see cref="P:System.Security.Claims.Claim.Issuer" /> property.</param>
    /// <param name="subject">The subject that this claim describes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    public Claim(
      string type,
      string value,
      string? valueType,
      string? issuer,
      string? originalIssuer,
      ClaimsIdentity? subject)
      : this(type, value, valueType, issuer, originalIssuer, subject, (string) null, (string) null)
    {
    }


    #nullable disable
    internal Claim(
      string type,
      string value,
      string valueType,
      string issuer,
      string originalIssuer,
      ClaimsIdentity subject,
      string propertyKey,
      string propertyValue)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      ArgumentNullException.ThrowIfNull((object) value, nameof (value));
      this._type = type;
      this._value = value;
      this._valueType = string.IsNullOrEmpty(valueType) ? "http://www.w3.org/2001/XMLSchema#string" : valueType;
      this._issuer = string.IsNullOrEmpty(issuer) ? "LOCAL AUTHORITY" : issuer;
      this._originalIssuer = string.IsNullOrEmpty(originalIssuer) ? this._issuer : originalIssuer;
      this._subject = subject;
      if (propertyKey == null)
        return;
      this._properties = new Dictionary<string, string>();
      this._properties[propertyKey] = propertyValue;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class.</summary>
    /// <param name="other">The security claim.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="other" /> is <see langword="null" />.</exception>
    protected Claim(Claim other)
      : this(other, other == null ? (ClaimsIdentity) null : other._subject)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Security.Claims.Claim" /> class with the specified security claim and subject.</summary>
    /// <param name="other">The security claim.</param>
    /// <param name="subject">The subject that this claim describes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="other" /> is <see langword="null" />.</exception>
    protected Claim(Claim other, ClaimsIdentity? subject)
    {
      ArgumentNullException.ThrowIfNull((object) other, nameof (other));
      this._issuer = other._issuer;
      this._originalIssuer = other._originalIssuer;
      this._subject = subject;
      this._type = other._type;
      this._value = other._value;
      this._valueType = other._valueType;
      if (other._properties != null)
        this._properties = new Dictionary<string, string>((IDictionary<string, string>) other._properties);
      if (other._userSerializationData == null)
        return;
      this._userSerializationData = other._userSerializationData.Clone() as byte[];
    }

    /// <summary>Contains any additional data provided by a derived type.</summary>
    /// <returns>A <see cref="T:System.Byte" /> array representing the additional serialized data.</returns>
    protected virtual byte[]? CustomSerializationData => this._userSerializationData;

    /// <summary>Gets the issuer of the claim.</summary>
    /// <returns>A name that refers to the issuer of the claim.</returns>
    public string Issuer => this._issuer;

    /// <summary>Gets the original issuer of the claim.</summary>
    /// <returns>A name that refers to the original issuer of the claim.</returns>
    public string OriginalIssuer => this._originalIssuer;

    /// <summary>Gets a dictionary that contains additional properties associated with this claim.</summary>
    /// <returns>A dictionary that contains additional properties associated with the claim. The properties are represented as name-value pairs.</returns>
    public IDictionary<string, string> Properties => (IDictionary<string, string>) this._properties ?? (IDictionary<string, string>) (this._properties = new Dictionary<string, string>());

    /// <summary>Gets the subject of the claim.</summary>
    /// <returns>The subject of the claim.</returns>
    public ClaimsIdentity? Subject => this._subject;

    /// <summary>Gets the claim type of the claim.</summary>
    /// <returns>The claim type.</returns>
    public string Type => this._type;

    /// <summary>Gets the value of the claim.</summary>
    /// <returns>The claim value.</returns>
    public string Value => this._value;

    /// <summary>Gets the value type of the claim.</summary>
    /// <returns>The claim value type.</returns>
    public string ValueType => this._valueType;

    /// <summary>Returns a new <see cref="T:System.Security.Claims.Claim" /> object copied from this object. The new claim does not have a subject.</summary>
    /// <returns>The new claim object.</returns>
    public virtual Claim Clone() => this.Clone((ClaimsIdentity) null);

    /// <summary>Returns a new <see cref="T:System.Security.Claims.Claim" /> object copied from this object. The subject of the new claim is set to the specified ClaimsIdentity.</summary>
    /// <param name="identity">The intended subject of the new claim.</param>
    /// <returns>The new claim object.</returns>
    public virtual Claim Clone(ClaimsIdentity? identity) => new Claim(this, identity);

    /// <summary>Writes this <see cref="T:System.Security.Claims.Claim" /> to the writer.</summary>
    /// <param name="writer">The writer to use for data storage.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="writer" /> is <see langword="null" />.</exception>
    public virtual void WriteTo(BinaryWriter writer) => this.WriteTo(writer, (byte[]) null);

    /// <summary>Writes this <see cref="T:System.Security.Claims.Claim" /> to the writer.</summary>
    /// <param name="writer">The writer to write this claim.</param>
    /// <param name="userData">The user data to claim.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="writer" /> is <see langword="null" />.</exception>
    protected virtual void WriteTo(BinaryWriter writer, byte[]? userData)
    {
      ArgumentNullException.ThrowIfNull((object) writer, nameof (writer));
      int num = 1;
      Claim.SerializationMask serializationMask = Claim.SerializationMask.None;
      if (string.Equals(this._type, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"))
        serializationMask |= Claim.SerializationMask.NameClaimType;
      else if (string.Equals(this._type, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
        serializationMask |= Claim.SerializationMask.RoleClaimType;
      else
        ++num;
      if (!string.Equals(this._valueType, "http://www.w3.org/2001/XMLSchema#string", StringComparison.Ordinal))
      {
        ++num;
        serializationMask |= Claim.SerializationMask.StringType;
      }
      if (!string.Equals(this._issuer, "LOCAL AUTHORITY", StringComparison.Ordinal))
      {
        ++num;
        serializationMask |= Claim.SerializationMask.Issuer;
      }
      if (string.Equals(this._originalIssuer, this._issuer, StringComparison.Ordinal))
        serializationMask |= Claim.SerializationMask.OriginalIssuerEqualsIssuer;
      else if (!string.Equals(this._originalIssuer, "LOCAL AUTHORITY"))
      {
        ++num;
        serializationMask |= Claim.SerializationMask.OriginalIssuer;
      }
      if (this._properties != null && this._properties.Count > 0)
      {
        ++num;
        serializationMask |= Claim.SerializationMask.HasProperties;
      }
      if (userData != null && userData.Length != 0)
      {
        ++num;
        serializationMask |= Claim.SerializationMask.UserData;
      }
      writer.Write((int) serializationMask);
      writer.Write(num);
      writer.Write(this._value);
      if ((serializationMask & Claim.SerializationMask.NameClaimType) != Claim.SerializationMask.NameClaimType && (serializationMask & Claim.SerializationMask.RoleClaimType) != Claim.SerializationMask.RoleClaimType)
        writer.Write(this._type);
      if ((serializationMask & Claim.SerializationMask.StringType) == Claim.SerializationMask.StringType)
        writer.Write(this._valueType);
      if ((serializationMask & Claim.SerializationMask.Issuer) == Claim.SerializationMask.Issuer)
        writer.Write(this._issuer);
      if ((serializationMask & Claim.SerializationMask.OriginalIssuer) == Claim.SerializationMask.OriginalIssuer)
        writer.Write(this._originalIssuer);
      if ((serializationMask & Claim.SerializationMask.HasProperties) == Claim.SerializationMask.HasProperties)
      {
        writer.Write(this._properties.Count);
        foreach (KeyValuePair<string, string> property in this._properties)
        {
          writer.Write(property.Key);
          writer.Write(property.Value);
        }
      }
      if ((serializationMask & Claim.SerializationMask.UserData) == Claim.SerializationMask.UserData)
      {
        writer.Write(userData.Length);
        writer.Write(userData);
      }
      writer.Flush();
    }

    /// <summary>Returns a string representation of this <see cref="T:System.Security.Claims.Claim" /> object.</summary>
    /// <returns>The string representation of this <see cref="T:System.Security.Claims.Claim" /> object.</returns>
    public override string ToString() => this._type + ": " + this._value;


    #nullable disable
    private enum SerializationMask
    {
      None = 0,
      NameClaimType = 1,
      RoleClaimType = 2,
      StringType = 4,
      Issuer = 8,
      OriginalIssuerEqualsIssuer = 16, // 0x00000010
      OriginalIssuer = 32, // 0x00000020
      HasProperties = 64, // 0x00000040
      UserData = 128, // 0x00000080
    }
  }
}
