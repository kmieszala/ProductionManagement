using Microsoft.EntityFrameworkCore;
using ProductionManagement.DbMigrator;

var dbFactory = new ProductionManagementContextFactory();
Console.WriteLine("Migration - START");
using (var db = dbFactory.CreateDbContext(null))
{
    db.Database.Migrate();
}
Console.WriteLine("Migration - STOP");
Console.WriteLine("Press any key");
Console.ReadKey();

