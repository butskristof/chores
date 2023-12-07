using Chores.Application.Common.Persistence;
using Chores.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chores.Persistence;

internal sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Tag> Tags => Set<Tag>();
}