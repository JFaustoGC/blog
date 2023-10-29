using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new Context(optionsBuilder.Options);
    }
}