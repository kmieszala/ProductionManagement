namespace ProductionManagement.Model
{
    using Microsoft.EntityFrameworkCore;
    using ProductionManagement.Model.Core;

    public class ProductionManagementContext : DbContext
    {

        public ProductionManagementContext(DbContextOptions<ProductionManagementContext> options)
            : base(options)
        {
        }

        public ProductionManagementContext(int userId, DbContextOptions<ProductionManagementContext> options)
            : base(options)
        {
            UserId = userId;
        }

        public int UserId { get; set; } = 0;

        public override int SaveChanges()
        {
            FillTrackableData();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            FillTrackableData();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        private void FillTrackableData()
        {
            var modified = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity as ITrackable)
                .ToArray();

            var added = ChangeTracker.Entries()
               .Where(t => t.State == EntityState.Added)
               .Select(t => t.Entity as ITrackable)
               .ToArray();

            if (modified != null && modified.Length > 0)
            {
                foreach (var entity in modified)
                {
                    if (entity is ITrackable)
                    {
                        entity.ModificationDate = DateTime.Now;
                        entity.ModificationUserId = UserId;
                    }
                }
            }

            if (added != null && added.Length > 0)
            {
                foreach (var entity in added)
                {
                    if (entity is ITrackable)
                    {
                        entity.ModificationDate = DateTime.Now;
                        entity.ModificationUserId = UserId;
                        entity.CreationDate = DateTime.Now;
                    }
                }
            }
        }
    }
}