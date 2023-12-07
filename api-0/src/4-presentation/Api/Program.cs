using Chores.Api;
using Chores.Api.Modules;
using Chores.Application;
using Chores.Infrastructure;
using Chores.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence()
    .AddApi();

var app = builder.Build();

app
    .UseStatusCodePages()
    .UseExceptionHandler();
app
    .MapSwagger();
app
    .MapTagsEndpoints();

app.Run();