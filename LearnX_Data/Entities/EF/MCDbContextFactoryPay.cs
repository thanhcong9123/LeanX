using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LearnX_Data.EF
{
    public class MCDbContextFactoryPay : IDesignTimeDbContextFactory<LearnXPayMentDbContext>
    {
        public LearnXPayMentDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("PayMent");

            var optionsBuilder = new DbContextOptionsBuilder<LearnXPayMentDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LearnXPayMentDbContext(optionsBuilder.Options);
        }
    }
}