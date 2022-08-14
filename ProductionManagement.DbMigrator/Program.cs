using Microsoft.EntityFrameworkCore;
using ProductionManagement.DbMigrator;

Console.WriteLine("Migration - START");

var dbFactory = new ProductionManagementContextFactory();
using (var db = dbFactory.CreateDbContext(null))
{
    db.Database.Migrate();
}

Console.WriteLine("Migration - STOP");
Console.WriteLine("Press any key");
Console.ReadKey();