using ChoresPoc.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi();

var app = builder.Build();

app.MapSwagger();
if (app.Environment.IsDevelopment())
	app.UseSwaggerUI(); // no "MapSwaggerUI" available (yet)

app.Run();