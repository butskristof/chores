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

app.MapSwagger();
app
    .MapTagsEndpoints();

app.Run();