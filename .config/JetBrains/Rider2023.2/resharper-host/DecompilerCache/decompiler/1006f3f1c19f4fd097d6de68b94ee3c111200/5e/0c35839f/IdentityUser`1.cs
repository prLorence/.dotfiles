// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Identity.IdentityUser`1
// Assembly: Microsoft.Extensions.Identity.Stores, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 1006F3F1-C19F-4FD0-97D6-DE68B94EE3C1
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.Extensions.Identity.Stores.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.Extensions.Identity.Stores.xml

using System;


#nullable enable
namespace Microsoft.AspNetCore.Identity
{
  /// <summary>Represents a user in the identity system</summary>
  /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
  public class IdentityUser<TKey> where TKey : IEquatable<TKey>
  {
    /// <summary>
    /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Identity.IdentityUser`1" />.
    /// </summary>
    public IdentityUser()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:Microsoft.AspNetCore.Identity.IdentityUser`1" />.
    /// </summary>
    /// <param name="userName">The user name.</param>
    public IdentityUser(string userName)
      : this()
    {
      this.UserName = userName;
    }

    /// <summary>Gets or sets the primary key for this user.</summary>
    [PersonalData]
    public virtual TKey Id { get; set; }

    /// <summary>Gets or sets the user name for this user.</summary>
    [ProtectedPersonalData]
    public virtual string? UserName { get; set; }

    /// <summary>Gets or sets the normalized user name for this user.</summary>
    public virtual string? NormalizedUserName { get; set; }

    /// <summary>Gets or sets the email address for this user.</summary>
    [ProtectedPersonalData]
    public virtual string? Email { get; set; }

    /// <summary>
    /// Gets or sets the normalized email address for this user.
    /// </summary>
    public virtual string? NormalizedEmail { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their email address.
    /// </summary>
    /// <value>True if the email address has been confirmed, otherwise false.</value>
    [PersonalData]
    public virtual bool EmailConfirmed { get; set; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password for this user.
    /// </summary>
    public virtual string? PasswordHash { get; set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    public virtual string? SecurityStamp { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>Gets or sets a telephone number for the user.</summary>
    [ProtectedPersonalData]
    public virtual string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their telephone address.
    /// </summary>
    /// <value>True if the telephone number has been confirmed, otherwise false.</value>
    [PersonalData]
    public virtual bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
    /// </summary>
    /// <value>True if 2fa is enabled, otherwise false.</value>
    [PersonalData]
    public virtual bool TwoFactorEnabled { get; set; }

    /// <summary>
    /// Gets or sets the date and time, in UTC, when any user lockout ends.
    /// </summary>
    /// <remarks>A value in the past means the user is not locked out.</remarks>
    public virtual DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    public virtual bool LockoutEnabled { get; set; }

    /// <summary>
    /// Gets or sets the number of failed login attempts for the current user.
    /// </summary>
    public virtual int AccessFailedCount { get; set; }

    /// <summary>Returns the username for this user.</summary>
    public override string ToString() => this.UserName ?? string.Empty;
  }
}
