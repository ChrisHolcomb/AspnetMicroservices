using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot()
    .AddCacheManager(settings =>
    {
        settings.WithDictionaryHandle();
    });


// Choose the correct Ocelot config file based on the environment
builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
});

// Configure Logging
builder.WebHost.ConfigureLogging((hostingContext, loggingBuilder) =>
{
    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(("Logging")));
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();