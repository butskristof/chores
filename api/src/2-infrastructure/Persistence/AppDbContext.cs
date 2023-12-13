using System.Reflection;
using Chores.Application.Common.Authentication;
using Chores.Application.Common.Constants;
using Chores.Application.Common.Persistence;
using Chores.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chores.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    #region construction

    private readonly IAuthenticationInfo _authenticationInfo;

    public AppDbContext(DbContextOptions options, IAuthenticationInfo authenticationInfo) : base(options)
    {
        _authenticationInfo = authenticationInfo;
    }

    #endregion

    #region entities

    public DbSet<Tag> Tags => Set<Tag>();

    #endregion

    #region queryables

    public IQueryable<Tag> CurrentUserTags(bool tracking = false)
    {
        var query = Tags
            .Where(t => t.CreatedBy == _authenticationInfo.UserId)
            .AsQueryable();
        
        if (!tracking) query = query.AsNoTracking();
        
        return query;
    }

    #endregion

    #region configuration

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // the base method is empty, but retain the call to minimise impact if
        // it should be used in a future version
        base.ConfigureConventions(configurationBuilder);

        // set text fields to have a reduced maximum length by default 
        // this cuts down on a lot of varchar(max) columns, and can still be set to a higher 
        // maximum length on a per-column basis
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(ApplicationConstants.DefaultMaxStringLength);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // the base method is empty, but retain the call to minimise impact if
        // it should be used in a future version
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #endregion
}