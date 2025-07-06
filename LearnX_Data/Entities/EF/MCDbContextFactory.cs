using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LearnX_Data.EF
{
    public class MCDbContextFactory : IDesignTimeDbContextFactory<LearnXDbContext>
    {
        public LearnXDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("LearnX");

            var optionsBuilder = new DbContextOptionsBuilder<LearnXDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LearnXDbContext(optionsBuilder.Options);
        }
    }
}