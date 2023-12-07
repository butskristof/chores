using Chores.Api;
using Chores.Api.Modules;
using Chores.Application;
using Chores.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddApplication()
    .AddInfrastructure()
    .AddApi();

var app = builder.Build();

app
    // the default exception handler will catch unhandled exceptions and return 
    // them as ProblemDetails with status code 500 Internal Server Error
    .UseExceptionHandler()
    // the status code pages will map additional failed requests (outside of
    // those throwing exceptions) to ProblemDetails responses
    // this includes 404, method not allowed, ...
    .UseStatusCodePages();

// add endpoint to retrieve OpenAPI definition
app.MapSwagger();
// add API endpoints
app.MapTagsEndpoints();

app.Run();