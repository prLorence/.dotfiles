// Decompiled with JetBrains decompiler
// Type: Microsoft.AspNetCore.Identity.RoleManager`1
// Assembly: Microsoft.Extensions.Identity.Core, Version=7.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
// MVID: 92910454-F3D8-437F-BB56-BBBFE449C699
// Assembly location: /usr/lib/dotnet/shared/Microsoft.AspNetCore.App/7.0.9/Microsoft.Extensions.Identity.Core.dll
// XML documentation location: /usr/lib/dotnet/packs/Microsoft.AspNetCore.App.Ref/7.0.9/ref/net7.0/Microsoft.Extensions.Identity.Core.xml

using Microsoft.Extensions.Identity.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Microsoft.AspNetCore.Identity
{
  /// <summary>
  /// Provides the APIs for managing roles in a persistence store.
  /// </summary>
  /// <typeparam name="TRole">The type encapsulating a role.</typeparam>
  public class RoleManager<TRole> : IDisposable where TRole : class
  {
    private bool _disposed;

    /// <summary>The cancellation token used to cancel operations.</summary>
    protected virtual CancellationToken CancellationToken => CancellationToken.None;

    /// <summary>
    /// Constructs a new instance of <see cref="T:Microsoft.AspNetCore.Identity.RoleManager`1" />.
    /// </summary>
    /// <param name="store">The persistence store the manager will operate over.</param>
    /// <param name="roleValidators">A collection of validators for roles.</param>
    /// <param name="keyNormalizer">The normalizer to use when normalizing role names to keys.</param>
    /// <param name="errors">The <see cref="T:Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> used to provider error messages.</param>
    /// <param name="logger">The logger used to log messages, warnings and errors.</param>
    public RoleManager(
      IRoleStore<TRole> store,
      IEnumerable<IRoleValidator<TRole>> roleValidators,
      ILookupNormalizer keyNormalizer,
      IdentityErrorDescriber errors,
      ILogger<RoleManager<TRole>> logger)
    {
      this.Store = store != null ? store : throw new ArgumentNullException(nameof (store));
      this.KeyNormalizer = keyNormalizer;
      this.ErrorDescriber = errors;
      this.Logger = (ILogger) logger;
      if (roleValidators == null)
        return;
      foreach (IRoleValidator<TRole> roleValidator in roleValidators)
        this.RoleValidators.Add(roleValidator);
    }

    /// <summary>Gets the persistence store this instance operates over.</summary>
    /// <value>The persistence store this instance operates over.</value>
    protected IRoleStore<TRole> Store { get; private set; }

    /// <summary>
    /// Gets the <see cref="T:Microsoft.Extensions.Logging.ILogger" /> used to log messages from the manager.
    /// </summary>
    /// <value>
    /// The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> used to log messages from the manager.
    /// </value>
    public virtual ILogger Logger { get; set; }

    /// <summary>
    /// Gets a list of validators for roles to call before persistence.
    /// </summary>
    /// <value>A list of validators for roles to call before persistence.</value>
    public IList<IRoleValidator<TRole>> RoleValidators { get; } = (IList<IRoleValidator<TRole>>) new List<IRoleValidator<TRole>>();

    /// <summary>
    /// Gets the <see cref="T:Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> used to provider error messages.
    /// </summary>
    /// <value>
    /// The <see cref="T:Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> used to provider error messages.
    /// </value>
    public IdentityErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// Gets the normalizer to use when normalizing role names to keys.
    /// </summary>
    /// <value>The normalizer to use when normalizing role names to keys.</value>
    public ILookupNormalizer KeyNormalizer { get; set; }

    /// <summary>
    /// Gets an IQueryable collection of Roles if the persistence store is an <see cref="T:Microsoft.AspNetCore.Identity.IQueryableRoleStore`1" />,
    /// otherwise throws a <see cref="T:System.NotSupportedException" />.
    /// </summary>
    /// <value>An IQueryable collection of Roles if the persistence store is an <see cref="T:Microsoft.AspNetCore.Identity.IQueryableRoleStore`1" />.</value>
    /// <exception cref="T:System.NotSupportedException">Thrown if the persistence store is not an <see cref="T:Microsoft.AspNetCore.Identity.IQueryableRoleStore`1" />.</exception>
    /// <remarks>
    /// Callers to this property should use <see cref="P:Microsoft.AspNetCore.Identity.RoleManager`1.SupportsQueryableRoles" /> to ensure the backing role store supports
    /// returning an IQueryable list of roles.
    /// </remarks>
    public virtual IQueryable<TRole> Roles => this.Store is IQueryableRoleStore<TRole> store ? store.Roles : throw new NotSupportedException(Resources.StoreNotIQueryableRoleStore);

    /// <summary>
    /// Gets a flag indicating whether the underlying persistence store supports returning an <see cref="T:System.Linq.IQueryable" /> collection of roles.
    /// </summary>
    /// <value>
    /// true if the underlying persistence store supports returning an <see cref="T:System.Linq.IQueryable" /> collection of roles, otherwise false.
    /// </value>
    public virtual bool SupportsQueryableRoles
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IQueryableRoleStore<TRole>;
      }
    }

    /// <summary>
    /// Gets a flag indicating whether the underlying persistence store supports <see cref="T:System.Security.Claims.Claim" />s for roles.
    /// </summary>
    /// <value>
    /// true if the underlying persistence store supports <see cref="T:System.Security.Claims.Claim" />s for roles, otherwise false.
    /// </value>
    public virtual bool SupportsRoleClaims
    {
      get
      {
        this.ThrowIfDisposed();
        return this.Store is IRoleClaimStore<TRole>;
      }
    }

    /// <summary>
    /// Creates the specified <paramref name="role" /> in the persistence store.
    /// </summary>
    /// <param name="role">The role to create.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
    /// </returns>
    public virtual async Task<IdentityResult> CreateAsync(TRole role)
    {
      this.ThrowIfDisposed();
      ConfiguredTaskAwaitable<IdentityResult> configuredTaskAwaitable = (object) role != null ? this.ValidateRoleAsync(role).ConfigureAwait(false) : throw new ArgumentNullException(nameof (role));
      IdentityResult async = await configuredTaskAwaitable;
      if (!async.Succeeded)
        return async;
      await this.UpdateNormalizedRoleNameAsync(role).ConfigureAwait(false);
      configuredTaskAwaitable = this.Store.CreateAsync(role, this.CancellationToken).ConfigureAwait(false);
      return await configuredTaskAwaitable;
    }

    /// <summary>
    /// Updates the normalized name for the specified <paramref name="role" />.
    /// </summary>
    /// <param name="role">The role whose normalized name needs to be updated.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation.
    /// </returns>
    public virtual async Task UpdateNormalizedRoleNameAsync(TRole role)
    {
      string key = await this.GetRoleNameAsync(role).ConfigureAwait(false);
      await this.Store.SetNormalizedRoleNameAsync(role, this.NormalizeKey(key), this.CancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates the specified <paramref name="role" />.
    /// </summary>
    /// <param name="role">The role to updated.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> for the update.
    /// </returns>
    public virtual Task<IdentityResult> UpdateAsync(TRole role)
    {
      this.ThrowIfDisposed();
      return (object) role != null ? this.UpdateRoleAsync(role) : throw new ArgumentNullException(nameof (role));
    }

    /// <summary>
    /// Deletes the specified <paramref name="role" />.
    /// </summary>
    /// <param name="role">The role to delete.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> for the delete.
    /// </returns>
    public virtual Task<IdentityResult> DeleteAsync(TRole role)
    {
      this.ThrowIfDisposed();
      if ((object) role == null)
        throw new ArgumentNullException(nameof (role));
      return this.Store.DeleteAsync(role, this.CancellationToken);
    }

    /// <summary>
    /// Gets a flag indicating whether the specified <paramref name="roleName" /> exists.
    /// </summary>
    /// <param name="roleName">The role name whose existence should be checked.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing true if the role name exists, otherwise false.
    /// </returns>
    public virtual async Task<bool> RoleExistsAsync(string roleName)
    {
      this.ThrowIfDisposed();
      if (roleName == null)
        throw new ArgumentNullException(nameof (roleName));
      return (object) await this.FindByNameAsync(roleName).ConfigureAwait(false) != null;
    }

    /// <summary>
    /// Gets a normalized representation of the specified <paramref name="key" />.
    /// </summary>
    /// <param name="key">The value to normalize.</param>
    /// <returns>A normalized representation of the specified <paramref name="key" />.</returns>
    [return: NotNullIfNotNull("key")]
    public virtual string? NormalizeKey(string? key) => this.KeyNormalizer != null ? this.KeyNormalizer.NormalizeName(key) : key;

    /// <summary>
    /// Finds the role associated with the specified <paramref name="roleId" /> if any.
    /// </summary>
    /// <param name="roleId">The role ID whose role should be returned.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the role
    /// associated with the specified <paramref name="roleId" />
    /// </returns>
    public virtual Task<TRole?> FindByIdAsync(string roleId)
    {
      this.ThrowIfDisposed();
      return this.Store.FindByIdAsync(roleId, this.CancellationToken);
    }

    /// <summary>
    /// Gets the name of the specified <paramref name="role" />.
    /// </summary>
    /// <param name="role">The role whose name should be retrieved.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the name of the
    /// specified <paramref name="role" />.
    /// </returns>
    public virtual Task<string?> GetRoleNameAsync(TRole role)
    {
      this.ThrowIfDisposed();
      return this.Store.GetRoleNameAsync(role, this.CancellationToken);
    }

    /// <summary>
    /// Sets the name of the specified <paramref name="role" />.
    /// </summary>
    /// <param name="role">The role whose name should be set.</param>
    /// <param name="name">The name to set.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> SetRoleNameAsync(TRole role, string? name)
    {
      this.ThrowIfDisposed();
      ConfiguredTaskAwaitable configuredTaskAwaitable = this.Store.SetRoleNameAsync(role, name, this.CancellationToken).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.UpdateNormalizedRoleNameAsync(role).ConfigureAwait(false);
      await configuredTaskAwaitable;
      return IdentityResult.Success;
    }

    /// <summary>
    /// Gets the ID of the specified <paramref name="role" />.
    /// </summary>
    /// <param name="role">The role whose ID should be retrieved.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the ID of the
    /// specified <paramref name="role" />.
    /// </returns>
    public virtual Task<string> GetRoleIdAsync(TRole role)
    {
      this.ThrowIfDisposed();
      return this.Store.GetRoleIdAsync(role, this.CancellationToken);
    }

    /// <summary>
    /// Finds the role associated with the specified <paramref name="roleName" /> if any.
    /// </summary>
    /// <param name="roleName">The name of the role to be returned.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the role
    /// associated with the specified <paramref name="roleName" />
    /// </returns>
    public virtual Task<TRole?> FindByNameAsync(string roleName)
    {
      this.ThrowIfDisposed();
      if (roleName == null)
        throw new ArgumentNullException(nameof (roleName));
      return this.Store.FindByNameAsync(this.NormalizeKey(roleName), this.CancellationToken);
    }

    /// <summary>Adds a claim to a role.</summary>
    /// <param name="role">The role to add the claim to.</param>
    /// <param name="claim">The claim to add.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> AddClaimAsync(TRole role, Claim claim)
    {
      this.ThrowIfDisposed();
      IRoleClaimStore<TRole> claimStore = this.GetClaimStore();
      if (claim == null)
        throw new ArgumentNullException(nameof (claim));
      TRole role1 = (object) role != null ? role : throw new ArgumentNullException(nameof (role));
      Claim claim1 = claim;
      CancellationToken cancellationToken = this.CancellationToken;
      await claimStore.AddClaimAsync(role1, claim1, cancellationToken).ConfigureAwait(false);
      return await this.UpdateRoleAsync(role).ConfigureAwait(false);
    }

    /// <summary>Removes a claim from a role.</summary>
    /// <param name="role">The role to remove the claim from.</param>
    /// <param name="claim">The claim to remove.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" />
    /// of the operation.
    /// </returns>
    public virtual async Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim)
    {
      this.ThrowIfDisposed();
      IRoleClaimStore<TRole> claimStore = this.GetClaimStore();
      TRole role1 = (object) role != null ? role : throw new ArgumentNullException(nameof (role));
      Claim claim1 = claim;
      CancellationToken cancellationToken = this.CancellationToken;
      await claimStore.RemoveClaimAsync(role1, claim1, cancellationToken).ConfigureAwait(false);
      return await this.UpdateRoleAsync(role).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a list of claims associated with the specified <paramref name="role" />.
    /// </summary>
    /// <param name="role">The role whose claims should be returned.</param>
    /// <returns>
    /// The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the list of <see cref="T:System.Security.Claims.Claim" />s
    /// associated with the specified <paramref name="role" />.
    /// </returns>
    public virtual Task<IList<Claim>> GetClaimsAsync(TRole role)
    {
      this.ThrowIfDisposed();
      IRoleClaimStore<TRole> claimStore = this.GetClaimStore();
      TRole role1 = (object) role != null ? role : throw new ArgumentNullException(nameof (role));
      CancellationToken cancellationToken = this.CancellationToken;
      return claimStore.GetClaimsAsync(role1, cancellationToken);
    }

    /// <summary>Releases all resources used by the role manager.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing && !this._disposed)
        this.Store.Dispose();
      this._disposed = true;
    }

    /// <summary>
    /// Should return <see cref="P:Microsoft.AspNetCore.Identity.IdentityResult.Success" /> if validation is successful. This is
    /// called before saving the role via Create or Update.
    /// </summary>
    /// <param name="role">The role</param>
    /// <returns>A <see cref="T:Microsoft.AspNetCore.Identity.IdentityResult" /> representing whether validation was successful.</returns>
    protected virtual async Task<IdentityResult> ValidateRoleAsync(TRole role)
    {
      RoleManager<TRole> manager = this;
      List<IdentityError> errors = new List<IdentityError>();
      foreach (IRoleValidator<TRole> roleValidator in (IEnumerable<IRoleValidator<TRole>>) manager.RoleValidators)
      {
        IdentityResult identityResult = await roleValidator.ValidateAsync(manager, role).ConfigureAwait(false);
        if (!identityResult.Succeeded)
          errors.AddRange(identityResult.Errors);
      }
      if (errors.Count <= 0)
        return IdentityResult.Success;
      ILogger logger = manager.Logger;
      string str = await manager.GetRoleIdAsync(role).ConfigureAwait(false);
      logger.LogWarning(LoggerEventIds.RoleValidationFailed, "Role {roleId} validation failed: {errors}.", (object) str, (object) string.Join(";", errors.Select<IdentityError, string>((Func<IdentityError, string>) (e => e.Code))));
      logger = (ILogger) null;
      return IdentityResult.Failed(errors.ToArray());
    }

    /// <summary>
    /// Called to update the role after validating and updating the normalized role name.
    /// </summary>
    /// <param name="role">The role.</param>
    /// <returns>Whether the operation was successful.</returns>
    protected virtual async Task<IdentityResult> UpdateRoleAsync(TRole role)
    {
      ConfiguredTaskAwaitable<IdentityResult> configuredTaskAwaitable = this.ValidateRoleAsync(role).ConfigureAwait(false);
      IdentityResult identityResult = await configuredTaskAwaitable;
      if (!identityResult.Succeeded)
        return identityResult;
      await this.UpdateNormalizedRoleNameAsync(role).ConfigureAwait(false);
      configuredTaskAwaitable = this.Store.UpdateAsync(role, this.CancellationToken).ConfigureAwait(false);
      return await configuredTaskAwaitable;
    }


    #nullable disable
    private IRoleClaimStore<TRole> GetClaimStore() => this.Store is IRoleClaimStore<TRole> store ? store : throw new NotSupportedException(Resources.StoreNotIRoleClaimStore);

    /// <summary>Throws if this class has been disposed.</summary>
    protected void ThrowIfDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(this.GetType().Name);
    }
  }
}
