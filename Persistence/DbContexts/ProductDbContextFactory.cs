using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.DbContexts
{
    public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {

        public ProductDbContextFactory()
        {

        }
        public ProductDbContext CreateDbContext(string[] args)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("default");
            //optionsBuilder.UseMySql(connectionString, serverVersion,
            //    mySqlOptions => mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
            //    );

            optionsBuilder.UseSqlServer(connectionString);


            return new ProductDbContext(optionsBuilder.Options);
        }
    }
}
