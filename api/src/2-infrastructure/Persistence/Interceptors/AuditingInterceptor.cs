using Chores.Application.Common.Authentication;
using Chores.Application.Common.Exceptions;
using Chores.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Chores.Persistence.Interceptors;

internal sealed class AuditingInterceptor : SaveChangesInterceptor
{
    #region construction

    private readonly TimeProvider _timeProvider;
    private readonly IAuthenticationInfo _authenticationInfo;

    public AuditingInterceptor(TimeProvider timeProvider, IAuthenticationInfo authenticationInfo)
    {
        _timeProvider = timeProvider;
        _authenticationInfo = authenticationInfo;
    }

    #endregion

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AuditEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AuditEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AuditEntities(DbContext? context)
    {
        if (context is null) return;

        var timestamp = _timeProvider.GetLocalNow();
        var userId = _authenticationInfo.UserId
                     ?? throw new AuthenticationException("Could not determine user ID for auditing");
        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedOn = timestamp;
            }

            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = userId;
                entry.Entity.LastModifiedOn = timestamp;
            }
        }
    }
}

internal static class Extensions
{
    internal static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}