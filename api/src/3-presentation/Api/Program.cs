using Chores.Api;
using Chores.Api.Modules;
using Chores.Application;
using Chores.Application.Common.Constants;
using Chores.Infrastructure;
using Chores.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddConfiguration(builder.Configuration)
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration.GetConnectionString(ConfigurationConstants.AppDbContextConnectionStringKey))
    .AddApi();

try
{
    var app = builder.Build();

    app
        // the default exception handler will catch unhandled exceptions and return 
        // them as ProblemDetails with status code 500 Internal Server Error
        .UseExceptionHandler()
        // the status code pages will map additional failed requests (outside of
        // those throwing exceptions) to responses with ProblemDetails body content
        // this includes 404, method not allowed, ... (all status codes between 400 and 599)
        // keep in mind that this middleware will only activate if the body is empty when
        // it reaches it
        .UseStatusCodePages();

    app.UseAuthorization();
    app.UseCors(ApplicationConstants.CorsPolicy);

    app.MapHealthChecks(ApplicationConstants.HealthCheckPattern);
    // add endpoint to retrieve OpenAPI definition
    app.MapSwagger();
    // add API endpoints
    app
        .MapTagsEndpoints()
        .MapChoresEndpoints();

    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
        catch (Exception)
        {
            // TODO log
            throw;
        }
    }

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    throw;
}