using ClamAVHttpProxy.Middleware;
using ClamAVHttpProxy.Models.Configuration;
using nClam;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new ClamClient(
    builder.Configuration.GetValue<string>("ClamAV:Host"),
    builder.Configuration.GetValue<int>("ClamAV:Port")));

builder.Services.Configure<AuthConfiguration>(
    builder.Configuration.GetRequiredSection("Auth"));

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();

app.MapControllers();

app.Run();
