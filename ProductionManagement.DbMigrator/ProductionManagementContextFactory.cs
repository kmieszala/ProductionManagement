namespace ProductionManagement.DbMigrator
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using ProductionManagement.Model;

    public class ProductionManagementContextFactory : IDesignTimeDbContextFactory<ProductionManagementContext>
    {
        public ProductionManagementContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ProductionManagementContext>();

            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(ProductionManagementContextFactory).Assembly.FullName));

            return new ProductionManagementContext(optionsBuilder.Options);
        }
    }
}