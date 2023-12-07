using Chores.Api;
using Chores.Api.Modules;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddApi();

var app = builder.Build();

app.MapSwagger();
app
    .MapTagsEndpoints();

app.Run();