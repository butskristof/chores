using System.Data.Common;
using ChoresPoc.Api.Data;
using ChoresPoc.Api.Tests.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChoresPoc.Api.Tests;

public class ApiFactory : WebApplicationFactory<Program>
{
	private readonly DbConnection _dbConnection;

	public ApiFactory()
	{
		_dbConnection = new SqliteConnection("Data Source=:memory:");
		_dbConnection.Open();
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders());

		builder.ConfigureTestServices(services =>
		{
			var descriptor = services.SingleOrDefault(d =>
				d.ServiceType == typeof(DbContextOptions<ChoreDbContext>));
			if (descriptor is not null)
				services.Remove(descriptor);
			services.RemoveAll(typeof(ChoreDbContext));

			services.AddDbContext<ChoreDbContext>(options => options.UseSqlite(_dbConnection));

			var serviceProvider = services.BuildServiceProvider();
			using var scope = serviceProvider.CreateScope();
			using var ctx = scope.ServiceProvider.GetRequiredService<ChoreDbContext>();
			ctx.Database.EnsureCreated();
			TestDataSeeder.Seed(ctx);
		});
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		_dbConnection.Close();
	}
}