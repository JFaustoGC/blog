using System;
using Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Blog.Startup))]
namespace Blog;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        builder.Services.AddDbContext<Context>(
            options => options.UseSqlServer(connectionString!));
    }
}