using Chores.Domain.Models.Tags;
using Microsoft.EntityFrameworkCore;

namespace Chores.Application.Common.Persistence;

public interface IAppDbContext
{
    // public DbSet<Chore> Chores { get; }
    public DbSet<Tag> Tags { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}