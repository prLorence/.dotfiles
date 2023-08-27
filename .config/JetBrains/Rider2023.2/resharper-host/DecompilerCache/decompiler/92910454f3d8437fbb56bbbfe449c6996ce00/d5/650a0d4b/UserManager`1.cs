// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Identity.UserManager`1
// Assembly: Microsoft.Extensions.Identity.Core, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 92910454-F3D8-437F-BB56-BBBFE449C699
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.Extensions.Identity.Core.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.Extensions.Identity.Core.xml

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Identity.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Microsoft.AspNetCore.Identity
{
  /// <summary>
  /// Provides the APIs for managing user in a persistence store.
  /// </summary>
  /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
  public class UserManager<TUser> : IDisposable where TUser : class
  {
    /// <summary>
    /// The data protection purpose used for the reset password related methods.
    /// </summary>
    public const string ResetPasswordTokenPurpose = "ResetPassword";
    /// <summary>
    /// The data protection purpose used for the change phone number methods.
    /// </summary>
    public const string ChangePhoneNumberTokenPurpose = "ChangePhoneNumber";
    /// <summary>
    /// The data protection purpose used for the email confirmation related methods.
    /// </summary>
    public const string ConfirmEmailTokenPurpose = "EmailConfirmation";

    #nullable disable
    private readonly Dictionary<string, IUserTwoFactorTokenProvider<TUser>> _tokenProviders = new Dictionary<string, IUserTwoFactorTokenProvider<TUser>>();
    private bool _disposed;
    private readonly IServiceProvider _services;
    private static readonly char[] AllowedChars = "23456789BCDFGHJKMNPQRTVWXY".ToCharArray();

    /// <summary>The cancellation token used to cancel operations.</summary>
    protected virtual CancellationToken CancellationToken => CancellationToken.None;


    #nullable enable
    /// <summary>
    /// Constructs a new instance of <see cref="T:Microsoft.AspNetCore.Identity.UserManager`1" />.
    /// </summary>
    /// <param name="store">The persistence store the manager will operate over.</param>
    /// <param name="optionsAccessor">The accessor used to access the <see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" />.</param>
    /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
    /// <param name="userValidators">A collection of <see cref="T:Microsoft.AspNetCore.Identity.IUserValidator`1" /> to validate users against.</param>
    /// <param name="passwordValidators">A collection of <see cref="T:Microsoft.AspNetCore.Identity.IPasswordValidator`1" /> to validate passwords against.</param>
    /// <param name="keyNormalizer">The <see cref="T:Microsoft.AspNetCore.Identity.ILookupNormalizer" /> to use when generating index keys for users.</param>
    /// <param name="errors">The <see cref="T:Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> used to provider error messages.</param>
    /// <param name="services">The <see cref="T:System.IServiceProvider" /> used to resolve services.</param>
    /// <param name="logger">The logger used to log messages, warnings and errors.</param>
    public UserManager(
      IUserStore<TUser> store,
      IOptions<IdentityOptions> optionsAccessor,
      IPasswordHasher<TUser> passwordHasher,
      IEnumerable<IUserValidator<TUser>> userValidators,
      IEnumerable<IPasswordValidator<TUser>> passwordValidators,
      ILookupNormalizer keyNormalizer,
      IdentityErrorDescriber errors,
      IServiceProvider services,
      ILogger<UserManager<TUser>> logger)
    {
      this.Store = store != null ? store : throw new ArgumentNullException(nameof (store));
      this.Options = optionsAccessor?.Value ?? new IdentityOptions();
      this.PasswordHasher = passwordHasher;
      this.KeyNormalizer = keyNormalizer;
      this.ErrorDescriber = errors;
      this.Logger = (ILogger) logger;
      if (userValidators != null)
      {
        foreach (IUserValidator<TUser> userValidator in userValidators)
          this.UserValidators.Add(userValidator);
      }
      if (passwordValidators != null)
      {
        foreach (IPasswordValidator<TUser> passwordValidator in passwordValidators)
          this.PasswordValidators.Add(passwordValidator);
      }
      this._services = services;
      if (services != null)
      {
        foreach (string key in this.Options.Tokens.ProviderMap.Keys)
        {
          TokenProviderDescriptor provider1 = this.Options.Tokens.ProviderMap[key];
          if ((provider1.ProviderInstance ?? services.GetRequiredService(provider1.ProviderType)) is IUserTwoFactorTokenProvider<TUser> provider2)
            this.RegisterTokenProvider(key, provider2);
        }
      }
      if (!this.Options.Stores.ProtectPersonalData)
        return;
      if (!(this.Store is IProtectedUserStore<TUser>))
        throw new InvalidOperationException(Resources.StoreNotIProtectedUserStore);
      if ((services != null ? services.GetService<ILookupProtector>() : (ILookupProtector) null) == null)
        throw new InvalidOperationException(Resources.NoPersonalDataProtector);
    }

    /// <summary>
    /// Gets or sets the persistence store the manager operates over.
    /// </summary>
    /// <value>The persistence store the manager operates over.</value>
    protected internal IUserStore<TUser> Store { get; set; }

    /// <summary>
    /// The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> used to log messages from the manager.
    /// </summary>
    /// <value>
    /// The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> used to log messages from the manager.
    /// </value>
    public virtual ILogger Logger { get; set; }

    /// <summary>
    /// The <see cref="T:Microsoft.AspNetCore.Identity.IPasswordHasher`1" /> used to hash passwords.
    /// </summary>
    public IPasswordHasher<TUser> PasswordHasher { get; set; }

    /// <summary>
    /// The <see cref="T:Microsoft.AspNetCore.Identity.IUserValidator`1" /> used to validate users.
    /// </summary>
    public IList<IUserValidator<TUser>> UserValidators { get; } = (IList<IUserValidator<TUser>>) new List<IUserValidator<TUser>>();

    /// <summary>
    /// The <see cref="T:Microsoft.AspNetCore.Identity.IPasswordValidator`1" /> used to validate passwords.
    /// </summary>
    public IList<IPasswordValidator<TUser>> PasswordValidators { get; } = (IList<IPasswordValidator<TUser>>) new List<IPasswordValidator<TUser>>();

    /// <summary>
    /// The <see cref="T:Microsoft.AspNetCore.Identity.ILookupNormalizer" /> used to normalize things like user and role names.
    /// </summary>
    public ILookupNormalizer KeyNormalizer { get; set; }

    /// <summary>
    /// The <see cref="T:Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> used to generate error messages.
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// The <see cref="T:Microsoft.AspNetCore.Identity.IdentityOptions" /> used to configure Identity.
    /// </summary>
    public IdentityOptions Options { get; set; }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports authentication tokens.
    /// </summary>
    /// <value>
    /// true if the backing user store supports authentication tokens, otherwise false.
    /// </value>
    public virtual bool SupportsUserAuthenticationTokens
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserAuthenticationTokenStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports a user authenticator.
    /// </summary>
    /// <value>
    /// true if the backing user store supports a user authenticator, otherwise false.
    /// </value>
    public virtual bool SupportsUserAuthenticatorKey
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserAuthenticatorKeyStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports recovery codes.
    /// </summary>
    /// <value>
    /// true if the backing user store supports a user authenticator, otherwise false.
    /// </value>
    public virtual bool SupportsUserTwoFactorRecoveryCodes
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserTwoFactorRecoveryCodeStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports two factor authentication.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user two factor authentication, otherwise false.
    /// </value>
    public virtual bool SupportsUserTwoFactor
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserTwoFactorStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports user passwords.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user passwords, otherwise false.
    /// </value>
    public virtual bool SupportsUserPassword
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserPasswordStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports security stamps.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user security stamps, otherwise false.
    /// </value>
    public virtual bool SupportsUserSecurityStamp
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserSecurityStampStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports user roles.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user roles, otherwise false.
    /// </value>
    public virtual bool SupportsUserRole
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserRoleStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports external logins.
    /// </summary>
    /// <value>
    /// true if the backing user store supports external logins, otherwise false.
    /// </value>
    public virtual bool SupportsUserLogin
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserLoginStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports user emails.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user emails, otherwise false.
    /// </value>
    public virtual bool SupportsUserEmail
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserEmailStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports user telephone numbers.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user telephone numbers, otherwise false.
    /// </value>
    public virtual bool SupportsUserPhoneNumber
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserPhoneNumberStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports user claims.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user claims, otherwise false.
    /// </value>
    public virtual bool SupportsUserClaim
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserClaimStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports user lock-outs.
    /// </summary>
    /// <value>
    /// true if the backing user store supports user lock-outs, otherwise false.
    /// </value>
    public virtual bool SupportsUserLockout
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IUserLockoutStore<TUser>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the backing user store supports returning
    /// <see cref="T:System.Linq.IQueryable" /> collections of information.
    /// </summary>
    /// <value>
    /// true if the backing user store supports returning <see cref="T:System.Linq.IQueryable" /> collections of
    /// information, otherwise false.
    /// </value>
    public virtual bool SupportsQueryableUsers
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IQueryableUserStore<TUser>;
      }
    }

    /// <summary>
    ///     Returns an IQueryable of users if the store is an IQueryableUserStore
    /// </summary>
    public virtual IQueryable<TUser> Users => this.Store is IQueryableUserStore<TUser> store ? store.Users : throw new NotSupportedException(Resources.StoreNotIQueryableUserStore);

    /// <summary>Releases all resources used by the user manager.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Returns the Name claim value if present otherwise returns null.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> instance.</param>
    /// <returns>The Name claim value, or null if the claim is not present.</returns>
    /// <remarks>The Name claim is identified by <see cref="F:System.Security.Claims.ClaimsIdentity.DefaultNameClaimType" />.</remarks>
    public virtual string? GetUserName(ClaimsPrincipal principal)
    {
      if (principal == null)
        throw new ArgumentNullException(nameof (principal));
      return principal.FindFirstValue(this.Options.ClaimsIdentity.UserNameClaimType);
    }

    /// <summary>
    /// Returns the User ID claim value if present otherwise returns null.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> instance.</param>
    /// <returns>The User ID claim value, or null if the claim is not present.</returns>
    /// <remarks>The User ID claim is identified by <see cref="F:System.Security.Claims.ClaimTypes.NameIdentifier" />.</remarks>
    public virtual string? GetUserId(ClaimsPrincipal principal)
    {
      if (principal == null)
        throw new ArgumentNullException(nameof (principal));
      return principal.FindFirstValue(this.Options.ClaimsIdentity.UserIdClaimType);
    }

    /// <summary>
    /// Returns the user corresponding to the IdentityOptions.ClaimsIdentity.UserIdClaimType claim in
    /// the principal or null.
    /// </summary>
    /// <param name="principal">The principal which contains the user id claim.</param>
    /// <returns>The user corresponding to the IdentityOptions.ClaimsIdentity.UserIdClaimType claim in
    /// the principal or null</returns>
    public virtual Task<TUser?> GetUserAsync(ClaimsPrincipal principal)
    {
      string userId = principal != null ? this.GetUserId(principal) : throw new ArgumentNullException(nameof (principal));
      return userId != null ? this.FindByIdAsync(userId) : Task.FromResult<TUser>(default (TUser));
    }

    /// <summary>
    /// Generates a value suitable for use in concurrency tracking.
    /// </summary>
    /// <param name="user">The user to generate the stamp for.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the security
    /// stamp for the specified <paramref name="user" />.
    /// </returns>
    public virtual Task<string> GenerateConcurrencyStampAsync(TUser user) => Task.FromResult<string>(Guid.NewGuid().ToString());

    /// <summary>
    /// Creates the specified <paramref name="user" /> in the backing store with no password,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> CreateAsync(TUser user)
    {
      this.ThrowIfDisposed();
      ConfiguredTaskAwaitable configuredTaskAwaitable1 = this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      await configuredTaskAwaitable1;
      ConfiguredTaskAwaitable<IdentityResult> configuredTaskAwaitable2 = this.ValidateUserAsync(user).ConfigureAwait(false);
      IdentityResult async = await configuredTaskAwaitable2;
      if (!async.Succeeded)
        return async;
      if (this.Options.Lockout.AllowedForNewUsers && this.SupportsUserLockout)
      {
        configuredTaskAwaitable1 = this.GetUserLockoutStore().SetLockoutEnabledAsync(user, true, this.CancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable1;
      }
      configuredTaskAwaitable1 = this.UpdateNormalizedUserNameAsync(user).ConfigureAwait(false);
      await configuredTaskAwaitable1;
      configuredTaskAwaitable1 = this.UpdateNormalizedEmailAsync(user).ConfigureAwait(false);
      await configuredTaskAwaitable1;
      configuredTaskAwaitable2 = this.Store.CreateAsync(user, this.CancellationToken).ConfigureAwait(false);
      return await configuredTaskAwaitable2;
    }

    /// <summary>
    /// Updates the specified <paramref name="user" /> in the backing store.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual Task<IdentityResult> UpdateAsync(TUser user)
    {
      this.ThrowIfDisposed();
      return (object) user != null ? this.UpdateUserAsync(user) : throw new ArgumentNullException(nameof (user));
    }

    /// <summary>
    /// Deletes the specified <paramref name="user" /> from the backing store.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual Task<IdentityResult> DeleteAsync(TUser user)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      return this.Store.DeleteAsync(user, this.CancellationToken);
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId" />.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId" /> if it exists.
    /// </returns>
    public virtual Task<TUser?> FindByIdAsync(string userId)
    {
      this.ThrowIfDisposed();
      return this.Store.FindByIdAsync(userId, this.CancellationToken);
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified user name.
    /// </summary>
    /// <param name="userName">The user name to search for.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user matching the specified <paramref name="userName" /> if it exists.
    /// </returns>
    public virtual async Task<TUser?> FindByNameAsync(string userName)
    {
      this.ThrowIfDisposed();
      userName = userName != null ? this.NormalizeName(userName) : throw new ArgumentNullException(nameof (userName));
      TUser byNameAsync = await this.Store.FindByNameAsync(userName, this.CancellationToken).ConfigureAwait(false);
      if ((object) byNameAsync == null && this.Options.Stores.ProtectPersonalData)
      {
        ILookupProtectorKeyRing service = this._services.GetService<ILookupProtectorKeyRing>();
        ILookupProtector protector = this._services.GetService<ILookupProtector>();
        if (service != null && protector != null)
        {
          foreach (string allKeyId in service.GetAllKeyIds())
          {
            byNameAsync = await this.Store.FindByNameAsync(protector.Protect(allKeyId, userName), this.CancellationToken).ConfigureAwait(false);
            if ((object) byNameAsync != null)
              return byNameAsync;
          }
        }
        protector = (ILookupProtector) null;
      }
      return byNameAsync;
    }

    /// <summary>
    /// Creates the specified <paramref name="user" /> in the backing store with given password,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="password">The password for the user to hash and store.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> CreateAsync(TUser user, string password)
    {
      this.ThrowIfDisposed();
      IUserPasswordStore<TUser> passwordStore = this.GetPasswordStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (password == null)
        throw new ArgumentNullException(nameof (password));
      IdentityResult identityResult = await this.UpdatePasswordHash(passwordStore, user, password).ConfigureAwait(false);
      return !identityResult.Succeeded ? identityResult : await this.CreateAsync(user).ConfigureAwait(false);
    }

    /// <summary>Normalize user or role name for consistent comparisons.</summary>
    /// <param name="name">The name to normalize.</param>
    /// <returns>A normalized value representing the specified <paramref name="name" />.</returns>
    [return: NotNullIfNotNull("name")]
    public virtual string? NormalizeName(string? name) => this.KeyNormalizer != null ? this.KeyNormalizer.NormalizeName(name) : name;

    /// <summary>Normalize email for consistent comparisons.</summary>
    /// <param name="email">The email to normalize.</param>
    /// <returns>A normalized value representing the specified <paramref name="email" />.</returns>
    [return: NotNullIfNotNull("email")]
    public virtual string? NormalizeEmail(string? email) => this.KeyNormalizer != null ? this.KeyNormalizer.NormalizeEmail(email) : email;


    #nullable disable
    [return: NotNullIfNotNull("data")]
    private string ProtectPersonalData(string data)
    {
      if (!this.Options.Stores.ProtectPersonalData)
        return data;
      ILookupProtectorKeyRing requiredService = this._services.GetRequiredService<ILookupProtectorKeyRing>();
      return this._services.GetRequiredService<ILookupProtector>().Protect(requiredService.CurrentKeyId, data);
    }


    #nullable enable
    /// <summary>
    /// Updates the normalized user name for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose user name should be normalized and updated.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public virtual async Task UpdateNormalizedUserNameAsync(TUser user)
    {
      string normalizedName = this.ProtectPersonalData(this.NormalizeName(await this.GetUserNameAsync(user).ConfigureAwait(false)));
      await this.Store.SetNormalizedUserNameAsync(user, normalizedName, this.CancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the user name for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose name should be retrieved.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the name for the specified <paramref name="user" />.</returns>
    public virtual async Task<string?> GetUserNameAsync(TUser user)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      return await this.Store.GetUserNameAsync(user, this.CancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the given <paramref name="userName" /> for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="userName">The user name to set.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.</returns>
    public virtual async Task<IdentityResult> SetUserNameAsync(TUser user, string? userName)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      await this.Store.SetUserNameAsync(user, userName, this.CancellationToken).ConfigureAwait(false);
      await this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the user identifier for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose identifier should be retrieved.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user" />.</returns>
    public virtual async Task<string> GetUserIdAsync(TUser user)
    {
      this.ThrowIfDisposed();
      return await this.Store.GetUserIdAsync(user, this.CancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns a flag indicating whether the given <paramref name="password" /> is valid for the
    /// specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose password should be validated.</param>
    /// <param name="password">The password to validate</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing true if
    /// the specified <paramref name="password" /> matches the one store for the <paramref name="user" />,
    /// otherwise false.</returns>
    public virtual async Task<bool> CheckPasswordAsync(TUser user, string password)
    {
      this.ThrowIfDisposed();
      IUserPasswordStore<TUser> passwordStore = this.GetPasswordStore();
      if ((object) user == null)
        return false;
      PasswordVerificationResult result = await this.VerifyPasswordAsync(passwordStore, user, password).ConfigureAwait(false);
      if (result == PasswordVerificationResult.SuccessRehashNeeded)
      {
        IdentityResult identityResult1 = await this.UpdatePasswordHash(passwordStore, user, password, false).ConfigureAwait(false);
        IdentityResult identityResult2 = await this.UpdateUserAsync(user).ConfigureAwait(false);
      }
      int num = result != 0 ? 1 : 0;
      if (num == 0)
        this.Logger.LogDebug(LoggerEventIds.InvalidPassword, "Invalid password for user.");
      return num != 0;
    }

    /// <summary>
    /// Gets a flag indicating whether the specified <paramref name="user" /> has a password.
    /// </summary>
    /// <param name="user">The user to return a flag for, indicating whether they have a password or not.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning true if the specified <paramref name="user" /> has a password
    /// otherwise false.
    /// </returns>
    public virtual Task<bool> HasPasswordAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserPasswordStore<TUser> passwordStore = this.GetPasswordStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return passwordStore.HasPasswordAsync(user1, cancellationToken);
    }

    /// <summary>
    /// Adds the <paramref name="password" /> to the specified <paramref name="user" /> only if the user
    /// does not already have a password.
    /// </summary>
    /// <param name="user">The user whose password should be set.</param>
    /// <param name="password">The password to set.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> AddPasswordAsync(TUser user, string password)
    {
      this.ThrowIfDisposed();
      IUserPasswordStore<TUser> passwordStore = this.GetPasswordStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (await passwordStore.GetPasswordHashAsync(user, this.CancellationToken).ConfigureAwait(false) != null)
      {
        this.Logger.LogDebug(LoggerEventIds.UserAlreadyHasPassword, "User already has a password.");
        return IdentityResult.Failed(this.ErrorDescriber.UserAlreadyHasPassword());
      }
      IdentityResult identityResult = await this.UpdatePasswordHash(passwordStore, user, password).ConfigureAwait(false);
      return !identityResult.Succeeded ? identityResult : await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Changes a user's password after confirming the specified <paramref name="currentPassword" /> is correct,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose password should be set.</param>
    /// <param name="currentPassword">The current password to validate before changing.</param>
    /// <param name="newPassword">The new password to set for the specified <paramref name="user" />.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> ChangePasswordAsync(
      TUser user,
      string currentPassword,
      string newPassword)
    {
      this.ThrowIfDisposed();
      IUserPasswordStore<TUser> passwordStore = this.GetPasswordStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (await this.VerifyPasswordAsync(passwordStore, user, currentPassword).ConfigureAwait(false) != PasswordVerificationResult.Failed)
      {
        IdentityResult identityResult = await this.UpdatePasswordHash(passwordStore, user, newPassword).ConfigureAwait(false);
        return !identityResult.Succeeded ? identityResult : await this.UpdateUserAsync(user).ConfigureAwait(false);
      }
      this.Logger.LogDebug(LoggerEventIds.ChangePasswordFailed, "Change password failed for user.");
      return IdentityResult.Failed(this.ErrorDescriber.PasswordMismatch());
    }

    /// <summary>Removes a user's password.</summary>
    /// <param name="user">The user whose password should be removed.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> RemovePasswordAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserPasswordStore<TUser> passwordStore = this.GetPasswordStore();
      ConfiguredTaskAwaitable<IdentityResult> configuredTaskAwaitable = (object) user != null ? this.UpdatePasswordHash(passwordStore, user, (string) null, false).ConfigureAwait(false) : throw new ArgumentNullException(nameof (user));
      IdentityResult identityResult = await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateUserAsync(user).ConfigureAwait(false);
      return await configuredTaskAwaitable;
    }

    /// <summary>
    /// Returns a <see cref="T:Microsoft.AspNetCore.Identity.PasswordVerificationResult" /> indicating the result of a password hash comparison.
    /// </summary>
    /// <param name="store">The store containing a user's password.</param>
    /// <param name="user">The user whose password should be verified.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.PasswordVerificationResult" />
    /// of the operation.
    /// </returns>
    protected virtual async Task<PasswordVerificationResult> VerifyPasswordAsync(
      IUserPasswordStore<TUser> store,
      TUser user,
      string password)
    {
      string hashedPassword = await store.GetPasswordHashAsync(user, this.CancellationToken).ConfigureAwait(false);
      return hashedPassword != null ? this.PasswordHasher.VerifyHashedPassword(user, hashedPassword, password) : PasswordVerificationResult.Failed;
    }

    /// <summary>
    /// Get the security stamp for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose security stamp should be set.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user" />.</returns>
    public virtual async Task<string> GetSecurityStampAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserSecurityStampStore<TUser> securityStore = this.GetSecurityStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      string securityStampAsync = await securityStore.GetSecurityStampAsync(user1, cancellationToken).ConfigureAwait(false);
      if (securityStampAsync == null)
      {
        this.Logger.LogDebug(LoggerEventIds.GetSecurityStampFailed, "GetSecurityStampAsync for user failed because stamp was null.");
        throw new InvalidOperationException(Resources.NullSecurityStamp);
      }
      return securityStampAsync;
    }

    /// <summary>
    /// Regenerates the security stamp for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose security stamp should be regenerated.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    /// <remarks>
    /// Regenerating a security stamp will sign out any saved login for the user.
    /// </remarks>
    public virtual async Task<IdentityResult> UpdateSecurityStampAsync(TUser user)
    {
      this.ThrowIfDisposed();
      this.GetSecurityStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      await this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Generates a password reset token for the specified <paramref name="user" />, using
    /// the configured password reset token provider.
    /// </summary>
    /// <param name="user">The user to generate a password reset token for.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation,
    /// containing a password reset token for the specified <paramref name="user" />.</returns>
    public virtual Task<string> GeneratePasswordResetTokenAsync(TUser user)
    {
      this.ThrowIfDisposed();
      return this.GenerateUserTokenAsync(user, this.Options.Tokens.PasswordResetTokenProvider, "ResetPassword");
    }

    /// <summary>
    /// Resets the <paramref name="user" />'s password to the specified <paramref name="newPassword" /> after
    /// validating the given password reset <paramref name="token" />.
    /// </summary>
    /// <param name="user">The user whose password should be reset.</param>
    /// <param name="token">The password reset token to verify.</param>
    /// <param name="newPassword">The new password to set if reset token verification succeeds.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> ResetPasswordAsync(
      TUser user,
      string token,
      string newPassword)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!await this.VerifyUserTokenAsync(user, this.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token).ConfigureAwait(false))
        return IdentityResult.Failed(this.ErrorDescriber.InvalidToken());
      IdentityResult identityResult = await this.UpdatePasswordHash(user, newPassword, true).ConfigureAwait(false);
      return !identityResult.Succeeded ? identityResult : await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves the user associated with the specified external login provider and login provider key.
    /// </summary>
    /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey" />.</param>
    /// <param name="providerKey">The key provided by the <paramref name="loginProvider" /> to identify a user.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
    /// </returns>
    public virtual Task<TUser?> FindByLoginAsync(string loginProvider, string providerKey)
    {
      this.ThrowIfDisposed();
      IUserLoginStore<TUser> loginStore = this.GetLoginStore();
      if (loginProvider == null)
        throw new ArgumentNullException(nameof (loginProvider));
      if (providerKey == null)
        throw new ArgumentNullException(nameof (providerKey));
      string loginProvider1 = loginProvider;
      string providerKey1 = providerKey;
      CancellationToken cancellationToken = this.CancellationToken;
      return loginStore.FindByLoginAsync(loginProvider1, providerKey1, cancellationToken);
    }

    /// <summary>
    /// Attempts to remove the provided external login information from the specified <paramref name="user" />.
    /// and returns a flag indicating whether the removal succeed or not.
    /// </summary>
    /// <param name="user">The user to remove the login information from.</param>
    /// <param name="loginProvider">The login provide whose information should be removed.</param>
    /// <param name="providerKey">The key given by the external login provider for the specified user.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> RemoveLoginAsync(
      TUser user,
      string loginProvider,
      string providerKey)
    {
      this.ThrowIfDisposed();
      IUserLoginStore<TUser> loginStore = this.GetLoginStore();
      if (loginProvider == null)
        throw new ArgumentNullException(nameof (loginProvider));
      if (providerKey == null)
        throw new ArgumentNullException(nameof (providerKey));
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      string loginProvider1 = loginProvider;
      string providerKey1 = providerKey;
      CancellationToken cancellationToken = this.CancellationToken;
      await loginStore.RemoveLoginAsync(user1, loginProvider1, providerKey1, cancellationToken).ConfigureAwait(false);
      await this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds an external <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> to the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user to add the login to.</param>
    /// <param name="login">The external <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> to add to the specified <paramref name="user" />.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login)
    {
      this.ThrowIfDisposed();
      IUserLoginStore<TUser> loginStore = this.GetLoginStore();
      if (login == null)
        throw new ArgumentNullException(nameof (login));
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if ((object) await this.FindByLoginAsync(login.LoginProvider, login.ProviderKey).ConfigureAwait(false) != null)
      {
        this.Logger.LogDebug(LoggerEventIds.AddLoginFailed, "AddLogin for user failed because it was already associated with another user.");
        return IdentityResult.Failed(this.ErrorDescriber.LoginAlreadyAssociated());
      }
      await loginStore.AddLoginAsync(user, login, this.CancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves the associated logins for the specified <param ref="user" />.
    /// </summary>
    /// <param name="user">The user whose associated logins to retrieve.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> for the asynchronous operation, containing a list of <see cref="T:Microsoft.AspNetCore.Identity.UserLoginInfo" /> for the specified <paramref name="user" />, if any.
    /// </returns>
    public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserLoginStore<TUser> loginStore = this.GetLoginStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await loginStore.GetLoginsAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds the specified <paramref name="claim" /> to the <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user to add the claim to.</param>
    /// <param name="claim">The claim to add.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual Task<IdentityResult> AddClaimAsync(TUser user, Claim claim)
    {
      this.ThrowIfDisposed();
      this.GetClaimStore();
      if (claim == null)
        throw new ArgumentNullException(nameof (claim));
      return (object) user != null ? this.AddClaimsAsync(user, (IEnumerable<Claim>) new Claim[1]
      {
        claim
      }) : throw new ArgumentNullException(nameof (user));
    }

    /// <summary>
    /// Adds the specified <paramref name="claims" /> to the <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user to add the claim to.</param>
    /// <param name="claims">The claims to add.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> AddClaimsAsync(TUser user, IEnumerable<Claim> claims)
    {
      this.ThrowIfDisposed();
      IUserClaimStore<TUser> claimStore = this.GetClaimStore();
      if (claims == null)
        throw new ArgumentNullException(nameof (claims));
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      IEnumerable<Claim> claims1 = claims;
      CancellationToken cancellationToken = this.CancellationToken;
      await claimStore.AddClaimsAsync(user1, claims1, cancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Replaces the given <paramref name="claim" /> on the specified <paramref name="user" /> with the <paramref name="newClaim" />
    /// </summary>
    /// <param name="user">The user to replace the claim on.</param>
    /// <param name="claim">The claim to replace.</param>
    /// <param name="newClaim">The new claim to replace the existing <paramref name="claim" /> with.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> ReplaceClaimAsync(
      TUser user,
      Claim claim,
      Claim newClaim)
    {
      this.ThrowIfDisposed();
      IUserClaimStore<TUser> claimStore = this.GetClaimStore();
      if (claim == null)
        throw new ArgumentNullException(nameof (claim));
      if (newClaim == null)
        throw new ArgumentNullException(nameof (newClaim));
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      Claim claim1 = claim;
      Claim newClaim1 = newClaim;
      CancellationToken cancellationToken = this.CancellationToken;
      await claimStore.ReplaceClaimAsync(user1, claim1, newClaim1, cancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the specified <paramref name="claim" /> from the given <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user to remove the specified <paramref name="claim" /> from.</param>
    /// <param name="claim">The <see cref="T:System.Security.Claims.Claim" /> to remove.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual Task<IdentityResult> RemoveClaimAsync(TUser user, Claim claim)
    {
      this.ThrowIfDisposed();
      this.GetClaimStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      return claim != null ? this.RemoveClaimsAsync(user, (IEnumerable<Claim>) new Claim[1]
      {
        claim
      }) : throw new ArgumentNullException(nameof (claim));
    }

    /// <summary>
    /// Removes the specified <paramref name="claims" /> from the given <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user to remove the specified <paramref name="claims" /> from.</param>
    /// <param name="claims">A collection of <see cref="T:System.Security.Claims.Claim" />s to remove.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> RemoveClaimsAsync(
      TUser user,
      IEnumerable<Claim> claims)
    {
      this.ThrowIfDisposed();
      IUserClaimStore<TUser> claimStore = this.GetClaimStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (claims == null)
        throw new ArgumentNullException(nameof (claims));
      TUser user1 = user;
      IEnumerable<Claim> claims1 = claims;
      CancellationToken cancellationToken = this.CancellationToken;
      await claimStore.RemoveClaimsAsync(user1, claims1, cancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a list of <see cref="T:System.Security.Claims.Claim" />s to be belonging to the specified <paramref name="user" /> as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose claims to retrieve.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a list of <see cref="T:System.Security.Claims.Claim" />s.
    /// </returns>
    public virtual async Task<IList<Claim>> GetClaimsAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserClaimStore<TUser> claimStore = this.GetClaimStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await claimStore.GetClaimsAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Add the specified <paramref name="user" /> to the named role.
    /// </summary>
    /// <param name="user">The user to add to the named role.</param>
    /// <param name="role">The name of the role to add the user to.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> AddToRoleAsync(TUser user, string role)
    {
      this.ThrowIfDisposed();
      IUserRoleStore<TUser> userRoleStore = this.GetUserRoleStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      string normalizedRole = this.NormalizeName(role);
      if (await userRoleStore.IsInRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false))
        return this.UserAlreadyInRoleError(role);
      await userRoleStore.AddToRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Add the specified <paramref name="user" /> to the named roles.
    /// </summary>
    /// <param name="user">The user to add to the named roles.</param>
    /// <param name="roles">The name of the roles to add the user to.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> AddToRolesAsync(TUser user, IEnumerable<string> roles)
    {
      this.ThrowIfDisposed();
      IUserRoleStore<TUser> userRoleStore = this.GetUserRoleStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (roles == null)
        throw new ArgumentNullException(nameof (roles));
      foreach (string role in roles.Distinct<string>())
      {
        string normalizedRole = this.NormalizeName(role);
        if (await userRoleStore.IsInRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false))
          return this.UserAlreadyInRoleError(role);
        await userRoleStore.AddToRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false);
        normalizedRole = (string) null;
      }
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the specified <paramref name="user" /> from the named role.
    /// </summary>
    /// <param name="user">The user to remove from the named role.</param>
    /// <param name="role">The name of the role to remove the user from.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role)
    {
      this.ThrowIfDisposed();
      IUserRoleStore<TUser> userRoleStore = this.GetUserRoleStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      string normalizedRole = this.NormalizeName(role);
      if (!await userRoleStore.IsInRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false))
        return this.UserNotInRoleError(role);
      await userRoleStore.RemoveFromRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }


    #nullable disable
    private IdentityResult UserAlreadyInRoleError(string role)
    {
      this.Logger.LogDebug(LoggerEventIds.UserAlreadyInRole, "User is already in role {role}.", (object) role);
      return IdentityResult.Failed(this.ErrorDescriber.UserAlreadyInRole(role));
    }

    private IdentityResult UserNotInRoleError(string role)
    {
      this.Logger.LogDebug(LoggerEventIds.UserNotInRole, "User is not in role {role}.", (object) role);
      return IdentityResult.Failed(this.ErrorDescriber.UserNotInRole(role));
    }


    #nullable enable
    /// <summary>
    /// Removes the specified <paramref name="user" /> from the named roles.
    /// </summary>
    /// <param name="user">The user to remove from the named roles.</param>
    /// <param name="roles">The name of the roles to remove the user from.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> RemoveFromRolesAsync(
      TUser user,
      IEnumerable<string> roles)
    {
      this.ThrowIfDisposed();
      IUserRoleStore<TUser> userRoleStore = this.GetUserRoleStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (roles == null)
        throw new ArgumentNullException(nameof (roles));
      foreach (string role in roles)
      {
        string normalizedRole = this.NormalizeName(role);
        if (!await userRoleStore.IsInRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false))
          return this.UserNotInRoleError(role);
        await userRoleStore.RemoveFromRoleAsync(user, normalizedRole, this.CancellationToken).ConfigureAwait(false);
        normalizedRole = (string) null;
      }
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a list of role names the specified <paramref name="user" /> belongs to.
    /// </summary>
    /// <param name="user">The user whose role names to retrieve.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a list of role names.</returns>
    public virtual async Task<IList<string>> GetRolesAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserRoleStore<TUser> userRoleStore = this.GetUserRoleStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await userRoleStore.GetRolesAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="user" /> is a member of the given named role.
    /// </summary>
    /// <param name="user">The user whose role membership should be checked.</param>
    /// <param name="role">The name of the role to be checked.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing a flag indicating whether the specified <paramref name="user" /> is
    /// a member of the named role.
    /// </returns>
    public virtual async Task<bool> IsInRoleAsync(TUser user, string role)
    {
      this.ThrowIfDisposed();
      IUserRoleStore<TUser> userRoleStore = this.GetUserRoleStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      string roleName = this.NormalizeName(role);
      CancellationToken cancellationToken = this.CancellationToken;
      return await userRoleStore.IsInRoleAsync(user1, roleName, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the email address for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose email should be returned.</param>
    /// <returns>The task object containing the results of the asynchronous operation, the email address for the specified <paramref name="user" />.</returns>
    public virtual async Task<string?> GetEmailAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserEmailStore<TUser> emailStore = this.GetEmailStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await emailStore.GetEmailAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the <paramref name="email" /> address for a <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose email should be set.</param>
    /// <param name="email">The email to set.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> SetEmailAsync(TUser user, string? email)
    {
      this.ThrowIfDisposed();
      IUserEmailStore<TUser> store = this.GetEmailStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      await store.SetEmailAsync(user, email, this.CancellationToken).ConfigureAwait(false);
      ConfiguredTaskAwaitable configuredTaskAwaitable = store.SetEmailConfirmedAsync(user, false, this.CancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      await configuredTaskAwaitable;
      IdentityResult identityResult = await this.UpdateUserAsync(user).ConfigureAwait(false);
      store = (IUserEmailStore<TUser>) null;
      return identityResult;
    }

    /// <summary>
    /// Gets the user, if any, associated with the normalized value of the specified email address.
    /// Note: Its recommended that identityOptions.User.RequireUniqueEmail be set to true when using this method, otherwise
    /// the store may throw if there are users with duplicate emails.
    /// </summary>
    /// <param name="email">The email address to return the user for.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous lookup operation, the user, if any, associated with a normalized value of the specified email address.
    /// </returns>
    public virtual async Task<TUser?> FindByEmailAsync(string email)
    {
      this.ThrowIfDisposed();
      IUserEmailStore<TUser> store = this.GetEmailStore();
      email = email != null ? this.NormalizeEmail(email) : throw new ArgumentNullException(nameof (email));
      ConfiguredTaskAwaitable<TUser> configuredTaskAwaitable = store.FindByEmailAsync(email, this.CancellationToken).ConfigureAwait(false);
      TUser byEmailAsync = await configuredTaskAwaitable;
      if ((object) byEmailAsync == null && this.Options.Stores.ProtectPersonalData)
      {
        ILookupProtectorKeyRing service = this._services.GetService<ILookupProtectorKeyRing>();
        ILookupProtector protector = this._services.GetService<ILookupProtector>();
        if (service != null && protector != null)
        {
          foreach (string allKeyId in service.GetAllKeyIds())
          {
            configuredTaskAwaitable = store.FindByEmailAsync(protector.Protect(allKeyId, email), this.CancellationToken).ConfigureAwait(false);
            byEmailAsync = await configuredTaskAwaitable;
            if ((object) byEmailAsync != null)
              return byEmailAsync;
          }
        }
        protector = (ILookupProtector) null;
      }
      return byEmailAsync;
    }

    /// <summary>
    /// Updates the normalized email for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose email address should be normalized and updated.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public virtual async Task UpdateNormalizedEmailAsync(TUser user)
    {
      IUserEmailStore<TUser> store = this.GetOptionalEmailStore();
      if (store == null)
      {
        store = (IUserEmailStore<TUser>) null;
      }
      else
      {
        string email = await this.GetEmailAsync(user).ConfigureAwait(false);
        await store.SetNormalizedEmailAsync(user, this.ProtectPersonalData(this.NormalizeEmail(email)), this.CancellationToken).ConfigureAwait(false);
        store = (IUserEmailStore<TUser>) null;
      }
    }

    /// <summary>
    /// Generates an email confirmation token for the specified user.
    /// </summary>
    /// <param name="user">The user to generate an email confirmation token for.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, an email confirmation token.
    /// </returns>
    public virtual Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
    {
      this.ThrowIfDisposed();
      return this.GenerateUserTokenAsync(user, this.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation");
    }

    /// <summary>
    /// Validates that an email confirmation token matches the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user to validate the token against.</param>
    /// <param name="token">The email confirmation token to validate.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> ConfirmEmailAsync(TUser user, string token)
    {
      this.ThrowIfDisposed();
      IUserEmailStore<TUser> store = this.GetEmailStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!await this.VerifyUserTokenAsync(user, this.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation", token).ConfigureAwait(false))
        return IdentityResult.Failed(this.ErrorDescriber.InvalidToken());
      await store.SetEmailConfirmedAsync(user, true, this.CancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a flag indicating whether the email address for the specified <paramref name="user" /> has been verified, true if the email address is verified otherwise
    /// false.
    /// </summary>
    /// <param name="user">The user whose email confirmation status should be returned.</param>
    /// <returns>
    /// The task object containing the results of the asynchronous operation, a flag indicating whether the email address for the specified <paramref name="user" />
    /// has been confirmed or not.
    /// </returns>
    public virtual async Task<bool> IsEmailConfirmedAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserEmailStore<TUser> emailStore = this.GetEmailStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await emailStore.GetEmailConfirmedAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Generates an email change token for the specified user.</summary>
    /// <param name="user">The user to generate an email change token for.</param>
    /// <param name="newEmail">The new email address.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, an email change token.
    /// </returns>
    public virtual Task<string> GenerateChangeEmailTokenAsync(TUser user, string newEmail)
    {
      this.ThrowIfDisposed();
      return this.GenerateUserTokenAsync(user, this.Options.Tokens.ChangeEmailTokenProvider, UserManager<TUser>.GetChangeEmailTokenPurpose(newEmail));
    }

    /// <summary>
    /// Updates a users emails if the specified email change <paramref name="token" /> is valid for the user.
    /// </summary>
    /// <param name="user">The user whose email should be updated.</param>
    /// <param name="newEmail">The new email address.</param>
    /// <param name="token">The change email token to be verified.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> ChangeEmailAsync(
      TUser user,
      string newEmail,
      string token)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!await this.VerifyUserTokenAsync(user, this.Options.Tokens.ChangeEmailTokenProvider, UserManager<TUser>.GetChangeEmailTokenPurpose(newEmail), token).ConfigureAwait(false))
        return IdentityResult.Failed(this.ErrorDescriber.InvalidToken());
      IUserEmailStore<TUser> store = this.GetEmailStore();
      ConfiguredTaskAwaitable configuredTaskAwaitable = store.SetEmailAsync(user, newEmail, this.CancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = store.SetEmailConfirmedAsync(user, true, this.CancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      await configuredTaskAwaitable;
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the telephone number, if any, for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose telephone number should be retrieved.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the user's telephone number, if any.</returns>
    public virtual async Task<string?> GetPhoneNumberAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserPhoneNumberStore<TUser> phoneNumberStore = this.GetPhoneNumberStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await phoneNumberStore.GetPhoneNumberAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets the phone number for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose phone number to set.</param>
    /// <param name="phoneNumber">The phone number to set.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> SetPhoneNumberAsync(TUser user, string? phoneNumber)
    {
      this.ThrowIfDisposed();
      IUserPhoneNumberStore<TUser> store = this.GetPhoneNumberStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      await store.SetPhoneNumberAsync(user, phoneNumber, this.CancellationToken).ConfigureAwait(false);
      ConfiguredTaskAwaitable configuredTaskAwaitable = store.SetPhoneNumberConfirmedAsync(user, false, this.CancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      await configuredTaskAwaitable;
      IdentityResult identityResult = await this.UpdateUserAsync(user).ConfigureAwait(false);
      store = (IUserPhoneNumberStore<TUser>) null;
      return identityResult;
    }

    /// <summary>
    /// Sets the phone number for the specified <paramref name="user" /> if the specified
    /// change <paramref name="token" /> is valid.
    /// </summary>
    /// <param name="user">The user whose phone number to set.</param>
    /// <param name="phoneNumber">The phone number to set.</param>
    /// <param name="token">The phone number confirmation token to validate.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> ChangePhoneNumberAsync(
      TUser user,
      string phoneNumber,
      string token)
    {
      this.ThrowIfDisposed();
      IUserPhoneNumberStore<TUser> store = this.GetPhoneNumberStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!await this.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber).ConfigureAwait(false))
      {
        this.Logger.LogDebug(LoggerEventIds.PhoneNumberChanged, "Change phone number for user failed with invalid token.");
        return IdentityResult.Failed(this.ErrorDescriber.InvalidToken());
      }
      await store.SetPhoneNumberAsync(user, phoneNumber, this.CancellationToken).ConfigureAwait(false);
      ConfiguredTaskAwaitable configuredTaskAwaitable = store.SetPhoneNumberConfirmedAsync(user, true, this.CancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      await configuredTaskAwaitable;
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a flag indicating whether the specified <paramref name="user" />'s telephone number has been confirmed.
    /// </summary>
    /// <param name="user">The user to return a flag for, indicating whether their telephone number is confirmed.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning true if the specified <paramref name="user" /> has a confirmed
    /// telephone number otherwise false.
    /// </returns>
    public virtual Task<bool> IsPhoneNumberConfirmedAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserPhoneNumberStore<TUser> phoneNumberStore = this.GetPhoneNumberStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return phoneNumberStore.GetPhoneNumberConfirmedAsync(user1, cancellationToken);
    }

    /// <summary>
    /// Generates a telephone number change token for the specified user.
    /// </summary>
    /// <param name="user">The user to generate a telephone number token for.</param>
    /// <param name="phoneNumber">The new phone number the validation token should be sent to.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the telephone change number token.
    /// </returns>
    public virtual Task<string> GenerateChangePhoneNumberTokenAsync(TUser user, string phoneNumber)
    {
      this.ThrowIfDisposed();
      return this.GenerateUserTokenAsync(user, this.Options.Tokens.ChangePhoneNumberTokenProvider, "ChangePhoneNumber:" + phoneNumber);
    }

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="user" />'s phone number change verification
    /// token is valid for the given <paramref name="phoneNumber" />.
    /// </summary>
    /// <param name="user">The user to validate the token against.</param>
    /// <param name="token">The telephone number change token to validate.</param>
    /// <param name="phoneNumber">The telephone number the token was generated for.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning true if the <paramref name="token" />
    /// is valid, otherwise false.
    /// </returns>
    public virtual Task<bool> VerifyChangePhoneNumberTokenAsync(
      TUser user,
      string token,
      string phoneNumber)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      return this.VerifyUserTokenAsync(user, this.Options.Tokens.ChangePhoneNumberTokenProvider, "ChangePhoneNumber:" + phoneNumber, token);
    }

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="token" /> is valid for
    /// the given <paramref name="user" /> and <paramref name="purpose" />.
    /// </summary>
    /// <param name="user">The user to validate the token against.</param>
    /// <param name="tokenProvider">The token provider used to generate the token.</param>
    /// <param name="purpose">The purpose the token should be generated for.</param>
    /// <param name="token">The token to validate</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, returning true if the <paramref name="token" />
    /// is valid, otherwise false.
    /// </returns>
    public virtual async Task<bool> VerifyUserTokenAsync(
      TUser user,
      string tokenProvider,
      string purpose,
      string token)
    {
      UserManager<TUser> manager = this;
      manager.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (tokenProvider == null)
        throw new ArgumentNullException(nameof (tokenProvider));
      if (!manager._tokenProviders.ContainsKey(tokenProvider))
        throw new NotSupportedException(Resources.FormatNoTokenProvider((object) nameof (TUser), (object) tokenProvider));
      bool flag = await manager._tokenProviders[tokenProvider].ValidateAsync(purpose, token, manager, user).ConfigureAwait(false);
      if (!flag)
        manager.Logger.LogDebug(LoggerEventIds.VerifyUserTokenFailed, "VerifyUserTokenAsync() failed with purpose: {purpose} for user.", (object) purpose);
      return flag;
    }

    /// <summary>
    /// Generates a token for the given <paramref name="user" /> and <paramref name="purpose" />.
    /// </summary>
    /// <param name="purpose">The purpose the token will be for.</param>
    /// <param name="user">The user the token will be for.</param>
    /// <param name="tokenProvider">The provider which will generate the token.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents result of the asynchronous operation, a token for
    /// the given user and purpose.
    /// </returns>
    public virtual Task<string> GenerateUserTokenAsync(
      TUser user,
      string tokenProvider,
      string purpose)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (tokenProvider == null)
        throw new ArgumentNullException(nameof (tokenProvider));
      if (!this._tokenProviders.ContainsKey(tokenProvider))
        throw new NotSupportedException(Resources.FormatNoTokenProvider((object) nameof (TUser), (object) tokenProvider));
      return this._tokenProviders[tokenProvider].GenerateAsync(purpose, this, user);
    }

    /// <summary>Registers a token provider.</summary>
    /// <param name="providerName">The name of the provider to register.</param>
    /// <param name="provider">The provider to register.</param>
    public virtual void RegisterTokenProvider(
      string providerName,
      IUserTwoFactorTokenProvider<TUser> provider)
    {
      this.ThrowIfDisposed();
      this._tokenProviders[providerName] = provider != null ? provider : throw new ArgumentNullException(nameof (provider));
    }

    /// <summary>
    /// Gets a list of valid two factor token providers for the specified <paramref name="user" />,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user the whose two factor authentication providers will be returned.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents result of the asynchronous operation, a list of two
    /// factor authentication providers for the specified user.
    /// </returns>
    public virtual async Task<IList<string>> GetValidTwoFactorProvidersAsync(TUser user)
    {
      UserManager<TUser> manager = this;
      manager.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      List<string> results = new List<string>();
      foreach (KeyValuePair<string, IUserTwoFactorTokenProvider<TUser>> tokenProvider in manager._tokenProviders)
      {
        KeyValuePair<string, IUserTwoFactorTokenProvider<TUser>> f = tokenProvider;
        if (await f.Value.CanGenerateTwoFactorTokenAsync(manager, user).ConfigureAwait(false))
          results.Add(f.Key);
        f = new KeyValuePair<string, IUserTwoFactorTokenProvider<TUser>>();
      }
      IList<string> factorProvidersAsync = (IList<string>) results;
      results = (List<string>) null;
      return factorProvidersAsync;
    }

    /// <summary>
    /// Verifies the specified two factor authentication <paramref name="token" /> against the <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user the token is supposed to be for.</param>
    /// <param name="tokenProvider">The provider which will verify the token.</param>
    /// <param name="token">The token to verify.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents result of the asynchronous operation, true if the token is valid,
    /// otherwise false.
    /// </returns>
    public virtual async Task<bool> VerifyTwoFactorTokenAsync(
      TUser user,
      string tokenProvider,
      string token)
    {
      UserManager<TUser> manager = this;
      manager.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!manager._tokenProviders.ContainsKey(tokenProvider))
        throw new NotSupportedException(Resources.FormatNoTokenProvider((object) nameof (TUser), (object) tokenProvider));
      int num = await manager._tokenProviders[tokenProvider].ValidateAsync("TwoFactor", token, manager, user).ConfigureAwait(false) ? 1 : 0;
      if (num == 0)
        manager.Logger.LogDebug(LoggerEventIds.VerifyTwoFactorTokenFailed, "VerifyTwoFactorTokenAsync() failed for user.");
      return num != 0;
    }

    /// <summary>
    /// Gets a two factor authentication token for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user the token is for.</param>
    /// <param name="tokenProvider">The provider which will generate the token.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents result of the asynchronous operation, a two factor authentication token
    /// for the user.
    /// </returns>
    public virtual Task<string> GenerateTwoFactorTokenAsync(TUser user, string tokenProvider)
    {
      this.ThrowIfDisposed();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!this._tokenProviders.ContainsKey(tokenProvider))
        throw new NotSupportedException(Resources.FormatNoTokenProvider((object) nameof (TUser), (object) tokenProvider));
      return this._tokenProviders[tokenProvider].GenerateAsync("TwoFactor", this, user);
    }

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled or not,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose two factor authentication enabled status should be retrieved.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, true if the specified <paramref name="user " />
    /// has two factor authentication enabled, otherwise false.
    /// </returns>
    public virtual async Task<bool> GetTwoFactorEnabledAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserTwoFactorStore<TUser> userTwoFactorStore = this.GetUserTwoFactorStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await userTwoFactorStore.GetTwoFactorEnabledAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sets a flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled or not,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
    /// <param name="enabled">A flag indicating whether the specified <paramref name="user" /> has two factor authentication enabled.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the operation
    /// </returns>
    public virtual async Task<IdentityResult> SetTwoFactorEnabledAsync(TUser user, bool enabled)
    {
      this.ThrowIfDisposed();
      IUserTwoFactorStore<TUser> userTwoFactorStore = this.GetUserTwoFactorStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      int num = enabled ? 1 : 0;
      CancellationToken cancellationToken = this.CancellationToken;
      ConfiguredTaskAwaitable configuredTaskAwaitable = userTwoFactorStore.SetTwoFactorEnabledAsync(user1, num != 0, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      await configuredTaskAwaitable;
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="user" /> is locked out,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose locked out status should be retrieved.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, true if the specified <paramref name="user " />
    /// is locked out, otherwise false.
    /// </returns>
    public virtual async Task<bool> IsLockedOutAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> store = this.GetUserLockoutStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!await store.GetLockoutEnabledAsync(user, this.CancellationToken).ConfigureAwait(false))
        return false;
      DateTimeOffset? nullable = await store.GetLockoutEndDateAsync(user, this.CancellationToken).ConfigureAwait(false);
      DateTimeOffset utcNow = DateTimeOffset.UtcNow;
      return nullable.HasValue && nullable.GetValueOrDefault() >= utcNow;
    }

    /// <summary>
    /// Sets a flag indicating whether the specified <paramref name="user" /> is locked out,
    /// as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user whose locked out status should be set.</param>
    /// <param name="enabled">Flag indicating whether the user is locked out or not.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the operation
    /// </returns>
    public virtual async Task<IdentityResult> SetLockoutEnabledAsync(TUser user, bool enabled)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> userLockoutStore = this.GetUserLockoutStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      int num = enabled ? 1 : 0;
      CancellationToken cancellationToken = this.CancellationToken;
      await userLockoutStore.SetLockoutEnabledAsync(user1, num != 0, cancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a flag indicating whether user lockout can be enabled for the specified user.
    /// </summary>
    /// <param name="user">The user whose ability to be locked out should be returned.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
    /// </returns>
    public virtual async Task<bool> GetLockoutEnabledAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> userLockoutStore = this.GetUserLockoutStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await userLockoutStore.GetLockoutEnabledAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the last <see cref="T:System.DateTimeOffset" /> a user's last lockout expired, if any.
    /// A time value in the past indicates a user is not currently locked out.
    /// </summary>
    /// <param name="user">The user whose lockout date should be retrieved.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the lookup, a <see cref="T:System.DateTimeOffset" /> containing the last time a user's lockout expired, if any.
    /// </returns>
    public virtual async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> userLockoutStore = this.GetUserLockoutStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await userLockoutStore.GetLockoutEndDateAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
    /// </summary>
    /// <param name="user">The user whose lockout date should be set.</param>
    /// <param name="lockoutEnd">The <see cref="T:System.DateTimeOffset" /> after which the <paramref name="user" />'s lockout should end.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the operation.</returns>
    public virtual async Task<IdentityResult> SetLockoutEndDateAsync(
      TUser user,
      DateTimeOffset? lockoutEnd)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> store = this.GetUserLockoutStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (!await store.GetLockoutEnabledAsync(user, this.CancellationToken).ConfigureAwait(false))
      {
        this.Logger.LogDebug(LoggerEventIds.LockoutFailed, "Lockout for user failed because lockout is not enabled for this user.");
        return IdentityResult.Failed(this.ErrorDescriber.UserLockoutNotEnabled());
      }
      await store.SetLockoutEndDateAsync(user, lockoutEnd, this.CancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Increments the access failed count for the user as an asynchronous operation.
    /// If the failed access account is greater than or equal to the configured maximum number of attempts,
    /// the user will be locked out for the configured lockout time span.
    /// </summary>
    /// <param name="user">The user whose failed access count to increment.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the operation.</returns>
    public virtual async Task<IdentityResult> AccessFailedAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> store = this.GetUserLockoutStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (await store.IncrementAccessFailedCountAsync(user, this.CancellationToken).ConfigureAwait(false) < this.Options.Lockout.MaxFailedAccessAttempts)
        return await this.UpdateUserAsync(user).ConfigureAwait(false);
      this.Logger.LogDebug(LoggerEventIds.UserLockedOut, "User is locked out.");
      await store.SetLockoutEndDateAsync(user, new DateTimeOffset?(DateTimeOffset.UtcNow.Add(this.Options.Lockout.DefaultLockoutTimeSpan)), this.CancellationToken).ConfigureAwait(false);
      await store.ResetAccessFailedCountAsync(user, this.CancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Resets the access failed count for the specified <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose failed access count should be reset.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> of the operation.</returns>
    public virtual async Task<IdentityResult> ResetAccessFailedCountAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> store = this.GetUserLockoutStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (await this.GetAccessFailedCountAsync(user).ConfigureAwait(false) == 0)
        return IdentityResult.Success;
      await store.ResetAccessFailedCountAsync(user, this.CancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves the current number of failed accesses for the given <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose access failed count should be retrieved for.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that contains the result the asynchronous operation, the current failed access count
    /// for the user.</returns>
    public virtual async Task<int> GetAccessFailedCountAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserLockoutStore<TUser> userLockoutStore = this.GetUserLockoutStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return await userLockoutStore.GetAccessFailedCountAsync(user1, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns a list of users from the user store who have the specified <paramref name="claim" />.
    /// </summary>
    /// <param name="claim">The claim to look for.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a list of <typeparamref name="TUser" />s who
    /// have the specified claim.
    /// </returns>
    public virtual Task<IList<TUser>> GetUsersForClaimAsync(Claim claim)
    {
      this.ThrowIfDisposed();
      IUserClaimStore<TUser> claimStore = this.GetClaimStore();
      Claim claim1 = claim != null ? claim : throw new ArgumentNullException(nameof (claim));
      CancellationToken cancellationToken = this.CancellationToken;
      return claimStore.GetUsersForClaimAsync(claim1, cancellationToken);
    }

    /// <summary>
    /// Returns a list of users from the user store who are members of the specified <paramref name="roleName" />.
    /// </summary>
    /// <param name="roleName">The name of the role whose users should be returned.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task`1" /> that represents the result of the asynchronous query, a list of <typeparamref name="TUser" />s who
    /// are members of the specified role.
    /// </returns>
    public virtual Task<IList<TUser>> GetUsersInRoleAsync(string roleName)
    {
      this.ThrowIfDisposed();
      IUserRoleStore<TUser> userRoleStore = this.GetUserRoleStore();
      string roleName1 = roleName != null ? this.NormalizeName(roleName) : throw new ArgumentNullException(nameof (roleName));
      CancellationToken cancellationToken = this.CancellationToken;
      return userRoleStore.GetUsersInRoleAsync(roleName1, cancellationToken);
    }

    /// <summary>Returns an authentication token for a user.</summary>
    /// <param name="user"></param>
    /// <param name="loginProvider">The authentication scheme for the provider the token is associated with.</param>
    /// <param name="tokenName">The name of the token.</param>
    /// <returns>The authentication token for a user</returns>
    public virtual Task<string?> GetAuthenticationTokenAsync(
      TUser user,
      string loginProvider,
      string tokenName)
    {
      this.ThrowIfDisposed();
      IUserAuthenticationTokenStore<TUser> authenticationTokenStore = this.GetAuthenticationTokenStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (loginProvider == null)
        throw new ArgumentNullException(nameof (loginProvider));
      if (tokenName == null)
        throw new ArgumentNullException(nameof (tokenName));
      TUser user1 = user;
      string loginProvider1 = loginProvider;
      string name = tokenName;
      CancellationToken cancellationToken = this.CancellationToken;
      return authenticationTokenStore.GetTokenAsync(user1, loginProvider1, name, cancellationToken);
    }

    /// <summary>Sets an authentication token for a user.</summary>
    /// <param name="user"></param>
    /// <param name="loginProvider">The authentication scheme for the provider the token is associated with.</param>
    /// <param name="tokenName">The name of the token.</param>
    /// <param name="tokenValue">The value of the token.</param>
    /// <returns>Whether the user was successfully updated.</returns>
    public virtual async Task<IdentityResult> SetAuthenticationTokenAsync(
      TUser user,
      string loginProvider,
      string tokenName,
      string? tokenValue)
    {
      this.ThrowIfDisposed();
      IUserAuthenticationTokenStore<TUser> authenticationTokenStore = this.GetAuthenticationTokenStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (loginProvider == null)
        throw new ArgumentNullException(nameof (loginProvider));
      if (tokenName == null)
        throw new ArgumentNullException(nameof (tokenName));
      TUser user1 = user;
      string loginProvider1 = loginProvider;
      string name = tokenName;
      string str = tokenValue;
      CancellationToken cancellationToken = this.CancellationToken;
      await authenticationTokenStore.SetTokenAsync(user1, loginProvider1, name, str, cancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>Remove an authentication token for a user.</summary>
    /// <param name="user"></param>
    /// <param name="loginProvider">The authentication scheme for the provider the token is associated with.</param>
    /// <param name="tokenName">The name of the token.</param>
    /// <returns>Whether a token was removed.</returns>
    public virtual async Task<IdentityResult> RemoveAuthenticationTokenAsync(
      TUser user,
      string loginProvider,
      string tokenName)
    {
      this.ThrowIfDisposed();
      IUserAuthenticationTokenStore<TUser> authenticationTokenStore = this.GetAuthenticationTokenStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      if (loginProvider == null)
        throw new ArgumentNullException(nameof (loginProvider));
      if (tokenName == null)
        throw new ArgumentNullException(nameof (tokenName));
      TUser user1 = user;
      string loginProvider1 = loginProvider;
      string name = tokenName;
      CancellationToken cancellationToken = this.CancellationToken;
      await authenticationTokenStore.RemoveTokenAsync(user1, loginProvider1, name, cancellationToken).ConfigureAwait(false);
      return await this.UpdateUserAsync(user).ConfigureAwait(false);
    }

    /// <summary>Returns the authenticator key for the user.</summary>
    /// <param name="user">The user.</param>
    /// <returns>The authenticator key</returns>
    public virtual Task<string?> GetAuthenticatorKeyAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserAuthenticatorKeyStore<TUser> authenticatorKeyStore = this.GetAuthenticatorKeyStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return authenticatorKeyStore.GetAuthenticatorKeyAsync(user1, cancellationToken);
    }

    /// <summary>Resets the authenticator key for the user.</summary>
    /// <param name="user">The user.</param>
    /// <returns>Whether the user was successfully updated.</returns>
    public virtual async Task<IdentityResult> ResetAuthenticatorKeyAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserAuthenticatorKeyStore<TUser> authenticatorKeyStore = this.GetAuthenticatorKeyStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      string authenticatorKey = this.GenerateNewAuthenticatorKey();
      CancellationToken cancellationToken = this.CancellationToken;
      ConfiguredTaskAwaitable configuredTaskAwaitable = authenticatorKeyStore.SetAuthenticatorKeyAsync(user1, authenticatorKey, cancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      await configuredTaskAwaitable;
      return await this.UpdateAsync(user).ConfigureAwait(false);
    }

    /// <summary>
    /// Generates a new base32 encoded 160-bit security secret (size of SHA1 hash).
    /// </summary>
    /// <returns>The new security secret.</returns>
    public virtual string GenerateNewAuthenticatorKey() => UserManager<TUser>.NewSecurityStamp();

    /// <summary>
    /// Generates recovery codes for the user, this invalidates any previous recovery codes for the user.
    /// </summary>
    /// <param name="user">The user to generate recovery codes for.</param>
    /// <param name="number">The number of codes to generate.</param>
    /// <returns>The new recovery codes for the user.  Note: there may be less than number returned, as duplicates will be removed.</returns>
    public virtual async Task<IEnumerable<string>?> GenerateNewTwoFactorRecoveryCodesAsync(
      TUser user,
      int number)
    {
      this.ThrowIfDisposed();
      IUserTwoFactorRecoveryCodeStore<TUser> recoveryCodeStore = this.GetRecoveryCodeStore();
      if ((object) user == null)
        throw new ArgumentNullException(nameof (user));
      List<string> newCodes = new List<string>(number);
      for (int index = 0; index < number; ++index)
        newCodes.Add(this.CreateTwoFactorRecoveryCode());
      await recoveryCodeStore.ReplaceCodesAsync(user, newCodes.Distinct<string>(), this.CancellationToken).ConfigureAwait(false);
      IEnumerable<string> recoveryCodesAsync = !(await this.UpdateAsync(user).ConfigureAwait(false)).Succeeded ? (IEnumerable<string>) null : (IEnumerable<string>) newCodes;
      newCodes = (List<string>) null;
      return recoveryCodesAsync;
    }

    /// <summary>Generate a new recovery code.</summary>
    /// <returns></returns>
    protected virtual string CreateTwoFactorRecoveryCode()
    {
      StringBuilder stringBuilder = new StringBuilder(11);
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append('-');
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      stringBuilder.Append(UserManager<TUser>.GetRandomRecoveryCodeChar());
      return stringBuilder.ToString();
    }

    private static unsafe char GetRandomRecoveryCodeChar()
    {
      uint num1 = (uint) (UserManager<TUser>.AllowedChars.Length - 1);
      uint num2 = num1;
      uint num3 = num2 | num2 >> 1;
      uint num4 = num3 | num3 >> 2;
      uint num5 = num4 | num4 >> 4;
      uint num6 = num5 | num5 >> 8;
      uint num7 = num6 | num6 >> 16;
      // ISSUE: untyped stack allocation
      Span<uint> span = new Span<uint>((void*) __untypedstackalloc(new IntPtr(4)), 1);
      uint index;
      do
      {
        RandomNumberGenerator.Fill(MemoryMarshal.AsBytes<uint>(span));
        index = num7 & span[0];
      }
      while (index > num1);
      return UserManager<TUser>.AllowedChars[(int) index];
    }

    /// <summary>
    /// Returns whether a recovery code is valid for a user. Note: recovery codes are only valid
    /// once, and will be invalid after use.
    /// </summary>
    /// <param name="user">The user who owns the recovery code.</param>
    /// <param name="code">The recovery code to use.</param>
    /// <returns>True if the recovery code was found for the user.</returns>
    public virtual async Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(
      TUser user,
      string code)
    {
      this.ThrowIfDisposed();
      IUserTwoFactorRecoveryCodeStore<TUser> recoveryCodeStore = this.GetRecoveryCodeStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      string code1 = code;
      CancellationToken cancellationToken = this.CancellationToken;
      if (await recoveryCodeStore.RedeemCodeAsync(user1, code1, cancellationToken).ConfigureAwait(false))
        return await this.UpdateAsync(user).ConfigureAwait(false);
      return IdentityResult.Failed(this.ErrorDescriber.RecoveryCodeRedemptionFailed());
    }

    /// <summary>
    /// Returns how many recovery code are still valid for a user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>How many recovery code are still valid for a user.</returns>
    public virtual Task<int> CountRecoveryCodesAsync(TUser user)
    {
      this.ThrowIfDisposed();
      IUserTwoFactorRecoveryCodeStore<TUser> recoveryCodeStore = this.GetRecoveryCodeStore();
      TUser user1 = (object) user != null ? user : throw new ArgumentNullException(nameof (user));
      CancellationToken cancellationToken = this.CancellationToken;
      return recoveryCodeStore.CountCodesAsync(user1, cancellationToken);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._disposed)
        return;
      this.Store.Dispose();
      this._disposed = true;
    }


    #nullable disable
    private IUserTwoFactorStore<TUser> GetUserTwoFactorStore() => this.Store is IUserTwoFactorStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserTwoFactorStore);

    private IUserLockoutStore<TUser> GetUserLockoutStore() => this.Store is IUserLockoutStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserLockoutStore);

    private IUserEmailStore<TUser> GetEmailStore() => this.Store is IUserEmailStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserEmailStore);

    private IUserEmailStore<TUser> GetOptionalEmailStore() => this.Store as IUserEmailStore<TUser>;

    private IUserPhoneNumberStore<TUser> GetPhoneNumberStore() => this.Store is IUserPhoneNumberStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserPhoneNumberStore);


    #nullable enable
    /// <summary>
    /// Creates bytes to use as a security token from the user's security stamp.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The security token bytes.</returns>
    public virtual async Task<byte[]> CreateSecurityTokenAsync(TUser user)
    {
      Encoding unicode = Encoding.Unicode;
      return unicode.GetBytes(await this.GetSecurityStampAsync(user).ConfigureAwait(false));
    }


    #nullable disable
    private async Task UpdateSecurityStampInternal(TUser user)
    {
      if (!this.SupportsUserSecurityStamp)
        return;
      await this.GetSecurityStore().SetSecurityStampAsync(user, UserManager<TUser>.NewSecurityStamp(), this.CancellationToken).ConfigureAwait(false);
    }


    #nullable enable
    /// <summary>Updates a user's password hash.</summary>
    /// <param name="user">The user.</param>
    /// <param name="newPassword">The new password.</param>
    /// <param name="validatePassword">Whether to validate the password.</param>
    /// <returns>Whether the password has was successfully updated.</returns>
    protected virtual Task<IdentityResult> UpdatePasswordHash(
      TUser user,
      string newPassword,
      bool validatePassword)
    {
      return this.UpdatePasswordHash(this.GetPasswordStore(), user, newPassword, validatePassword);
    }


    #nullable disable
    private async Task<IdentityResult> UpdatePasswordHash(
      IUserPasswordStore<TUser> passwordStore,
      TUser user,
      string newPassword,
      bool validatePassword = true)
    {
      if (validatePassword)
      {
        IdentityResult identityResult = await this.ValidatePasswordAsync(user, newPassword).ConfigureAwait(false);
        if (!identityResult.Succeeded)
          return identityResult;
      }
      await passwordStore.SetPasswordHashAsync(user, newPassword != null ? this.PasswordHasher.HashPassword(user, newPassword) : (string) null, this.CancellationToken).ConfigureAwait(false);
      await this.UpdateSecurityStampInternal(user).ConfigureAwait(false);
      return IdentityResult.Success;
    }

    private IUserRoleStore<TUser> GetUserRoleStore() => this.Store is IUserRoleStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserRoleStore);

    private static string NewSecurityStamp()
    {
      byte[] numArray = new byte[20];
      RandomNumberGenerator.Fill((Span<byte>) numArray);
      return Base32.ToBase32(numArray);
    }

    private IUserLoginStore<TUser> GetLoginStore() => this.Store is IUserLoginStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserLoginStore);

    private IUserSecurityStampStore<TUser> GetSecurityStore() => this.Store is IUserSecurityStampStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserSecurityStampStore);

    private IUserClaimStore<TUser> GetClaimStore() => this.Store is IUserClaimStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserClaimStore);


    #nullable enable
    /// <summary>Generates the token purpose used to change email.</summary>
    /// <param name="newEmail">The new email address.</param>
    /// <returns>The token purpose.</returns>
    public static string GetChangeEmailTokenPurpose(string newEmail) => "ChangeEmail:" + newEmail;

    /// <summary>
    /// Should return <see cref="P:Microsoft.AspNetCore.Identity.IdentityResult.Success" /> if validation is successful. This is
    /// called before saving the user via Create or Update.
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> representing whether validation was successful.</returns>
    protected async Task<IdentityResult> ValidateUserAsync(TUser user)
    {
      UserManager<TUser> manager = this;
      if (manager.SupportsUserSecurityStamp)
      {
        if (await manager.GetSecurityStampAsync(user).ConfigureAwait(false) == null)
          throw new InvalidOperationException(Resources.NullSecurityStamp);
      }
      List<IdentityError> errors = new List<IdentityError>();
      foreach (IUserValidator<TUser> userValidator in (IEnumerable<IUserValidator<TUser>>) manager.UserValidators)
      {
        IdentityResult identityResult = await userValidator.ValidateAsync(manager, user).ConfigureAwait(false);
        if (!identityResult.Succeeded)
          errors.AddRange(identityResult.Errors);
      }
      if (errors.Count <= 0)
        return IdentityResult.Success;
      manager.Logger.LogDebug(LoggerEventIds.UserValidationFailed, "User validation failed: {errors}.", (object) string.Join(";", errors.Select<IdentityError, string>((Func<IdentityError, string>) (e => e.Code))));
      return IdentityResult.Failed(errors.ToArray());
    }

    /// <summary>
    /// Should return <see cref="P:Microsoft.AspNetCore.Identity.IdentityResult.Success" /> if validation is successful. This is
    /// called before updating the password hash.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="password">The password.</param>
    /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> representing whether validation was successful.</returns>
    protected async Task<IdentityResult> ValidatePasswordAsync(TUser user, string? password)
    {
      UserManager<TUser> manager = this;
      List<IdentityError> errors = new List<IdentityError>();
      bool isValid = true;
      foreach (IPasswordValidator<TUser> passwordValidator in (IEnumerable<IPasswordValidator<TUser>>) manager.PasswordValidators)
      {
        IdentityResult identityResult = await passwordValidator.ValidateAsync(manager, user, password).ConfigureAwait(false);
        if (!identityResult.Succeeded)
        {
          if (identityResult.Errors.Any<IdentityError>())
            errors.AddRange(identityResult.Errors);
          isValid = false;
        }
      }
      if (isValid)
        return IdentityResult.Success;
      manager.Logger.LogDebug(LoggerEventIds.PasswordValidationFailed, "User password validation failed: {errors}.", (object) string.Join(";", errors.Select<IdentityError, string>((Func<IdentityError, string>) (e => e.Code))));
      return IdentityResult.Failed(errors.ToArray());
    }

    /// <summary>
    /// Called to update the user after validating and updating the normalized email/user name.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>Whether the operation was successful.</returns>
    protected virtual async Task<IdentityResult> UpdateUserAsync(TUser user)
    {
      ConfiguredTaskAwaitable<IdentityResult> configuredTaskAwaitable1 = this.ValidateUserAsync(user).ConfigureAwait(false);
      IdentityResult identityResult = await configuredTaskAwaitable1;
      if (!identityResult.Succeeded)
        return identityResult;
      ConfiguredTaskAwaitable configuredTaskAwaitable2 = this.UpdateNormalizedUserNameAsync(user).ConfigureAwait(false);
      await configuredTaskAwaitable2;
      configuredTaskAwaitable2 = this.UpdateNormalizedEmailAsync(user).ConfigureAwait(false);
      await configuredTaskAwaitable2;
      configuredTaskAwaitable1 = this.Store.UpdateAsync(user, this.CancellationToken).ConfigureAwait(false);
      return await configuredTaskAwaitable1;
    }


    #nullable disable
    private IUserAuthenticatorKeyStore<TUser> GetAuthenticatorKeyStore() => this.Store is IUserAuthenticatorKeyStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserAuthenticatorKeyStore);

    private IUserTwoFactorRecoveryCodeStore<TUser> GetRecoveryCodeStore() => this.Store is IUserTwoFactorRecoveryCodeStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserTwoFactorRecoveryCodeStore);

    private IUserAuthenticationTokenStore<TUser> GetAuthenticationTokenStore() => this.Store is IUserAuthenticationTokenStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserAuthenticationTokenStore);

    private IUserPasswordStore<TUser> GetPasswordStore() => this.Store is IUserPasswordStore<TUser> store ? store : throw new NotSupportedException(Resources.StoreNotIUserPasswordStore);

    /// <summary>Throws if this class has been disposed.</summary>
    protected void ThrowIfDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().Name);
    }
  }
}
