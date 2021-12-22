using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot();

var app = builder.Build();

// Configure Logging
builder.WebHost.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(("Logging")));
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();