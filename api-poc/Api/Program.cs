using ChoresPoc.Api.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();