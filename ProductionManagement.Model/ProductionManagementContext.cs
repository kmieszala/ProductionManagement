namespace ProductionManagement.Model
{
    using Microsoft.EntityFrameworkCore;
    using ProductionManagement.Common.Enums;
    using ProductionManagement.Common.Extensions;
    using ProductionManagement.Model.Core;
    using ProductionManagement.Model.DbSets;

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

        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<Users> Users { get; set; }

        public virtual DbSet<UserRoles> UserRoles { get; set; }

        public virtual DbSet<Tanks> Tanks { get; set; }

        public virtual DbSet<Log> Log { get; set; }

        public virtual DbSet<LogCodeDict> LogCodeDict { get; set; }

        public virtual DbSet<UserStatusDict> UserStatusDict { get; set; }

        public virtual DbSet<Parts> Parts { get; set; }

        public virtual DbSet<TankParts> TankParts { get; set; }

        public virtual DbSet<LineTank> LineTank { get; set; }

        public virtual DbSet<ProductionLine> ProductionLine { get; set; }

        public virtual DbSet<Orders> Orders { get; set; }

        public virtual DbSet<ProductionDays> ProductionDays { get; set; }

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

            modelBuilder
                .Entity<Users>()
                .HasIndex(e => e.Email)
                .IncludeProperties(e => new { e.Password });

            modelBuilder
                .Entity<Users>()
                .HasIndex(e => e.Code)
                .IsUnique();

            SeedDictionary<LogCodeEnum, LogCodeDict>(modelBuilder);
            SeedDictionary<UserStatusEnum, UserStatusDict>(modelBuilder);
            SeedDictionary<RolesEnum, Role>(modelBuilder);

            PredefineUser(modelBuilder);
        }

        private void PredefineUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(new
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                Status = UserStatusEnum.Active,
                Email = string.Empty,
                Password = "admin123".ComputeSha256Hash(),
                ActivationDate = DateTime.Now,
                RegisteredDate = DateTime.Now,
                TimeBlockCount = 0,
                Code = "0807"
            });

            modelBuilder.Entity<UserRoles>().HasData(new
            {
                Id = 1,
                UserId = 1,
                RoleId = RolesEnum.Administrator,
            });
        }

        private void SeedDictionary<TEnum, TEntity>(ModelBuilder modelBuilder)
            where TEnum : System.Enum
            where TEntity : class, new()
        {
            var type = typeof(TEntity);

            foreach (var e in System.Enum.GetValues(typeof(TEnum)))
            {
                var entity = new TEntity();

                type.GetProperty("Id")?.SetValue(entity, (TEnum)e, null);
                type.GetProperty("Name")?.SetValue(entity, ((TEnum)e).GetDisplayName(), null);
                type.GetProperty("Active")?.SetValue(entity, !IsObsolete((TEnum)e), null);

                modelBuilder.Entity<TEntity>().HasData(entity);
            }
        }

        private bool IsObsolete<TEnum>(TEnum value)
            where TEnum : System.Enum
        {
            var enumType = value.GetType();
            var enumName = enumType.GetEnumName(value);
            var fieldInfo = enumType.GetField(enumName!);
            return Attribute.IsDefined(fieldInfo!, typeof(ObsoleteAttribute));
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