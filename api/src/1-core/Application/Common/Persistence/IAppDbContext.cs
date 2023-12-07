using Chores.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chores.Application.Common.Persistence;

public interface IAppDbContext
{
    DbSet<Tag> Tags { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}