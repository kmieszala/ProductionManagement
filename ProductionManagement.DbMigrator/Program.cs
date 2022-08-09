using Microsoft.EntityFrameworkCore;
using ProductionManagement.DbMigrator;

static void Main(string[] args)
{
    var dbFactory = new ProductionManagementContextFactory();

    using (var db = dbFactory.CreateDbContext(null))
    {
        db.Database.Migrate();
    }
}
