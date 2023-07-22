namespace GarageBuddy.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using GarageBuddy.Data.Common.Models;
    using GarageBuddy.Data.Models;
    using GarageBuddy.Data.Models.Job;
    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #region "Jobs"
        public DbSet<Job> Jobs { get; set; } = null!;

        public DbSet<JobDocument> JobDocuments { get; set; } = null!;

        public DbSet<JobItem> JobItems { get; set; } = null!;

        public DbSet<JobItemPart> JobItemParts { get; set; } = null!;

        public DbSet<JobItemType> JobItemTypes { get; set; } = null!;

        public DbSet<JobStatus> JobStatus { get; set; } = null!;
        #endregion

        #region "Vehicles"
        public DbSet<Brand> Brands { get; set; } = null!;

        public DbSet<BrandModel> BrandModels { get; set; } = null!;

        public DbSet<DriveType> DriveTypes { get; set; } = null!;

        public DbSet<FuelType> FuelTypes { get; set; } = null!;

        public DbSet<GearboxType> GearboxTypes { get; set; } = null!;

        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        #endregion

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Garage> Garages { get; set; } = null!;

        public DbSet<Setting> Settings { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply entity configurations
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            var entityTypes = builder.Model.GetEntityTypes().ToList();
            var deletableEntityTypes = entityTypes
                .Where(et => typeof(IDeletableEntity).IsAssignableFrom(et.ClrType)).ToList();

            GlobalEntityConfiguration.AddIndexForDeletableEntities(builder, deletableEntityTypes);

            // Set global query filter for not deleted entities only
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            GlobalEntityConfiguration.DisableCascadeDelete(builder, entityTypes);
        }

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    e.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
