using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RMS.EntityFrameworkCore
{

    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class RMSDbContextFactory : IDesignTimeDbContextFactory<RMSDbContext>
    {
        public RMSDbContext CreateDbContext(string[] args)
        {
            RMSEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<RMSDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new RMSDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../RMS.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}