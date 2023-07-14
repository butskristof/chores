using ChoresPoc.Api.Modules.Chores.Models;
using Microsoft.EntityFrameworkCore;

namespace ChoresPoc.Api.Data;

public class ChoreDbContext : DbContext
{
	public ChoreDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Chore> Chores => Set<Chore>();
}