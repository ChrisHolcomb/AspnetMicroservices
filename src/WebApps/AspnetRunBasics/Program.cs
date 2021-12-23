using System;
using AspnetRunBasics.Data;
using AspnetRunBasics.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Database Dependency
builder.Services.AddDbContext<AspnetRunContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("AspnetRunConnection")));

// Add Repository Dependencies
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddRazorPages();

var app = builder.Build();

SeedDatabase(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();


static void SeedDatabase(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var aspnetRunContext = services.GetRequiredService<AspnetRunContext>();
        AspnetRunContextSeed.SeedAsync(aspnetRunContext, loggerFactory).Wait();
    }
    catch (Exception exception)
    {
        var logger = loggerFactory.CreateLogger<Microsoft.VisualStudio.Web.CodeGeneration.Design.Program>();
        logger.LogError(exception, "An error occurred seeding the DB.");
    }
}
