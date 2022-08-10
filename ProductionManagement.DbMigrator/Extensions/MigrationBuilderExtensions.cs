namespace ProductionManagement.DbMigrator.Extensions
{
    using System.Reflection;
    using Microsoft.EntityFrameworkCore.Migrations;

    public static class MigrationBuilderExtensions
    {
        public static void SqlFromResource(this MigrationBuilder builder, string resourceName)
        {
            string sql = string.Empty;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    sql = reader.ReadToEnd();
                }
            }

            builder.Sql(sql);
        }
    }
}