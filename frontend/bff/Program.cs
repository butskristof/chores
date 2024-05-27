using Chores.Bff;
using Chores.Bff.Configuration;
using Duende.Bff;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();

builder.Services
    .AddConfiguration()
    .AddChoresBff(isDevelopment);

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();
var remoteApiSettings = app.Services.GetRequiredService<IOptions<RemoteApiSettings>>().Value;
app.MapRemoteBffApiEndpoint("/api", remoteApiSettings.ChoresApiUrl)
    .RequireAccessToken(TokenType.User);

app.Run();