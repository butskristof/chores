using ChoresPoc.Api.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

app.UseFastEndpoints(config =>
{
	// in this POC, all endpoints are accessible without authorization
	config.Endpoints.Configurator = definition => definition.AllowAnonymous();
});
app.UseSwaggerGen();

app.Run();

// this makes the Program class available in the integration test project
public partial class Program { }