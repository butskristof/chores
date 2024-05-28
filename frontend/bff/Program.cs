using Chores.Bff;
using Chores.Bff.Configuration;
using Duende.Bff;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
var isProduction = builder.Environment.IsProduction();

builder.Services
    .AddConfiguration()
    .AddChoresBff(isDevelopment);

var app = builder.Build();

app.UseForwardedHeaders();
if (isProduction) // serve SPA assets from wwwroot
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseRouting();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();
var remoteApiSettings = app.Services.GetRequiredService<IOptions<RemoteApiSettings>>().Value;
app.MapRemoteBffApiEndpoint("/api", remoteApiSettings.ChoresApiUrl)
    .RequireAccessToken(TokenType.User);

// serve index for everything that's not either a static asset or a configured route (bff, api proxy, ...)
if (isProduction)
    app.MapFallbackToFile("index.html");

app.Run();