using Chores.Application.Common.Persistence;
using Chores.Domain.Models.Tags;
using Microsoft.EntityFrameworkCore;

namespace Chores.Persistence;

internal sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    #region entities

    // public DbSet<Chore> Chores => Set<Chore>();
    public DbSet<Tag> Tags => Set<Tag>();

    #endregion
}