using Chores.Api;
using Chores.Api.Modules;
using Chores.Application;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddApplication()
    .AddApi();

var app = builder.Build();

app.MapSwagger();
app
    .MapTagsEndpoints();

app.Run();