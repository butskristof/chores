using Chores.Domain.Models.Chores;
using Chores.Domain.Models.Tags;
using Microsoft.EntityFrameworkCore;

namespace Chores.Application.Common.Persistence;

public interface IAppDbContext
{
    DbSet<Tag> Tags { get; }
    DbSet<Chore> Chores { get; }
    IQueryable<Tag> CurrentUserTags(bool tracking);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}